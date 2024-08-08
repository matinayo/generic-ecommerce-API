using AutoMapper;
using HalceraAPI.Common.AppsettingsOptions;
using HalceraAPI.Common.Utilities;
using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;
using HalceraAPI.Services.Contract;
using HalceraAPI.Services.Dtos.ApplicationUser;
using HalceraAPI.Services.Dtos.Identity;
using HalceraAPI.Services.Dtos.RefreshToken;
using HalceraAPI.Services.Dtos.Role;
using HalceraAPI.Services.Token;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace HalceraAPI.Services.Operations
{
    public class IdentityOperation : IIdentityOperation
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JWTOptions jwtOptions;
        private readonly IEmailSenderOperation _emailSenderOperation;

        public IdentityOperation(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IOptions<JWTOptions> options,
            IEmailSenderOperation emailSenderOperation)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            jwtOptions = options.Value;
            _emailSenderOperation = emailSenderOperation;
        }

        public async Task<UserAuthResponse> Register(RegisterRequest registerRequest)
        {
            ApplicationUser? applicationUser = await GetUserWithEmail(registerRequest.Email);
            if (applicationUser != null)
            {
                throw new Exception("The email address entered is already being used.");
            }

            applicationUser = _mapper.Map<ApplicationUser>(registerRequest);
            applicationUser.Register(registerRequest.Password);
            await SetUserRole(registerRequest.RolesId, applicationUser);

            await _unitOfWork.ApplicationUser.Add(applicationUser);
            await _unitOfWork.SaveAsync();

            UserAuthResponse userResponse = _mapper.Map<UserAuthResponse>(applicationUser);
            userResponse.Token = JWTManager.CreateToken(applicationUser, jwtOptions.Token);

            return userResponse;
        }

        public async Task<UserAuthResponse> Login(LoginRequest loginRequest)
        {
            ApplicationUser applicationUserFromDb = await GetUserWithEmail(loginRequest.Email)
                ?? throw new Exception("Incorrect email or password");

            applicationUserFromDb.Login(loginRequest.Password
                ?? throw new Exception("Password is required"));
            await _unitOfWork.SaveAsync();

            UserAuthResponse userResponse = _mapper.Map<UserAuthResponse>(applicationUserFromDb);
            userResponse.Token = JWTManager.CreateToken(applicationUserFromDb, jwtOptions.Token);

            return userResponse;
        }

        public async Task<UserAuthResponse> RefreshToken(RefreshTokenRequest refreshTokenRequest)
        {
            ApplicationUser applicationUser = await GetLoggedInUserAsync();
            if (applicationUser.RefreshToken == null || !applicationUser.RefreshToken.Token.Equals(refreshTokenRequest.Token))
            {
                throw new UnauthorizedAccessException("Invalid Refresh Token");
            }
            else if (applicationUser.RefreshToken.DateExpires < DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("Token expired");
            }

            string token = JWTManager.CreateToken(applicationUser, jwtOptions.Token);
            applicationUser.GenerateRefreshToken();
            await _unitOfWork.SaveAsync();

            UserAuthResponse userResponse = _mapper.Map<UserAuthResponse>(applicationUser);
            userResponse.Token = token;

            return userResponse;
        }

        public async Task<ApplicationUser> GetLoggedInUserAsync()
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                var claim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (claim != null)
                {
                    ApplicationUser? applicationUser = await _unitOfWork.ApplicationUser
                        .GetFirstOrDefault(user => user.Id == claim.Value,
                        includeProperties: nameof(ApplicationUser.RefreshToken));

                    if (applicationUser != null)
                    {
                        return applicationUser;
                    }
                }
            }
            // TODO: create Unauthorized exception
            throw new UnauthorizedAccessException("Login to your account");
        }

        public async Task<IEnumerable<RoleResponse>> GetApplicationRoles()
        {
            return await _unitOfWork.Roles.GetAll<RoleResponse>();
        }

        public async Task<ApplicationUser?> GetUserWithEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new Exception("Email is required.");
            }

            ApplicationUser? userFromDb = await _unitOfWork.ApplicationUser
                .GetFirstOrDefault(user => user.Email.Trim().ToLower().Equals(email.Trim().ToLower()),
                includeProperties: nameof(ApplicationUser.Roles));

            return userFromDb;
        }

        private async Task SetUserRole(IEnumerable<RoleRequest>? rolesId, ApplicationUser applicationUser)
        {
            if (rolesId != null && rolesId.Any())
            {
                var selectedRoles = await _unitOfWork.Roles.GetAll(
                    role => rolesId.Select(opt => opt.Id).Contains(role.Id));

                if (selectedRoles != null && selectedRoles.Any())
                {
                    applicationUser.Roles = selectedRoles.ToList();
                }
            }

            if (applicationUser.Roles == null)
            {
                Roles? roleId = await _unitOfWork.Roles.GetFirstOrDefault(u => u.Title.Equals(RoleDefinition.Customer));
                if (roleId != null)
                {
                    applicationUser.Roles = new List<Roles> { roleId };
                }
            }
        }

        public async Task ForgotPassword(string email)
        {
            var user = await _unitOfWork.ApplicationUser
                .GetFirstOrDefault(user => user.Email.Trim().ToLower().Equals(email.Trim().ToLower()));

            if (user != null)
            {
                user.PasswordResetToken = CreateRandomToken();
                user.ResetTokenExpires = DateTime.UtcNow.AddHours(1);
                await _unitOfWork.SaveAsync();

                await _emailSenderOperation.SendEmailAsync(
                    "madeayo04@gmail.com", //user.Email, 
                    EmailConstants.ForgotPasswordSubject,
                    EmailConstants.ForgotPasswordPlainTextMessage(user.PasswordResetToken),
                    EmailConstants.ForgotPasswordHtmlMessage);
            }
        }

        public async Task ResetUserPassword(ResetUserPasswordRequest resetUserPasswordRequest)
        {
            var user = await _unitOfWork.ApplicationUser
                .GetFirstOrDefault(user => user.Email.Trim().ToLower().Equals(resetUserPasswordRequest.Email.Trim().ToLower()));

            if (user != null && user.PasswordResetToken != null && DateTime.UtcNow < user.ResetTokenExpires)
            {
                if (resetUserPasswordRequest.OTP.Trim().Equals(user.PasswordResetToken.Trim()))
                {
                    user.ResetPassword(resetUserPasswordRequest.Password);
                    await _unitOfWork.SaveAsync();

                    return;
                }
            }

            throw new Exception("Invalid Token");
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }

        private static string CreateRandomToken()
        {
            return Guid.NewGuid().ToString()[..5];
        }
    }
}
