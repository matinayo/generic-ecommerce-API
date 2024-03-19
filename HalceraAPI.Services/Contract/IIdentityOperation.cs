using HalceraAPI.Models;
using HalceraAPI.Models.Enums;
using HalceraAPI.Models.Requests.ApplicationUser;
using HalceraAPI.Models.Requests.RefreshToken;
using HalceraAPI.Models.Requests.Role;

namespace HalceraAPI.Services.Contract
{
    public interface IIdentityOperation
    {
        Task<UserAuthResponse> Register(RegisterRequest registerRequest);
        Task<UserAuthResponse> Login(LoginRequest loginRequest);
        Task<UserAuthResponse> RefreshToken(RefreshTokenRequest refreshTokenRequest);
        Task<ApplicationUser> GetLoggedInUserAsync();
        Task<IEnumerable<RoleResponse>> GetApplicationRoles();
        Task<ApplicationUser?> GetUserWithEmail(string? email);
    }
}
