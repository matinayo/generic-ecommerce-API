﻿using HalceraAPI.Common.Utilities;
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
        [ProducesResponseType(typeof(IEnumerable<CategoryResponse>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<CategoryResponse>?>> GetCategories(bool? active, bool? featured = null)
        {
            try
            {
                IEnumerable<CategoryResponse>? listOfCategories = 
                    await _categoryOperation.GetAllCategories(active: active, featured: featured);

                return Ok(listOfCategories);
            }
            catch (Exception exception)
            {
                return BadRequest(Problem(statusCode: StatusCodes.Status400BadRequest, detail: exception.Message));
            }
        }

        [HttpGet("{categoryId}")]
        [ProducesResponseType(typeof(CategoryResponse), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<CategoryResponse>?>> GetCategoryById(int categoryId)
        {
            try
            {
                CategoryResponse? categoryDetails = await _categoryOperation.GetCategory(categoryId);

                return Ok(categoryDetails);
            }
            catch (Exception exception)
            {
                return BadRequest(Problem(statusCode: StatusCodes.Status400BadRequest, detail: exception.Message));
            }
        }

        [Authorize(Roles = $"{RoleDefinition.Admin},{RoleDefinition.Employee}")]
        [HttpPost]
        [ProducesResponseType(typeof(CategoryResponse), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<CategoryResponse?>> CreateCategory([FromBody] CreateCategoryRequest category)
        {
            try
            {
                CategoryResponse categoryDetails = await _categoryOperation.CreateCategory(category);
                return Ok(categoryDetails);
            }
            catch (Exception exception)
            {
                return BadRequest(Problem(statusCode: StatusCodes.Status400BadRequest, detail: exception.Message));
            }
        }

        [Authorize(Roles = $"{RoleDefinition.Admin},{RoleDefinition.Employee}")]
        [HttpPut("{categoryId}")]
        [ProducesResponseType(typeof(CategoryResponse), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<CategoryResponse?>> UpdateCategory(int categoryId, [FromBody] UpdateCategoryRequest category)
        {
            try
            {
                CategoryResponse categoryDetails = await _categoryOperation.UpdateCategory(categoryId, category);
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