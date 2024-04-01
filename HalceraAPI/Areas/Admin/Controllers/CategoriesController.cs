using HalceraAPI.Common.Utilities;
using HalceraAPI.Services.Dtos.APIResponse;
using HalceraAPI.Services.Dtos.Category;
using HalceraAPI.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HalceraAPI.Areas.Admin.Controllers
{
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
            APIResponse<IEnumerable<CategoryResponse>> listOfCategories =
                    await _categoryOperation.GetAllCategoriesAsync(active: active, featured: featured, page: page);

                return Ok(listOfCategories);
        }

        [HttpGet("{categoryId}")]
        [ProducesResponseType(typeof(APIResponse<CategoryResponse>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> GetCategoryByIdAsync(int categoryId)
        {
                APIResponse<CategoryResponse>? categoryDetails = await _categoryOperation.GetCategoryAsync(categoryId);

            return Ok(categoryDetails);
        }

        [Authorize(Roles = $"{RoleDefinition.Admin},{RoleDefinition.Employee}")]
        [HttpPost]
        [ProducesResponseType(typeof(APIResponse<CategoryResponse>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> CreateCategory([FromBody] CreateCategoryRequest category)
        {
                APIResponse<CategoryResponse> categoryDetails = await _categoryOperation.CreateCategoryAsync(category);

                return Ok(categoryDetails);
        }

        [Authorize(Roles = $"{RoleDefinition.Admin},{RoleDefinition.Employee}")]
        [HttpPut("{categoryId}")]
        [ProducesResponseType(typeof(APIResponse<CategoryResponse>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> UpdateCategory(int categoryId, [FromBody] UpdateCategoryRequest category)
        {
                APIResponse<CategoryResponse> categoryDetails = await _categoryOperation.UpdateCategoryAsync(categoryId, category);

                return Ok(categoryDetails);
        }

        [Authorize(Roles = $"{RoleDefinition.Admin},{RoleDefinition.Employee}")]
        [HttpDelete("{categoryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> DeleteCategory(int categoryId)
        {
                await _categoryOperation.DeleteCategoryAsync(categoryId);

                return NoContent();
        }

        [Authorize(Roles = $"{RoleDefinition.Admin},{RoleDefinition.Employee}")]
        [HttpDelete("{categoryId}/Media/{mediaId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> DeleteCategoryMediaByMediaIdAsync(int categoryId, int mediaId)
        {
                await _categoryOperation.DeleteMediaFromCategoryByMediaIdAsync(categoryId, mediaId);

                return NoContent();
        }
    }
}
