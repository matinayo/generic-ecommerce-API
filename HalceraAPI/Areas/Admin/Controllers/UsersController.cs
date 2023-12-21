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
            try
            {
                APIResponse<IEnumerable<UserResponse>> usersResponse =
                    await _userOperation.GetUsersAsync(roleId, active, deleted, page);

                return Ok(usersResponse);
            }
            catch (Exception exception)
            {
                return BadRequest(
                        Problem(
                            statusCode: StatusCodes.Status400BadRequest,
                            detail: exception.InnerException?.Message ?? exception.Message));
            }
        }

        [HttpGet]
        [Route("{userId}")]
        [ProducesResponseType(typeof(APIResponse<UserResponse>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> GetUserByIdAsync(string userId)
        {
            try
            {
                APIResponse<UserResponse> userResponse = await _userOperation.GetUserByIdAsync(userId);

                return Ok(userResponse);
            }
            catch (Exception exception)
            {
                return BadRequest(
                        Problem(
                            statusCode: StatusCodes.Status400BadRequest,
                            detail: exception.InnerException?.Message ?? exception.Message));
            }
        }

        [Authorize(Roles = RoleDefinition.Admin + "," + RoleDefinition.Employee)]
        [HttpPut]
        [Route("{userId}/{accountAction}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> LockUnlockAccountAsync(string userId, AccountAction accountAction)
        {
            try
            {
                await _userOperation.LockUnlockUserAsync(userId, accountAction);

                return NoContent();
            }
            catch (Exception exception)
            {
                return BadRequest(
                        Problem(
                            statusCode: StatusCodes.Status400BadRequest,
                            detail: exception.InnerException?.Message ?? exception.Message));
            }
        }

        [HttpPut]
        [Route("{userId}")]
        [ProducesResponseType(typeof(APIResponse<UserResponse>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> UpdateUserDetailsAsync(string userId, UpdateUserRequest updateUserRequest)
        {
            try
            {
                APIResponse<UserResponse> userResponse =
                    await _userOperation.UpdateUserDetailsAsync(userId, updateUserRequest);

                return Ok(userResponse);
            }
            catch (Exception exception)
            {
                return BadRequest(
                        Problem(
                            statusCode: StatusCodes.Status400BadRequest,
                            detail: exception.InnerException?.Message ?? exception.Message));
            }
        }

        [HttpPut]
        [Route("{userId}/Address")]
        [ProducesResponseType(typeof(APIResponse<AddressResponse>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> UpdateUserAddressAsync(string userId, AddressRequest updateAddressRequest)
        {
            try
            {
                APIResponse<AddressResponse> addressResponse =
                    await _userOperation.UpdateAddressAsync(userId, updateAddressRequest);

                return Ok(addressResponse);
            }
            catch (Exception exception)
            {
                return BadRequest(
                        Problem(
                            statusCode: StatusCodes.Status400BadRequest,
                            detail: exception.InnerException?.Message ?? exception.Message));
            }
        }

        [Authorize(Roles = RoleDefinition.Admin)]
        [HttpPut]
        [Route("{userId}/Roles/{roleId}")]
        [ProducesResponseType(typeof(APIResponse<UserAuthResponse>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> UpdateUserRoleUserAsync(string userId, int roleId)
        {
            try
            {
                APIResponse<UserAuthResponse> userResponse =
                    await _userOperation.UpdateUserRoleUserAsync(userId, roleId);

                return Ok(userResponse);
            }
            catch (Exception exception)
            {
                return BadRequest(
                        Problem(
                            statusCode: StatusCodes.Status400BadRequest,
                            detail: exception.InnerException?.Message ?? exception.Message));
            }
        }

        [HttpDelete]
        [Route("{userId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> DeleteAccountAsync(string userId)
        {
            try
            {
                await _userOperation.DeleteAccountAsync(userId);

                return NoContent();
            }
            catch (Exception exception)
            {
                return BadRequest(
                        Problem(
                            statusCode: StatusCodes.Status400BadRequest,
                            detail: exception.InnerException?.Message ??
                            exception.Message));
            }
        }

        [Authorize(Roles = RoleDefinition.Admin)]
        [HttpDelete]
        [Route("{userId}/Roles/{roleId}")]
        [ProducesResponseType(typeof(APIResponse<UserAuthResponse>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> DeleteRoleFromUserAsync(string userId, int roleId)
        {
            try
            {
                APIResponse<UserAuthResponse> userResponse =
                    await _userOperation.DeleteRoleFromUserAsync(userId, roleId);

                return Ok(userResponse);
            }
            catch (Exception exception)
            {
                return BadRequest(
                        Problem(
                            statusCode: StatusCodes.Status400BadRequest,
                            detail: exception.InnerException?.Message ??
                            exception.Message));
            }
        }
    }
}
