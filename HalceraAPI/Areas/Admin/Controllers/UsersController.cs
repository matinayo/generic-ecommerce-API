using HalceraAPI.Common.Utilities;
using HalceraAPI.Models.Enums;
using HalceraAPI.Models.Requests.APIResponse;
using HalceraAPI.Models.Requests.ApplicationUser;
using HalceraAPI.Models.Requests.BaseAddress;
using HalceraAPI.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HalceraAPI.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserOperation _userOperation;

        public UsersController(IUserOperation userOperation)
        {
            _userOperation = userOperation;
        }

        [Authorize(Roles = RoleDefinition.Admin + "," + RoleDefinition.Employee)]
        [HttpGet]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<UserResponse>>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> GetUsersAsync(
            int? roleId,
            bool? active,
            bool? deleted,
            int? page)
        {
            APIResponse<IEnumerable<UserResponse>> usersResponse =
                await _userOperation.GetUsersAsync(roleId, active, deleted, page);

            return Ok(usersResponse);
        }

        [HttpGet]
        [Route("{userId}")]
        [ProducesResponseType(typeof(APIResponse<UserDetailsResponse>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> GetUserByIdAsync(string userId)
        {
            APIResponse<UserDetailsResponse> userResponse = await _userOperation.GetUserByIdAsync(userId);

            return Ok(userResponse);
        }

        [Authorize(Roles = RoleDefinition.Admin + "," + RoleDefinition.Employee)]
        [HttpPut]
        [Route("{userId}/{accountAction}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> LockUnlockAccountAsync(string userId, AccountAction accountAction)
        {
            await _userOperation.LockUnlockUserAsync(userId, accountAction);

            return NoContent();
        }

        [HttpPut]
        [Route("{userId}")]
        [ProducesResponseType(typeof(APIResponse<UserDetailsResponse>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> UpdateUserDetailsAsync(string userId, UpdateUserRequest updateUserRequest)
        {
            APIResponse<UserDetailsResponse> userResponse =
                await _userOperation.UpdateUserDetailsAsync(userId, updateUserRequest);

            return Ok(userResponse);
        }

        [HttpPut]
        [Route("{userId}/Address")]
        [ProducesResponseType(typeof(APIResponse<AddressResponse>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> UpdateUserAddressAsync(string userId, AddressRequest updateAddressRequest)
        {
            APIResponse<AddressResponse> addressResponse =
                await _userOperation.UpdateAddressAsync(userId, updateAddressRequest);

            return Ok(addressResponse);
        }

        [Authorize(Roles = RoleDefinition.Admin)]
        [HttpPut]
        [Route("{userId}/Roles/{roleId}")]
        [ProducesResponseType(typeof(APIResponse<UserAuthResponse>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> UpdateUserRoleUserAsync(string userId, int roleId)
        {
            APIResponse<UserAuthResponse> userResponse =
                await _userOperation.UpdateUserRoleUserAsync(userId, roleId);

            return Ok(userResponse);
        }

        [HttpDelete]
        [Route("{userId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> DeleteAccountAsync(string userId)
        {
            await _userOperation.DeleteAccountAsync(userId);

            return NoContent();
        }

        [Authorize(Roles = RoleDefinition.Admin)]
        [HttpDelete]
        [Route("{userId}/Roles/{roleId}")]
        [ProducesResponseType(typeof(APIResponse<UserAuthResponse>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> DeleteRoleFromUserAsync(string userId, int roleId)
        {
            APIResponse<UserAuthResponse> userResponse =
                await _userOperation.DeleteRoleFromUserAsync(userId, roleId);

            return Ok(userResponse);
        }
    }
}
