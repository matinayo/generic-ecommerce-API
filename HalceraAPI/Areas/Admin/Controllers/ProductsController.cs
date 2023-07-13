using HalceraAPI.Models;
using HalceraAPI.Models.Requests.Category;
using HalceraAPI.Models.Requests.Product;
using HalceraAPI.Services.Contract;
using HalceraAPI.Services.Operations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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

        [HttpGet]
        [Route("GetAll")]
        [ProducesResponseType(typeof(IEnumerable<Product>), 200)]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<Product>> GetAll()
        {
            if (ModelState.IsValid)
            {
                return Problem();
            }
            //if (true)
            //{
            //    return NotFound();
            //}
            return BadRequest(ModelState);
        }

        [HttpPost]
        [Route("CreateProduct")]
        [ProducesResponseType(typeof(ProductResponse), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ProductResponse?>> CreateProduct([FromBody] CreateProductRequest product)
        {
            try
            {
                ProductResponse productDetails = await _productOperation.CreateProduct(product);
                return Ok(productDetails);
            }
            catch (Exception exception)
            {
                return BadRequest(Problem(statusCode: StatusCodes.Status400BadRequest, detail: exception.Message));
            }
        }
    }
}
