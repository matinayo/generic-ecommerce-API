using AutoMapper;
using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;
using HalceraAPI.Models.Requests.Product;
using HalceraAPI.Services.Contract;

namespace HalceraAPI.Services.Operations
{
    public class ProductOperation : IProductOperation
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductOperation(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Add product item to cart
        /// </summary>
        /// <param name="productId">id of product</param>
        /// <returns>id of product in cart</returns>
        public async Task<int> AddProductToCart(int productId)
        {
            try
            {
                ShoppingCart? cart = await _unitOfWork.ShoppingCart.GetFirstOrDefault(cart => cart.ProductId == productId);
                if (cart == null)
                {   // adds new item in cart
                    Product? productItem = await _unitOfWork.Product.GetFirstOrDefault(product => product.Id == productId);
                    if (productItem == null)
                    {
                        throw new Exception("Product not found");
                    }
                    // TODO: add ApplicationUser from Token
                    //  cart = new ShoppingCart() { ProductId = productId, Price = productItem.Price };
                    // if product item does not exist in cart, add new item
                    await _unitOfWork.ShoppingCart.Add(cart);
                }
                else
                {   // update existing item count in cart
                    cart.Count++;
                }
                await _unitOfWork.SaveAsync();
                return cart.Id;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public async Task<ProductResponse> CreateProduct(CreateProductRequest productRequest)
        {
            try
            {
                Product product = new();
                _mapper.Map(productRequest, product);

                if (productRequest.Categories != null && productRequest.Categories.Any())
                {
                    var categories = await _unitOfWork.Category.GetAll(category => productRequest.Categories != null && productRequest.Categories.Select(opt => opt.CategoryId).Contains(category.Id), includeProperties: nameof(Category.MediaCollection));
                    product.Categories = categories.ToList();
                }

                await _unitOfWork.Product.Add(product);
                await _unitOfWork.SaveAsync();

                ProductResponse productResponse = new();
                _mapper.Map(product, productResponse);
                return productResponse;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            try
            {
                Product? productDetails = await _unitOfWork.Product.GetFirstOrDefault(product => product.Id == productId);
                if (productDetails == null)
                    throw new Exception("Product not found");
                _unitOfWork.Product.Remove(productDetails);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        /// <summary>
        /// Gets All products
        /// </summary>
        /// <returns>List of Products</returns>
        public async Task<IEnumerable<ProductResponse>?> GetAllProducts(bool Active)
        {
            try
            {
                IEnumerable<Product>? listOfProducts = await _unitOfWork.Product.GetAll(product => product.Active == Active, includeProperties: nameof(Product.Categories));

                IEnumerable<ProductResponse>? listOfProductResponse = new List<ProductResponse>();
                _mapper.Map(listOfProducts, listOfProductResponse);

                return listOfProductResponse;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        /// <summary>
        /// Get a single product by Id
        /// </summary>
        /// <param name="id">Id of product to be returned</param>
        /// <returns>Product</returns>
        public async Task<ProductResponse?> GetProductById(int productId)
        {
            try
            {
                Product? product = await _unitOfWork.Product.GetFirstOrDefault(product => product.Id == productId);
                return new();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public async Task<ProductResponse> UpdateProduct(CreateProductRequest product)
        {
            try
            {
                Product? productFromDb = await _unitOfWork.Product.GetFirstOrDefault(productDetails => productDetails.Id == 0);
                if (productFromDb == null) throw new Exception("Product not found");

                //productFromDb.Title = product.Title ?? productFromDb.Title;

                _unitOfWork.Product.Update(productFromDb);
                await _unitOfWork.SaveAsync();

                return new();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

    }
}
