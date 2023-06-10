using HalceraAPI.Models;
using HalceraAPI.Services.Contract;
using Microsoft.AspNetCore.Mvc;

namespace HalceraAPI.Areas.Admin.Controllers
{
    /// <summary>
    /// Admin controllers
    /// </summary>
    [Area("Admin")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryOperation _categoryOperation;
        public CategoryController(ICategoryOperation categoryOperation)
        {
            _categoryOperation = categoryOperation;
        }

        [HttpGet]
        [Route("GetAll")]
        [ProducesResponseType(typeof(IEnumerable<Category>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<Category>?>> GetAll()
        {
            try
            {
                IEnumerable<Category>? listOfCategories = await _categoryOperation.GetAllCategories();
                return Ok(listOfCategories);
            }
            catch (Exception exception)
            {
                return BadRequest(Problem(statusCode: StatusCodes.Status400BadRequest, detail: exception.Message));
            }
        }

        [HttpGet]
        [Route("GetCategory/{categoryId}")]
        [ProducesResponseType(typeof(Category), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<Category>?>> GetCategoryById(int categoryId)
        {
            try
            {
                Category? categoryDetails = await _categoryOperation.GetCategory(categoryId);
                return Ok(categoryDetails);
            }
            catch (Exception exception)
            {
                return BadRequest(Problem(statusCode: StatusCodes.Status400BadRequest, detail: exception.Message));
            }
        }

        [HttpPost]
        [Route("CreateCategory")]
        [ProducesResponseType(typeof(Category), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Product?>> CreateCategory([FromBody] Category category)
        {
            try
            {
                Category categoryDetails = await _categoryOperation.CreateCategory(category);
                return Ok(categoryDetails);
            }
            catch (Exception exception)
            {
                return BadRequest(Problem(statusCode: StatusCodes.Status400BadRequest, detail: exception.Message));
            }
        }

        [HttpPut]
        [Route("UpdateCategory")]
        [ProducesResponseType(typeof(Category), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Category?>> UpdateCategory([FromBody] Category category)
        {
            try
            {
                Category categoryDetails = await _categoryOperation.UpdateCategory(category);
                return Ok(categoryDetails);
            }
            catch (Exception exception)
            {
                return BadRequest(Problem(statusCode: StatusCodes.Status400BadRequest, detail: exception.Message));
            }
        }

        [HttpDelete]
        [Route("DeleteCategory")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Category?>> DeleteCategory([FromQuery] int categoryId)
        {
            try
            {
                bool result = await _categoryOperation.DeleteCategory(categoryId);
                return Ok(result);
            }
            catch (Exception exception)
            {
                return BadRequest(Problem(statusCode: StatusCodes.Status400BadRequest, detail: exception.Message));
            }
        }
    }
}
