using HalceraAPI.Model;
using HalceraAPI.Services.Contract;
using Microsoft.AspNetCore.Mvc;

namespace HalceraAPI.Areas.Customer.Controllers
{
    [Area("Customer")]
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
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<Product>?>> GetAll()
        {
            try
            {
                IEnumerable<Product>? listOfProducts = await _productOperation.GetAllProducts();
                return Ok(listOfProducts);
            }
            catch (Exception exception)
            {
                return BadRequest(Problem(statusCode: StatusCodes.Status400BadRequest, detail: exception.Message));
            }
        }
    }
}
