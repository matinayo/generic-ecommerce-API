using HalceraAPI.Common.Enums;
using HalceraAPI.Services.Dtos.APIResponse;
using HalceraAPI.Services.Dtos.ApplicationUser;
using HalceraAPI.Services.Dtos.BaseAddress;
using HalceraAPI.Services.Dtos.Role;

namespace HalceraAPI.Services.Contract
{
    public interface IUserOperation
    {
        Task<APIResponse<IEnumerable<UserResponse>>> GetUsersAsync(
            int? roleId,
            bool? active, 
            bool? deleted, 
            int? page);
        Task<APIResponse<UserDetailsResponse>> GetUserByIdAsync(string userId);
        Task<APIResponse<UserDetailsResponse>> UpdateUserDetailsAsync(string userId, UpdateUserRequest updateUserRequest);
        Task <APIResponse<AddressResponse>>UpdateAddressAsync(string userId, AddressRequest updateAddressRequest);
        Task DeleteAccountAsync(string userId);
        Task LockUnlockUserAsync(string userId, AccountAction accountAction);
        Task <APIResponse<UserAuthResponse>> DeleteRoleFromUserAsync(string userId, int roleId);
        Task<APIResponse<UserAuthResponse>> UpdateUserRoleUserAsync(string userId, int roleId);
    }
}
