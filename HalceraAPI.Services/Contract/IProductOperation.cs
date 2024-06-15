using HalceraAPI.Services.Dtos.APIResponse;
using HalceraAPI.Services.Dtos.Product;

namespace HalceraAPI.Services.Contract
{
    public interface IProductOperation
    {
        Task<APIResponse<IEnumerable<ProductResponse>>> GetAllProductsAsync(bool? active, bool? featured, int? categoryId, int? page);

        Task<APIResponse<ProductDetailsResponse>> GetProductByIdAsync(int productId);

        Task<APIResponse<ProductDetailsResponse>> CreateProductAsync(CreateProductRequest productRequest);

        Task<APIResponse<ProductDetailsResponse>> UpdateProductAsync(int productId, UpdateProductRequest productRequest);

        Task DeleteProductAsync(int productId);

        Task DeleteCategoryFromProductByCategoryIdAsync(int productId, int categoryId);

        Task DeleteCompositionFromProductByCompositionIdAsync(int productId, int compositionId);

        Task DeleteComponentDataFromProductByComponentDataIdAsync(int productId, int componentId);
    }
}