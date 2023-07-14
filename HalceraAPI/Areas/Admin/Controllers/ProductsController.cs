using AutoMapper.Features;
using HalceraAPI.Models;
using HalceraAPI.Models.Requests.Category;
using HalceraAPI.Models.Requests.Product;
using HalceraAPI.Services.Contract;
using HalceraAPI.Services.Operations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace HalceraAPI.Areas.Admin.Controllers
{
    /// <summary>
    /// Admin controllers
    /// </summary>
    [Area("Admin")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductOperation _productOperation;
        public ProductsController(IProductOperation productOperation)
        {
            _productOperation = productOperation;
        }

        [HttpGet]
        [Route("GetAll")]
        [ProducesResponseType(typeof(IEnumerable<ProductResponse>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetAll(bool? active, bool? featured, int? categoryId)
        {
            try
            {
                IEnumerable<ProductResponse>? listOfProducts = await _productOperation.GetAllProducts(active: active, featured: featured, categoryId: categoryId);
                return Ok(listOfProducts);
            }
            catch (Exception exception)
            {
                return BadRequest(Problem(statusCode: StatusCodes.Status400BadRequest, detail: exception.Message));
            }
        }

        [HttpGet]
        [Route("GetProduct/{productId}")]
        [ProducesResponseType(typeof(ProductDetailsResponse), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<ProductDetailsResponse>?>> GetProductById(int productId)
        {
            try
            {
                ProductDetailsResponse? productDetails = await _productOperation.GetProductById(productId);
                return Ok(productDetails);
            }
            catch (Exception exception)
            {
                return BadRequest(Problem(statusCode: StatusCodes.Status400BadRequest, detail: exception.Message));
            }
        }

        [HttpPost]
        [Route("CreateProduct")]
        [ProducesResponseType(typeof(ProductDetailsResponse), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ProductDetailsResponse?>> CreateProduct([FromBody] CreateProductRequest product)
        {
            try
            {
                ProductDetailsResponse productDetails = await _productOperation.CreateProduct(product);
                return Ok(productDetails);
            }
            catch (Exception exception)
            {
                return BadRequest(Problem(statusCode: StatusCodes.Status400BadRequest, detail: exception.Message));
            }
        }
    }
}
