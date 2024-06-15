using HalceraAPI.Common.Utilities;
using HalceraAPI.Services.Dtos.APIResponse;
using HalceraAPI.Services.Dtos.Product;
using HalceraAPI.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HalceraAPI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductOperation _productOperation;
        public ProductsController(IProductOperation productOperation)
        {
            _productOperation = productOperation;
        }

        [HttpGet]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<ProductResponse>>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> GetProductsAsync(bool? active, bool? featured, int? categoryId, int? page)
        {
            APIResponse<IEnumerable<ProductResponse>> listOfProducts =
                await _productOperation.GetAllProductsAsync(active, featured, categoryId, page);

            return Ok(listOfProducts);
        }

        [HttpGet("{productId}")]
        [ProducesResponseType(typeof(APIResponse<ProductDetailsResponse>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> GetProductByIdAsync(int productId)
        {
            APIResponse<ProductDetailsResponse>? productDetails =
                await _productOperation.GetProductByIdAsync(productId);

            return Ok(productDetails);
        }

        [Authorize(Roles = $"{RoleDefinition.Admin},{RoleDefinition.Employee}")]
        [HttpPost]
        [ProducesResponseType(typeof(APIResponse<ProductDetailsResponse>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> CreateProductAsync([FromBody] CreateProductRequest productRequest)
        {
            APIResponse<ProductDetailsResponse> productDetails =
                await _productOperation.CreateProductAsync(productRequest);

            return Ok(productDetails);
        }

        [Authorize(Roles = $"{RoleDefinition.Admin},{RoleDefinition.Employee}")]
        [HttpPut("{productId}")]
        [ProducesResponseType(typeof(APIResponse<ProductDetailsResponse>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> UpdateProductAsync(int productId, [FromBody] UpdateProductRequest productRequest)
        {
            APIResponse<ProductDetailsResponse> productDetails = await _productOperation.UpdateProductAsync(productId, productRequest);

            return Ok(productDetails);
        }

        [Authorize(Roles = $"{RoleDefinition.Admin},{RoleDefinition.Employee}")]
        [HttpDelete("{productId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> DeleteProductAsync(int productId)
        {
            await _productOperation.DeleteProductAsync(productId);

            return NoContent();
        }

        [Authorize(Roles = $"{RoleDefinition.Admin},{RoleDefinition.Employee}")]
        [HttpDelete("{productId}/Category/{categoryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> DeleteCategoryFromProductByCategoryIdAsync(int productId, int categoryId)
        {
            await _productOperation.DeleteCategoryFromProductByCategoryIdAsync(productId, categoryId);

            return NoContent();
        }

        [Authorize(Roles = $"{RoleDefinition.Admin},{RoleDefinition.Employee}")]
        [HttpDelete("{productId}/Composition/{compositionId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> DeleteCompositionFromProductByCompositionIdAsync(int productId, int compositionId)
        {
            await _productOperation.DeleteCompositionFromProductByCompositionIdAsync(productId, compositionId);

            return NoContent();
        }

        [Authorize(Roles = $"{RoleDefinition.Admin},{RoleDefinition.Employee}")]
        [HttpDelete("{productId}/ComponentData/{componentDataId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> DeleteCompomemtDataByCompositionDataIdAsync(
            int productId,
            int componentDataId)
        {
            await _productOperation.DeleteComponentDataFromProductByComponentDataIdAsync(
                productId,
                componentDataId);

            return NoContent();
        }
    }
}
