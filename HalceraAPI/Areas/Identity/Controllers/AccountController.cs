using HalceraAPI.Services.Dtos.ApplicationUser;
using HalceraAPI.Services.Dtos.RefreshToken;
using HalceraAPI.Services.Dtos.Role;
using HalceraAPI.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HalceraAPI.Services.Dtos.Identity;

namespace HalceraAPI.Areas.Identity.Controllers
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
        [ProducesResponseType(typeof(UserAuthResponse), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<UserAuthResponse>> Register([FromBody] RegisterRequest registerRequest)
        {
            UserAuthResponse applicationUser = await _applicationUserOperation.Register(registerRequest);

            return Ok(applicationUser);
        }

        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(typeof(UserAuthResponse), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<UserAuthResponse>> Login([FromBody] LoginRequest loginRequest)
        {
            UserAuthResponse applicationUser = await _applicationUserOperation.Login(loginRequest);

            return Ok(applicationUser);
        }

        [HttpPost]
        [Route("ForgotPassword")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> ForgotPassword(string email)
        {
            await _applicationUserOperation.ForgotPassword(email);

            return Ok();
        }

        [HttpPost]
        [Route("ResetPassword")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> ResetPassword(ResetUserPasswordRequest resetUserPasswordRequest)
        {
            await _applicationUserOperation.ResetUserPassword(resetUserPasswordRequest);

            return Ok();
        }

        [Authorize]
        [HttpPost]
        [Route("RefreshToken")]
        [ProducesResponseType(typeof(UserAuthResponse), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<UserAuthResponse>> RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            UserAuthResponse applicationUser = await _applicationUserOperation.RefreshToken(refreshTokenRequest);

            return Ok(applicationUser);
        }

        [Authorize]
        [HttpGet]
        [Route("GetRoles")]
        [ProducesResponseType(typeof(IEnumerable<RoleResponse>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<RoleResponse>>> GetRoles()
        {
            IEnumerable<RoleResponse> roles = await _applicationUserOperation.GetApplicationRoles();

            return Ok(roles);
        }
    }
}
