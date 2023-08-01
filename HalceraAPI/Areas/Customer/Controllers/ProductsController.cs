﻿using HalceraAPI.Models.Requests.ShoppingCart;
using HalceraAPI.Services.Contract;
using Microsoft.AspNetCore.Mvc;

namespace HalceraAPI.Areas.Customer.Controllers
{
    /// <summary>
    /// Product controller defining customers endpoint
    /// </summary>
    [Area("Customer")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductOperation _productOperation;
        public ProductsController(IProductOperation productOperation)
        {
            _productOperation = productOperation;
        }

        ///// <summary>
        ///// Get list of products
        ///// </summary>
        ///// <returns>List of products</returns>
        //[HttpGet]
        //[Route("GetAll")]
        //[ProducesResponseType(typeof(IEnumerable<Product>), 200)]
        //[ProducesResponseType(400)]
        //public async Task<ActionResult<IEnumerable<Product>?>> GetAll([FromQuery] bool Active = false)
        //{
        //    try
        //    {
        //        IEnumerable<Product>? listOfProducts = await _productOperation.GetAllProducts(Active);
        //        return Ok(listOfProducts);
        //    }
        //    catch (Exception exception)
        //    {
        //        return BadRequest(Problem(statusCode: StatusCodes.Status400BadRequest, detail: exception.Message));
        //    }
        //}

        ///// <summary>
        ///// Get product by Id
        ///// </summary>
        ///// <param name="productId">id of product</param>
        ///// <returns>Product details from DB</returns>
        //[HttpGet]
        //[Route("GetProduct/{productId}")]
        //[ProducesResponseType(typeof(Product), 200)]
        //[ProducesResponseType(204)]
        //[ProducesResponseType(400)]
        //public async Task<ActionResult<ProductResponse?>> GetProduct(int productId)
        //{
        //    try
        //    {
        //        Product? productFromDb = await _productOperation.GetProductById(productId);
        //        return Ok(productFromDb);
        //    }
        //    catch (Exception exception)
        //    {
        //        return BadRequest(Problem(statusCode: StatusCodes.Status400BadRequest, detail: exception.Message));
        //    }
        //}

        /// <summary>
        /// Add product to shopping cart
        /// </summary>
        /// <param name="productId">Id of product</param>
        /// <returns>id of item in cart</returns>
        [HttpPost]
        [Route("AddProductToCart/productId")]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<int?>> AddProductToCart(int productId, [FromBody] AddProductToCartRequest addProductToCart)
        {
            try
            {
                int idOfShoppingCart = await _productOperation.AddProductToCart(productId, addProductToCart);
                return Ok(idOfShoppingCart);
            }
            catch (Exception exception)
            {
                return BadRequest(Problem(statusCode: StatusCodes.Status400BadRequest, detail: exception.Message));
            }
        }
        // get products by category
    }
}
