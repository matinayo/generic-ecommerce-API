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

        Task DeleteCategoryFromProductByCategoryIdAsync(int productId, int categoryId);

        Task DeletePriceFromProductByPriceIdAsync(int productId, int priceId);

        Task DeleteMediaFromProductByMediaIdAsync(int productId, int mediaId);

        Task DeleteCompositionFromProductByCompositionIdAsync(int productId, int compositionId);

        Task DeleteProductCompositionDataByCompositionDataIdAsync(int productId, int compositionId, int compositionDataId);
    }
}