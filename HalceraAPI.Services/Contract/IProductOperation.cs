using HalceraAPI.Models.Requests.APIResponse;
using HalceraAPI.Models.Requests.Product;

namespace HalceraAPI.Services.Contract
{
    public interface IProductOperation
    {
        Task<APIResponse<IEnumerable<ProductResponse>>> GetAllProductsAsync(bool? active, bool? featured, int? categoryId, int? page);

        Task<APIResponse<ProductDetailsResponse>> GetProductByIdAsync(int productId);

        Task<APIResponse<ProductDetailsResponse>> CreateProductAsync(CreateProductRequest productRequest);

        Task<APIResponse<ProductDetailsResponse>> UpdateProductAsync(int productId, UpdateProductRequest productRequest);

        Task DeleteProductAsync(int productId);
        Task DeleteProductCategoryByCategoryIdAsync(int productId, int categoryId);
    }
}