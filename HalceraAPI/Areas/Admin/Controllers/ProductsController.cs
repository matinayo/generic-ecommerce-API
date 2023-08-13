using HalceraAPI.Common.Utilities;
using HalceraAPI.Models.Requests.Product;
using HalceraAPI.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HalceraAPI.Areas.Admin.Controllers
{
    /// <summary>
    /// Admin controllers
    /// </summary>
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

        // [HttpGet("category/{categoryId}")]
        [HttpGet]
        [Route("GetAll")]
        [ProducesResponseType(typeof(IEnumerable<ProductResponse>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetAll(bool? active, bool? featured, int? categoryId)
        {
            try
            {
                IEnumerable<ProductResponse>? listOfProducts = await _productOperation.GetAllProducts(active: active, featured: featured, categoryId: categoryId);
                return Ok(listOfProducts);
            }
            catch (Exception exception)
            {
                return BadRequest(Problem(statusCode: StatusCodes.Status400BadRequest, detail: exception?.InnerException?.Message ?? exception?.Message));
            }
        }

        [HttpGet]
        [Route("GetProduct/{productId}")]
        [ProducesResponseType(typeof(ProductDetailsResponse), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<ProductDetailsResponse>?>> GetProductById(int productId)
        {
            try
            {
                ProductDetailsResponse? productDetails = await _productOperation.GetProductById(productId);
                return Ok(productDetails);
            }
            catch (Exception exception)
            {
                return BadRequest(Problem(statusCode: StatusCodes.Status400BadRequest, detail: exception?.InnerException?.Message ?? exception?.Message));
            }
        }

        [Authorize(Roles = $"{RoleDefinition.Admin},{RoleDefinition.Employee}")]
        [HttpPost]
        [Route("CreateProduct")]
        [ProducesResponseType(typeof(ProductDetailsResponse), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ProductDetailsResponse?>> CreateProduct([FromBody] CreateProductRequest productRequest)
        {
            try
            {
                ProductDetailsResponse productDetails = await _productOperation.CreateProduct(productRequest);
                return Ok(productDetails);
            }
            catch (Exception exception)
            {
                return BadRequest(Problem(statusCode: StatusCodes.Status400BadRequest, detail: exception?.InnerException?.Message ?? exception?.Message));
            }
        }

        [Authorize(Roles = $"{RoleDefinition.Admin},{RoleDefinition.Employee}")]
        [HttpPut]
        [Route("UpdateProduct/{productId}")]
        [ProducesResponseType(typeof(ProductDetailsResponse), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ProductDetailsResponse?>> UpdateProduct(int productId, [FromBody] UpdateProductRequest productRequest)
        {
            try
            {
                ProductDetailsResponse productDetails = await _productOperation.UpdateProduct(productId, productRequest);
                return Ok(productDetails);
            }
            catch (Exception exception)
            {
                return BadRequest(Problem(statusCode: StatusCodes.Status400BadRequest, detail: exception?.InnerException?.Message ?? exception?.Message));
            }
        }

        [Authorize(Roles = $"{RoleDefinition.Admin},{RoleDefinition.Employee}")]
        [HttpDelete]
        [Route("DeleteProduct/{productId}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<bool?>> DeleteProduct(int productId)
        {
            try
            {
                bool result = await _productOperation.DeleteProduct(productId);
                return Ok(result);
            }
            catch (Exception exception)
            {
                return BadRequest(Problem(statusCode: StatusCodes.Status400BadRequest, detail: exception?.InnerException?.Message ?? exception?.Message));
            }
        }
    }
}
