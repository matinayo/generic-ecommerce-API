using HalceraAPI.Models;
using HalceraAPI.Models.Enums;
using HalceraAPI.Models.Requests.ApplicationUser;
using HalceraAPI.Models.Requests.RefreshToken;
using HalceraAPI.Models.Requests.Role;

namespace HalceraAPI.Services.Contract
{
    public interface IIdentityOperation
    {
        Task<UserResponse> Register(RegisterRequest registerRequest);

        Task<UserResponse> Login(LoginRequest loginRequest);

        Task<UserResponse> RefreshToken(RefreshTokenRequest refreshTokenRequest);

        Task<ApplicationUser> GetLoggedInUserAsync();

        Task<IEnumerable<RoleResponse>> GetApplicationRoles();

        Task LockUnlockUserAsync(string userId, AccountAction accountAction);
    }
}
