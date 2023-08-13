using HalceraAPI.Models;
using HalceraAPI.Models.Requests.ApplicationUser;
using HalceraAPI.Models.Requests.RefreshToken;
using HalceraAPI.Models.Requests.Role;

namespace HalceraAPI.Services.Contract
{
    public interface IIdentityOperation
    {
        /// <summary>
        /// Register User
        /// </summary>
        /// <param name="registerRequest">Register user request</param>
        /// <returns>User information</returns>
        public Task<UserResponse> Register(RegisterRequest registerRequest);

        /// <summary>
        /// Login Request
        /// </summary>
        /// <param name="loginRequest">Login user request</param>
        /// <returns>User Information</returns>
        public Task<UserResponse> Login(LoginRequest loginRequest);

        /// <summary>
        /// Refresh the refresh token; generate new JWT and Refresh Token
        /// </summary>
        /// <returns>User response with new refresh token</returns>
        public Task<UserResponse> RefreshToken(RefreshTokenRequest refreshTokenRequest);

        /// <summary>
        /// Get Logged-In user from JWT claims
        /// </summary>
        /// <returns>Current application user</returns>
        public Task<ApplicationUser> GetLoggedInUser();

        /// <summary>
        /// Get all roles
        /// </summary>
        /// <returns>List of application roles</returns>
        public Task<IEnumerable<RoleResponse>> GetApplicationRoles();
    }
}
