using HalceraAPI.Common.Utilities;
using HalceraAPI.Common.Enums;
using HalceraAPI.Services.Dtos.APIResponse;
using HalceraAPI.Services.Dtos.Shipping;
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
        [ProducesResponseType(typeof(APIResponse<IEnumerable<ShippingDetailsResponse>>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> GetAllShippingRequestsAsync(ShippingStatus? shippingStatus, int? page)
        {
                APIResponse<IEnumerable<ShippingDetailsResponse>> shippingDetails =
                    await _shippingOperation.GetAllShippingRequestsAsync(shippingStatus, page);

                return Ok(shippingDetails);
        }

        [HttpPut("{shippingId}")]
        [ProducesResponseType(typeof(APIResponse<ShippingDetailsResponse>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateShippingDetailsAsync
            (int shippingId, UpdateShippingDetailsRequest shippingDetailsRequest)
        {
                APIResponse<ShippingDetailsResponse> shippingDetails = 
                    await _shippingOperation.UpdateShippingDetailsAsync(shippingId, shippingDetailsRequest);

                return Ok(shippingDetails);
        }
    }
}
