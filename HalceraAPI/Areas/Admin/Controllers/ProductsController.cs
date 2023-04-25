using HalceraAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HalceraAPI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
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
    }
}
