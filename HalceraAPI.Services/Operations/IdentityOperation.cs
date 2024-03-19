using AutoMapper;
using HalceraAPI.Common.AppsettingsOptions;
using HalceraAPI.Common.Utilities;
using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;
using HalceraAPI.Models.Requests.ApplicationUser;
using HalceraAPI.Models.Requests.RefreshToken;
using HalceraAPI.Models.Requests.Role;
using HalceraAPI.Services.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace HalceraAPI.Services.Operations
{
    public class IdentityOperation : IIdentityOperation
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JWTOptions jwtOptions;

        public IdentityOperation(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IHttpContextAccessor httpContextAccessor, 
            IOptions<JWTOptions> options)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            jwtOptions = options.Value;
        }

        public async Task<UserAuthResponse> Register(RegisterRequest registerRequest)
        {
            try
            {
                // validate if user email is already existing
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
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserAuthResponse> Login(LoginRequest loginRequest)
        {
            try
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
            catch (Exception)
            {
                throw;
            }
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
            try
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
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<RoleResponse>> GetApplicationRoles()
        {
            try
            {
                return await _unitOfWork.Roles.GetAll<RoleResponse>();
            }
            catch (Exception)
            {
                throw;
            }
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
                // assign new role
                Roles? roleId = await _unitOfWork.Roles.GetFirstOrDefault(u => u.Title.Equals(RoleDefinition.Customer));
                if (roleId != null)
                {
                    applicationUser.Roles = new List<Roles> { roleId };
                }
            }
        }
    }
}
