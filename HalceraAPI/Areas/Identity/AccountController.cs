using HalceraAPI.Common.Utilities;
using HalceraAPI.Models.Enums;
using HalceraAPI.Models.Requests.ApplicationUser;
using HalceraAPI.Models.Requests.RefreshToken;
using HalceraAPI.Models.Requests.Role;
using HalceraAPI.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HalceraAPI.Areas.Identity
{
    [Area("Identity")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IIdentityOperation _applicationUserOperation;
        public AccountController(IIdentityOperation applicationUserOperation)
        {
            _applicationUserOperation = applicationUserOperation;
        }

        [HttpPost]
        [Route("Register")]
        [ProducesResponseType(typeof(UserResponse), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<UserResponse>> Register([FromBody] RegisterRequest registerRequest)
        {
            try
            {
                UserResponse applicationUser = await _applicationUserOperation.Register(registerRequest);

                return Ok(applicationUser);
            }
            catch (Exception exception)
            {
                return BadRequest(
                        Problem(
                            statusCode: StatusCodes.Status400BadRequest,
                            detail: exception.InnerException?.Message ?? exception.Message));
            }
        }

        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(typeof(UserResponse), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<UserResponse>> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                UserResponse applicationUser = await _applicationUserOperation.Login(loginRequest);
                return Ok(applicationUser);
            }
            catch (Exception exception)
            {
                return BadRequest(
                        Problem(
                            statusCode: StatusCodes.Status400BadRequest,
                            detail: exception.InnerException?.Message ?? exception.Message));
            }
        }

        [Authorize]
        [HttpPost]
        [Route("RefreshToken")]
        [ProducesResponseType(typeof(UserResponse), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<UserResponse>> RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            try
            {
                UserResponse applicationUser = await _applicationUserOperation.RefreshToken(refreshTokenRequest);
                return Ok(applicationUser);
            }
            catch (Exception exception)
            {
                return BadRequest(
                        Problem(
                            statusCode: StatusCodes.Status400BadRequest,
                            detail: exception.InnerException?.Message ?? exception.Message));
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetRoles")]
        [ProducesResponseType(typeof(IEnumerable<RoleResponse>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<RoleResponse>>> GetRoles()
        {
            try
            {
                IEnumerable<RoleResponse> roles = await _applicationUserOperation.GetApplicationRoles();
                return Ok(roles);
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
                await _applicationUserOperation.LockUnlockUserAsync(userId, accountAction);

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
    }
}
