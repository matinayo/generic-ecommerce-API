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
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace HalceraAPI.Services.Operations
{
    public class IdentityOperation : IIdentityOperation
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JWTOptions jwtOptions;

        public IdentityOperation(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IOptions<JWTOptions> options)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            jwtOptions = options.Value;
        }

        public async Task<UserResponse> Register(RegisterRequest registerRequest)
        {
            try
            {
                // validate if user email is already existing
                ApplicationUser? applicationUser = await GetUserWithEmail(registerRequest.Email);
                if (applicationUser != null)
                {
                    throw new Exception("The email address entered is already being used. Please select another.");
                }

                string passwordHash = ValidateAndCreateUserPassword(registerRequest.Password, registerRequest.ConfirmPassword);
                applicationUser = new()
                {
                    PasswordHash = passwordHash
                };
                _mapper.Map(registerRequest, applicationUser);

                await SetUserRole(registerRequest.RolesId, applicationUser);

                applicationUser.LastLoginDate = DateTime.UtcNow;
                applicationUser.RefreshToken = GenerateRefreshToken();

                await _unitOfWork.ApplicationUser.Add(applicationUser);
                await _unitOfWork.SaveAsync();

                UserResponse userResponse = _mapper.Map<UserResponse>(applicationUser);
                string token = CreateToken(applicationUser);
                userResponse.Token = token;

                return userResponse;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserResponse> Login(LoginRequest loginRequest)
        {
            try
            {
                ApplicationUser? applicationUserFromDb = await GetUserWithEmail(loginRequest.Email);
                if (applicationUserFromDb == null)
                {
                    throw new Exception("Incorrect email or password.");
                }
                VerifyPassword(loginRequest.Password, applicationUserFromDb.PasswordHash);
                ValidateAccountStatus(applicationUserFromDb);

                applicationUserFromDb.LastLoginDate = DateTime.UtcNow;
                applicationUserFromDb.RefreshToken = GenerateRefreshToken();

                await _unitOfWork.SaveAsync();

                UserResponse userResponse = _mapper.Map<UserResponse>(applicationUserFromDb);
                string token = CreateToken(applicationUserFromDb);
                userResponse.Token = token;

                return userResponse;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserResponse> RefreshToken(RefreshTokenRequest refreshTokenRequest)
        {
            ApplicationUser applicationUser = await GetLoggedInUser();
            if (applicationUser.RefreshToken == null || !applicationUser.RefreshToken.Token.Equals(refreshTokenRequest.Token))
            {
                throw new UnauthorizedAccessException("Invalid Refresh Token");
            }
            else if (applicationUser.RefreshToken.DateExpires < DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("Token expired");
            }

            string token = CreateToken(applicationUser);
            applicationUser.RefreshToken = GenerateRefreshToken();

            await _unitOfWork.SaveAsync();
            UserResponse userResponse = _mapper.Map<UserResponse>(applicationUser);
            userResponse.Token = token;

            return userResponse;
        }

        public async Task<ApplicationUser> GetLoggedInUser()
        {
            try
            {
                if (_httpContextAccessor.HttpContext != null)
                {
                    var claim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                    if (claim != null)
                    {
                        ApplicationUser? applicationUser = await _unitOfWork.ApplicationUser.GetFirstOrDefault(user => user.Id == claim.Value, includeProperties: nameof(ApplicationUser.RefreshToken));
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
                var roles = await _unitOfWork.Roles.GetAll();
                return _mapper.Map<IEnumerable<RoleResponse>>(roles);
            }catch(Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get user email including roles
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>ApplicaitonUser associated with email</returns>
        private async Task<ApplicationUser?> GetUserWithEmail(string? email)
        {

            if (string.IsNullOrWhiteSpace(email))
            {
                throw new Exception("Email is required.");
            }

            ApplicationUser? userFromDb = await _unitOfWork.ApplicationUser
                .GetFirstOrDefault(user => user.Email.Trim().ToLower().Equals(email.Trim().ToLower()), includeProperties: nameof(ApplicationUser.Roles));
            return userFromDb;
        }

        public bool ValidateUsingRegex(string emailAddress)
        {
            var pattern = @"^[a-zA-Z0-9.!#$%&'*+-/=?^_`{|}~]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$";

            var regex = new Regex(pattern);
            return regex.IsMatch(emailAddress);
        }

        /// <summary>
        /// Validate user password request, and create Password Hash
        /// </summary>
        /// <param name="password">Password</param>
        /// <param name="confirmPassword">Confirm password</param>
        /// <returns>Password Hash</returns>
        private static string ValidateAndCreateUserPassword(string? password, string? confirmPassword)
        {
            ValidatePassword(password, confirmPassword);
            string passwordHash = CreatePasswordHash(password!);
            return passwordHash;
        }

        /// <summary>
        /// Validate password and password format
        /// </summary>
        /// <param name="password">Password</param>
        /// <param name="confirmPassword">Confirm Password</param>
        private static void ValidatePassword(string? password, string? confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                throw new Exception("Password and Confirm Password are required");
            }
            if (!password.Equals(confirmPassword))
            {
                throw new Exception("Passwords do not match");
            }
        }

        /// <summary>
        /// Returns created password hash
        /// </summary>
        /// <param name="password">Password string request</param>
        /// <returns>Password Hash</returns>
        private static string CreatePasswordHash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        /// <summary>
        /// Verify user password
        /// </summary>
        /// <param name="requestedPassword">Input password from user</param>
        /// <param name="applicationUserPasswordHash">Application user password hash with associated email</param>
        private static void VerifyPassword(string? requestedPassword, string applicationUserPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(requestedPassword) || !BCrypt.Net.BCrypt.Verify(requestedPassword, applicationUserPasswordHash))
            {
                throw new Exception("Incorrect email or password.");
            }
        }

        /// <summary>
        /// Validating Account status and preferences 
        /// </summary>
        /// <param name="applicationUser">Application User from db</param>
        private static void ValidateAccountStatus(ApplicationUser applicationUser)
        {
            bool accountIsInactive = !applicationUser.Active;
            if (applicationUser.LockoutEnd != null)
            {
                DateTime lockoutEnd = applicationUser.LockoutEnd.GetValueOrDefault();
                int result = DateTime.Compare(lockoutEnd, DateTime.UtcNow);
                if (result >= 0)
                {
                    accountIsInactive = true;
                }
            }
            if (accountIsInactive)
            {
                // account is inactive
                throw new Exception("Account is inactive");
            }
        }

        /// <summary>
        /// Create Json Web Token for user
        /// </summary>
        /// <param name="applicationUser">User from Db</param>
        /// <returns>Json Token</returns>
        private string CreateToken(ApplicationUser applicationUser)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, applicationUser.Name ?? string.Empty),
                new Claim(ClaimTypes.NameIdentifier, applicationUser.Id ?? string.Empty),
                new Claim(ClaimTypes.Email, applicationUser.Email)
            };

            if(applicationUser.Roles != null && applicationUser.Roles.Any())
            {
                foreach(var role in applicationUser.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Title));
                }
            }

            // key to create and verify JWT
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Token));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(claims: claims, expires: DateTime.UtcNow.AddDays(29), signingCredentials: credentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        /// <summary>
        /// Generate Refresh Token
        /// </summary>
        /// <returns>New refresh token</returns>
        private static RefreshToken GenerateRefreshToken()
        {
            return new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                DateExpires = DateTime.UtcNow.AddDays(29),
                DateCreated = DateTime.UtcNow
            };
        }

        private async Task SetUserRole(IEnumerable<RoleRequest>? rolesId, ApplicationUser applicationUser)
        {
            if (rolesId != null && rolesId.Any())
            {
                var selectedRoles = await _unitOfWork.Roles.GetAll(role => rolesId.Select(opt => opt.Id).Contains(role.Id));
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
