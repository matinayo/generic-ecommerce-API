using HalceraAPI.Models;
using HalceraAPI.Models.Requests.ApplicationUser;

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

        public Task<UserResponse> Login(LoginRequest loginRequest);
    }
}
