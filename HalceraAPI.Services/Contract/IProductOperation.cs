﻿using HalceraAPI.Model;

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
        Task<IEnumerable<Product>?> GetAllProducts();
        /// <summary>
        /// Get Product By Id
        /// </summary>
        /// <param name="productId">Id of Product to Get</param>
        /// <returns>Product details from DB</returns>
        Task<Product?> GetProductById(int productId);
        /// <summary>
        /// Add product item to cart
        /// </summary>
        /// <param name="productId">id of product</param>
        /// <returns>id of added item in cart</returns>
        Task<int> AddProductToCart(int productId);
    }
}