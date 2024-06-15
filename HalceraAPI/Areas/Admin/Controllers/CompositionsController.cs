using HalceraAPI.Common.Utilities;
using HalceraAPI.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HalceraAPI.Areas.Admin.Controllers
{
    [Authorize(Roles = $"{RoleDefinition.Admin},{RoleDefinition.Employee}")]
    [Area("Admin")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class CompositionsController : ControllerBase
    {
        private readonly ICompositionOperation _compositionOperation;

        public CompositionsController(ICompositionOperation compositionOperation)
        {
            _compositionOperation = compositionOperation;
        }

        [HttpDelete("{compositionId}/media/{mediaId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> DeleteMediaFromCompositionByMediaIdAsync(int compositionId, int mediaId)
        {
            await _compositionOperation.DeleteMediaFromCompositionByMediaIdAsync(compositionId, mediaId);

            return NoContent();
        }

        [HttpDelete("{compositionId}/price/{priceId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> DeletePriceFromCompositionByPriceIdAsync(int compositionId, int priceId)
        {
            await _compositionOperation.DeletePriceFromCompositionByPriceIdAsync(compositionId, priceId);

            return NoContent();
        }

        [HttpDelete("{compositionId}/size/{sizeId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> DeleteSizeFromCompositionBySizeIdAsync(int compositionId, int sizeId)
        {
            await _compositionOperation.DeleteSizeFromCompositionBySizeIdAsync(compositionId, sizeId);

            return NoContent();
        }

        [HttpPatch("{compositionId}/price/{priceId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> ResetDiscountOfCompositionPriceByPriceIdAsync(int compositionId, int priceId)
        {
            await _compositionOperation.ResetDiscountOfCompositionPriceByPriceIdAsync(compositionId, priceId);

            return NoContent();
        }
    }
}
