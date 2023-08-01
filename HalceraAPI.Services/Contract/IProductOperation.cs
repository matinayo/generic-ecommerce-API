using HalceraAPI.Models.Requests.Product;

namespace HalceraAPI.Services.Contract
{
    /// <summary>
    /// Product Operations
    /// </summary>
    public interface IProductOperation
    {
        /// <summary>
        /// Get list of products
        /// </summary>
        /// <returns>List of products from DB</returns>
        Task<IEnumerable<ProductResponse>?> GetAllProducts(bool? active, bool? featured, int? categoryId);
        /// <summary>
        /// Get Product By Id
        /// </summary>
        /// <param name="productId">Id of Product to Get</param>
        /// <returns>Product details from DB</returns>
        Task<ProductDetailsResponse?> GetProductById(int productId);
        /// <summary>
        /// Create new product
        /// </summary>
        /// <param name="product">product data to be created</param>
        /// <returns>Created product details</returns>
        Task<ProductDetailsResponse> CreateProduct(CreateProductRequest productRequest);
        /// <summary>
        /// Update a product
        /// </summary>
        /// <param name="product">Product data to be updated</param>
        /// <returns>Updated product details from DB</returns>
        Task<ProductDetailsResponse> UpdateProduct(int productId, UpdateProductRequest productRequest);
        /// <summary>
        /// Deletes a product
        /// </summary>
        /// <param name="productId">Id of product to be deleted</param>
        /// <returns>boolean to indicate if operation was successful</returns>
        Task<bool> DeleteProduct(int productId);
    }
}