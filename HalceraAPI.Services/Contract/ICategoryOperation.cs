using HalceraAPI.Models;
using HalceraAPI.Models.Requests.Category;

namespace HalceraAPI.Services.Contract
{
    /// <summary>
    /// Category Operations
    /// </summary>
    public interface ICategoryOperation
    {
        /// <summary>
        /// Get all categories
        /// </summary>
        /// <returns>List of categories</returns>
        Task<IEnumerable<CategoryResponse>?> GetAllCategories(bool? active, bool? featured);
        /// <summary>
        /// Get category by Id
        /// </summary>
        /// <param name="categoryId">id of category to retrieve</param>
        /// <returns>Category details from DB</returns>
        Task<CategoryResponse?> GetCategory(int categoryId);
        /// <summary>
        /// Create a new category
        /// </summary>
        /// <param name="category">Category details to be created</param>
        /// <returns>Created category</returns>
        Task<CategoryResponse> CreateCategory(CreateCategoryRequest category);
        /// <summary>
        /// Update details of a category
        /// </summary>
        /// <param name="category">Category data to be updated</param>
        /// <returns>updated category details</returns>
        Task<CategoryResponse> UpdateCategory(int categoryId, UpdateCategoryRequest category);
        /// <summary>
        /// Delete category record
        /// </summary>
        /// <param name="categoryId">id of category to be deleted</param>
        /// <returns>boolean indicating if delete operation is success</returns>
        Task<bool> DeleteCategory(int categoryId);

        /// <summary>
        /// Gets categories from List of category Id
        /// </summary>
        /// <param name="categoryRequests">List of category Id</param>
        /// <returns>List of category details</returns>
        Task<IEnumerable<Category>?> GetCategoriesFromListOfCategoryId(IEnumerable<ProductCategoryRequest>? categoryRequests);
    }
}
