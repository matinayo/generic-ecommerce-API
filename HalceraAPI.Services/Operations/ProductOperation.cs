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

        public async Task<ProductDetailsResponse> CreateProduct(CreateProductRequest productRequest)
        {
            try
            {
                Product product = new();
                _mapper.Map(productRequest, product);

                if (productRequest.Categories != null && productRequest.Categories.Any())
                {
                    var categories = await _unitOfWork.Category.GetAll(category => productRequest.Categories != null && productRequest.Categories.Select(opt => opt.CategoryId).Contains(category.Id));
                    product.Categories = categories.ToList();
                }
                await _unitOfWork.Product.Add(product);
                await _unitOfWork.SaveAsync();

                ProductDetailsResponse productResponse = new();
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
        public async Task<IEnumerable<ProductResponse>?> GetAllProducts(bool? active, bool? featured, int? categoryId)
        {
            try
            {
                IEnumerable<Product>? listOfProducts = Enumerable.Empty<Product>();
                if (active.HasValue && featured.HasValue && categoryId.HasValue)
                {
                    listOfProducts = await GetActiveAndFeaturedProductsByCategoryId(active, featured, categoryId.Value, listOfProducts);
                }
                else if (active.HasValue && categoryId.HasValue)
                {
                    listOfProducts = await GetActiveProductsByCategoryId(active, categoryId.Value, listOfProducts);
                }
                else if (featured.HasValue && categoryId.HasValue)
                {
                    listOfProducts = await GetFeaturedProductsByCategoryId(featured, categoryId.Value, listOfProducts);
                }
                else if (active.HasValue && featured.HasValue)
                {
                    listOfProducts = await GetActiveAndFeaturedProducts(active, featured, listOfProducts);
                }
                else if (categoryId.HasValue)
                {
                    listOfProducts = await GetProductsByCategoryId(categoryId.Value, listOfProducts);
                }
                else if (active.HasValue)
                {
                    listOfProducts = await GetActiveProducts(active, listOfProducts);
                }
                else if (featured.HasValue)
                {
                    listOfProducts = await GetFeaturedProducts(featured, listOfProducts);
                }
                else
                {
                    listOfProducts = await GetAllProducts(listOfProducts);
                }
                IEnumerable<ProductResponse>? listOfProductResponse = new List<ProductResponse>();
                if (listOfProducts is not null && listOfProducts.Any())
                {
                    _mapper.Map(listOfProducts, listOfProductResponse);
                }
                return listOfProductResponse;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get a single product by Id
        /// </summary>
        /// <param name="id">Id of product to be returned</param>
        /// <returns>Product</returns>
        public async Task<ProductDetailsResponse?> GetProductById(int productId)
        {
            try
            {
                Product? product = await _unitOfWork.Product.GetFirstOrDefault(product => product.Id == productId, includeProperties: $"{nameof(Product.Categories)},{nameof(Product.ProductCompositions)},ProductCompositions.CompositionDataCollection,{nameof(Product.MediaCollection)},{nameof(Product.Prices)}");
                ProductDetailsResponse response = new();
                if (product is not null)
                {
                    response = _mapper.Map<ProductDetailsResponse>(product);
                }
                return response;
            }
            catch (Exception)
            {
                throw;
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

        /// <summary>
        /// Get All Products
        /// </summary>
        /// <param name="listOfProducts">IEnumerable</param>
        /// <returns>List of products from DB</returns>
        private async Task<IEnumerable<Product>> GetAllProducts(IEnumerable<Product>? listOfProducts)
        {
            listOfProducts = await _unitOfWork.Product.GetAll(
                                    includeProperties: $"{nameof(Product.Categories)},{nameof(Product.MediaCollection)},{nameof(Product.Prices)}");
            return listOfProducts;
        }

        /// <summary>
        /// Get products by featured query
        /// </summary>
        /// <param name="featured">Featured query</param>
        /// <param name="listOfProducts">IEnumerable</param>
        /// <returns>List of filtered products from DB</returns>
        private async Task<IEnumerable<Product>> GetFeaturedProducts(bool? featured, IEnumerable<Product>? listOfProducts)
        {
            listOfProducts = await _unitOfWork.Product.GetAll(product => product.Featured == featured,
                                    includeProperties: $"{nameof(Product.Categories)},{nameof(Product.MediaCollection)},{nameof(Product.Prices)}");
            return listOfProducts;
        }

        /// <summary>
        /// Get products by active query
        /// </summary>
        /// <param name="active">Active query</param>
        /// <param name="listOfProducts">IEnumerable</param>
        /// <returns>List of filtered products from DB</returns>
        private async Task<IEnumerable<Product>> GetActiveProducts(bool? active, IEnumerable<Product>? listOfProducts)
        {
            listOfProducts = await _unitOfWork.Product.GetAll(product => product.Active == active,
                                    includeProperties: $"{nameof(Product.Categories)},{nameof(Product.MediaCollection)},{nameof(Product.Prices)}");
            return listOfProducts;
        }

        /// <summary>
        /// Get products by both active and featured query
        /// </summary>
        /// <param name="active">Active query</param>
        /// <param name="featured">Featured query</param>
        /// <param name="listOfProducts">IEnumerable</param>
        /// <returns>List of filtered products from DB</returns>
        private async Task<IEnumerable<Product>> GetActiveAndFeaturedProducts(bool? active, bool? featured, IEnumerable<Product>? listOfProducts)
        {
            listOfProducts = await _unitOfWork.Product.GetAll(product => product.Active == active && product.Featured == featured,
                includeProperties: $"{nameof(Product.Categories)},{nameof(Product.MediaCollection)},{nameof(Product.Prices)}");
            return listOfProducts;
        }

        /// <summary>
        /// Get Featured products by category Id
        /// </summary>
        /// <param name="featured">Featured query</param>
        /// <param name="categoryId">Category Id</param>
        /// <param name="listOfProducts">IEnumerable</param>
        /// <returns>List of filtered products by category</returns>
        private async Task<IEnumerable<Product>> GetFeaturedProductsByCategoryId(bool? featured, int categoryId, IEnumerable<Product>? listOfProducts)
        {
            listOfProducts = await _unitOfWork.Product.GetAll(
                                    product => product.Featured == featured
                                     && product.Categories!.Any(category => category.Id == categoryId),
                                   includeProperties: $"{nameof(Product.Categories)},{nameof(Product.MediaCollection)},{nameof(Product.Prices)}");
            return listOfProducts;
        }

        /// <summary>
        /// Get Active products by category Id
        /// </summary>
        /// <param name="active">Active query</param>
        /// <param name="categoryId">Category Id</param>
        /// <param name="listOfProducts">IEnumerable</param>
        /// <returns>List of filtered products by category</returns>
        private async Task<IEnumerable<Product>> GetActiveProductsByCategoryId(bool? active, int categoryId, IEnumerable<Product>? listOfProducts)
        {
            listOfProducts = await _unitOfWork.Product.GetAll(
                                    product => product.Active == active
                                    && product.Categories!.Any(category => category.Id == categoryId),
                                   includeProperties: $"{nameof(Product.Categories)},{nameof(Product.MediaCollection)},{nameof(Product.Prices)}");
            return listOfProducts;
        }

        /// <summary>
        /// Get Active and Featured products by category Id
        /// </summary>
        /// <param name="active">Active query</param>
        /// <param name="featured">Featured query</param>
        /// <param name="categoryId">Category Id</param>
        /// <param name="listOfProducts">IEnumerable</param>
        /// <returns>List of filtered products by category</returns>
        private async Task<IEnumerable<Product>> GetActiveAndFeaturedProductsByCategoryId(bool? active, bool? featured, int categoryId, IEnumerable<Product>? listOfProducts)
        {
            listOfProducts = await _unitOfWork.Product.GetAll(
                                    product => product.Active == active && product.Featured == featured
                                    && product.Categories!.Any(category => category.Id == categoryId),
                                   includeProperties: $"{nameof(Product.Categories)},{nameof(Product.MediaCollection)},{nameof(Product.Prices)}");
            return listOfProducts;
        }

        /// <summary>
        /// Get products by category Id
        /// </summary>
        /// <param name="categoryId">Category Id</param>
        /// <param name="listOfProducts">IEnumerable</param>
        /// <returns>List of filtered products by category</returns>
        private async Task<IEnumerable<Product>> GetProductsByCategoryId(int categoryId, IEnumerable<Product>? listOfProducts)
        {
            listOfProducts = await _unitOfWork.Product.GetAll(
            product => product.Categories!.Any(category => category.Id == categoryId),
                                   includeProperties: $"{nameof(Product.Categories)},{nameof(Product.MediaCollection)},{nameof(Product.Prices)}");
            return listOfProducts;
        }
    }
}
