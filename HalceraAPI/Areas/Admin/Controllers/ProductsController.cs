using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HalceraAPI.Areas.Admin.Controllers
{
    [Area("Admin")]
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
