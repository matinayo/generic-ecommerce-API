using HalceraAPI.Models.Enums;
using HalceraAPI.Models.Requests.APIResponse;
using HalceraAPI.Models.Requests.ApplicationUser;
using HalceraAPI.Models.Requests.BaseAddress;

namespace HalceraAPI.Services.Contract
{
    public interface IUserOperation
    {
        Task<APIResponse<IEnumerable<UserResponse>>> GetUsersAsync(
            int? roleId,
            bool? active, 
            bool? deleted, 
            int? page);
        Task<APIResponse<UserResponse>> GetUserByIdAsync(string userId);
        Task<APIResponse<UserResponse>> UpdateUserDetailsAsync(string userId, UpdateUserRequest updateUserRequest);
        Task <APIResponse<AddressResponse>>UpdateAddressAsync(string userId, AddressRequest updateAddressRequest);
        Task DeleteAccountAsync(string userId);

        Task LockUnlockUserAsync(string userId, AccountAction accountAction);
    }
}
