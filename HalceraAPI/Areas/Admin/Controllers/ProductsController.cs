using HalceraAPI.Common.Utilities;
using HalceraAPI.Models.Requests.APIResponse;
using HalceraAPI.Models.Requests.Product;
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
            try
            {
                APIResponse<IEnumerable<ProductResponse>> listOfProducts = 
                    await _productOperation.GetAllProductsAsync(active, featured, categoryId, page);
                
                return Ok(listOfProducts);
            }
            catch (Exception exception)
            {
                return BadRequest(
                        Problem(
                            statusCode: StatusCodes.Status400BadRequest, 
                            detail: exception?.InnerException?.Message ?? exception?.Message));
            }
        }

        [HttpGet("{productId}")]
        [ProducesResponseType(typeof(APIResponse<ProductDetailsResponse>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> GetProductByIdAsync(int productId)
        {
            try
            {
                APIResponse<ProductDetailsResponse>? productDetails = 
                    await _productOperation.GetProductByIdAsync(productId);

                return Ok(productDetails);
            }
            catch (Exception exception)
            {
                return BadRequest(
                        Problem(
                            statusCode: StatusCodes.Status400BadRequest, 
                            detail: exception?.InnerException?.Message ?? exception?.Message));
            }
        }

        [Authorize(Roles = $"{RoleDefinition.Admin},{RoleDefinition.Employee}")]
        [HttpPost]
        [ProducesResponseType(typeof(APIResponse<ProductDetailsResponse>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> CreateProductAsync([FromBody] CreateProductRequest productRequest)
        {
            try
            {
                APIResponse<ProductDetailsResponse> productDetails = 
                    await _productOperation.CreateProductAsync(productRequest);

                return Ok(productDetails);
            }
            catch (Exception exception)
            {
                return BadRequest(
                        Problem(
                            statusCode: StatusCodes.Status400BadRequest,
                            detail: exception?.InnerException?.Message ?? exception?.Message));
            }
        }

        [Authorize(Roles = $"{RoleDefinition.Admin},{RoleDefinition.Employee}")]
        [HttpPut("{productId}")]
        [ProducesResponseType(typeof(APIResponse<ProductDetailsResponse>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> UpdateProductAsync(int productId, [FromBody] UpdateProductRequest productRequest)
        {
            try
            {
                APIResponse<ProductDetailsResponse> productDetails = await _productOperation.UpdateProductAsync(productId, productRequest);

                return Ok(productDetails);
            }
            catch (Exception exception)
            {
                return BadRequest(
                        Problem(
                            statusCode: StatusCodes.Status400BadRequest, 
                            detail: exception?.InnerException?.Message ?? exception?.Message));
            }
        }

        [Authorize(Roles = $"{RoleDefinition.Admin},{RoleDefinition.Employee}")]
        [HttpDelete("{productId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> DeleteProductAsync(int productId)
        {
            try
            {
                await _productOperation.DeleteProductAsync(productId);

                return NoContent();
            }
            catch (Exception exception)
            {
                return BadRequest(
                        Problem(
                            statusCode: StatusCodes.Status400BadRequest, 
                            detail: exception?.InnerException?.Message ?? exception?.Message));
            }
        }

        [Authorize(Roles = $"{RoleDefinition.Admin},{RoleDefinition.Employee}")]
        [HttpDelete("{productId}/Category/{categoryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> DeleteProductCategoryAsync(int productId, int categoryId)
        {
            try
            {
                await _productOperation.DeleteProductCategoryByCategoryIdAsync(productId, categoryId);

                return NoContent();
            }
            catch (Exception exception)
            {
                return BadRequest(
                        Problem(
                            statusCode: StatusCodes.Status400BadRequest,
                            detail: exception?.InnerException?.Message ?? exception?.Message));
            }
        }

        [Authorize(Roles = $"{RoleDefinition.Admin},{RoleDefinition.Employee}")]
        [HttpDelete("{productId}/Price/{priceId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> DeleteProductPriceByPriceIdAsync(int productId, int priceId)
        {
            try
            {
                await _productOperation.DeleteProductPriceByPriceIdAsync(productId, priceId);

                return NoContent();
            }
            catch (Exception exception)
            {
                return BadRequest(
                        Problem(
                            statusCode: StatusCodes.Status400BadRequest,
                            detail: exception?.InnerException?.Message ?? exception?.Message));
            }
        }

        [Authorize(Roles = $"{RoleDefinition.Admin},{RoleDefinition.Employee}")]
        [HttpDelete("{productId}/Media/{mediaId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> DeleteProductMediaByMediaIdAsync(int productId, int mediaId)
        {
            try
            {
                await _productOperation.DeleteProductMediaByMediaIdAsync(productId, mediaId);

                return NoContent();
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
