using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HalceraAPI.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            return Ok();
        }
    }
}
