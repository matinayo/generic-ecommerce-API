using HalceraAPI.Models.Requests.ApplicationUser;

namespace HalceraAPI.Services.Contract
{
    public interface IUserOperation
    {
        Task<IEnumerable<UserAuthResponse>> GetUsersAsync(int? roleId, bool? active, int? page);
        Task<UserAuthResponse> EditUserDetailsAsync(int? userId);
    }
}
