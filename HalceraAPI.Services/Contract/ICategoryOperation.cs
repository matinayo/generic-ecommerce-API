using HalceraAPI.Models;
using HalceraAPI.Services.Dtos.APIResponse;
using HalceraAPI.Services.Dtos.Category;

namespace HalceraAPI.Services.Contract
{
    public interface ICategoryOperation
    {
        Task<APIResponse<IEnumerable<CategoryResponse>>> GetAllCategoriesAsync(bool? active, bool? featured, int? page);

        Task<APIResponse<CategoryResponse>> GetCategoryAsync(int categoryId);

        Task<APIResponse<CategoryResponse>> CreateCategoryAsync(CreateCategoryRequest category);

        Task<APIResponse<CategoryResponse>> UpdateCategoryAsync(int categoryId, UpdateCategoryRequest category);

        Task DeleteCategoryAsync(int categoryId);

        Task<IEnumerable<Category>?> GetCategoriesFromListOfCategoryIdAsync(IEnumerable<ProductCategoryRequest>? categoryRequests);

        Task DeleteCategoryFromProductByCategoryIdAsync(int productId, int categoryId);

        Task DeleteMediaFromCategoryByMediaIdAsync(int categoryId, int mediaId);
    }
}
