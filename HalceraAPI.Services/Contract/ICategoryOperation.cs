using HalceraAPI.Models;
using HalceraAPI.Models.Requests.APIResponse;
using HalceraAPI.Models.Requests.Category;

namespace HalceraAPI.Services.Contract
{
    /// <summary>
    /// Category Operations
    /// </summary>
    public interface ICategoryOperation
    {
        Task<APIResponse<IEnumerable<CategoryResponse>>> GetAllCategoriesAsync(bool? active, bool? featured, int? page);
        
        Task<CategoryResponse?> GetCategoryAsync(int categoryId);

        Task<APIResponse<CategoryResponse>> CreateCategoryAsync(CreateCategoryRequest category);

        Task<CategoryResponse> UpdateCategoryAsync(int categoryId, UpdateCategoryRequest category);

        Task<bool> DeleteCategoryAsync(int categoryId);

        Task<IEnumerable<Category>?> GetCategoriesFromListOfCategoryIdAsync(IEnumerable<ProductCategoryRequest>? categoryRequests);
    }
}
