using HalceraAPI.Common.Utilities;
using HalceraAPI.Models.Enums;
using HalceraAPI.Models.Requests.Shipping;
using HalceraAPI.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HalceraAPI.Areas.Admin.Controllers
{
    [Authorize(Roles = $"{RoleDefinition.Admin},{RoleDefinition.Employee}")]
    [Area("admin")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class ShippingsController : ControllerBase
    {
        private readonly IShippingOperation _shippingOperation;

        public ShippingsController(IShippingOperation shippingOperation)
        {
            _shippingOperation = shippingOperation;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ShippingDetailsResponse>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<ShippingDetailsResponse>>> GetAllShippingRequestsAsync(ShippingStatus? shippingStatus)
        {
            try
            {
                IEnumerable<ShippingDetailsResponse> shippingDetails = await _shippingOperation.GetAllShippingRequestsAsync(shippingStatus);

                return Ok(shippingDetails);
            }
            catch (Exception exception)
            {
                return BadRequest(
                    Problem(
                        statusCode: StatusCodes.Status400BadRequest,
                        detail: exception?.InnerException?.Message ?? exception?.Message));
            }
        }

        [HttpPut("{shippingId}")]
        [ProducesResponseType(typeof(ShippingDetailsResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ShippingDetailsResponse>> UpdateShippingDetailsAsync
            (int shippingId, UpdateShippingDetailsRequest shippingDetailsRequest)
        {
            try
            {
                ShippingDetailsResponse shippingDetails = await _shippingOperation.UpdateShippingDetailsAsync(shippingId, shippingDetailsRequest);

                return Ok(shippingDetails);
            }
            catch (Exception exception)
            {
                return BadRequest(
                    Problem(
                        statusCode: StatusCodes.Status400BadRequest,
                        detail: exception?.InnerException?.Message ?? exception?.Message));
            }
        }
    }
}
