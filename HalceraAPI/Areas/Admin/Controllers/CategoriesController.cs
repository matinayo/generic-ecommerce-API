using HalceraAPI.Common.Utilities;
using HalceraAPI.Models.Requests.APIResponse;
using HalceraAPI.Models.Requests.Category;
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
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryOperation _categoryOperation;
        public CategoriesController(ICategoryOperation categoryOperation)
        {
            _categoryOperation = categoryOperation;
        }

        [HttpGet]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<CategoryResponse>>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> GetCategoriesAsync(bool? active, bool? featured, int? page)
        {
            try
            {
                APIResponse<IEnumerable<CategoryResponse>> listOfCategories = 
                    await _categoryOperation.GetAllCategoriesAsync(active: active, featured: featured, page: page);

                return Ok(listOfCategories);
            }
            catch (Exception exception)
            {
                return BadRequest(Problem(statusCode: StatusCodes.Status400BadRequest, detail: exception.Message));
            }
        }

        [HttpGet("{categoryId}")]
        [ProducesResponseType(typeof(APIResponse<CategoryResponse>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> GetCategoryByIdAsync(int categoryId)
        {
            try
            {
                APIResponse<CategoryResponse>? categoryDetails = await _categoryOperation.GetCategoryAsync(categoryId);

                return Ok(categoryDetails);
            }
            catch (Exception exception)
            {
                return BadRequest(Problem(statusCode: StatusCodes.Status400BadRequest, detail: exception.Message));
            }
        }

        [Authorize(Roles = $"{RoleDefinition.Admin},{RoleDefinition.Employee}")]
        [HttpPost]
        [ProducesResponseType(typeof(APIResponse<CategoryResponse>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> CreateCategory([FromBody] CreateCategoryRequest category)
        {
            try
            {
                var categoryDetails = await _categoryOperation.CreateCategoryAsync(category);
                return Ok(categoryDetails);
            }
            catch (Exception exception)
            {
                return BadRequest(Problem(statusCode: StatusCodes.Status400BadRequest, detail: exception.Message));
            }
        }

        [Authorize(Roles = $"{RoleDefinition.Admin},{RoleDefinition.Employee}")]
        [HttpPut("{categoryId}")]
        [ProducesResponseType(typeof(APIResponse<CategoryResponse>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> UpdateCategory(int categoryId, [FromBody] UpdateCategoryRequest category)
        {
            try
            {
                APIResponse<CategoryResponse> categoryDetails = await _categoryOperation.UpdateCategoryAsync(categoryId, category);

                return Ok(categoryDetails);
            }
            catch (Exception exception)
            {
                return BadRequest(Problem(statusCode: StatusCodes.Status400BadRequest, detail: exception.Message));
            }
        }

        [Authorize(Roles = $"{RoleDefinition.Admin},{RoleDefinition.Employee}")]
        [HttpDelete("{categoryId}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<bool?>> DeleteCategory(int categoryId)
        {
            try
            {
                bool result = await _categoryOperation.DeleteCategoryAsync(categoryId);
                return Ok(result);
            }
            catch (Exception exception)
            {
                return BadRequest(Problem(statusCode: StatusCodes.Status400BadRequest, detail: exception.Message));
            }
        }
    }
}
