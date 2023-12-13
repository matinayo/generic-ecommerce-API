using AutoMapper;
using HalceraAPI.Common.Utilities;
using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;
using HalceraAPI.Models.Requests.APIResponse;
using HalceraAPI.Models.Requests.Product;
using HalceraAPI.Services.Contract;

namespace HalceraAPI.Services.Operations
{
    public class ProductOperation : IProductOperation
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediaOperation _mediaOperation;
        private readonly ICompositionOperation _compositionOperation;
        private readonly IPriceOperation _priceOperation;
        private readonly ICategoryOperation _categoryOperation;

        public ProductOperation(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IMediaOperation mediaOperation,
            ICompositionOperation compositionOperation,
            IPriceOperation priceOperation,
            ICategoryOperation categoryOperation)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediaOperation = mediaOperation;
            _compositionOperation = compositionOperation;
            _priceOperation = priceOperation;
            _categoryOperation = categoryOperation;
        }

        public async Task<APIResponse<ProductDetailsResponse>> CreateProductAsync(CreateProductRequest productRequest)
        {
            try
            {
                Product product = new();
                _mapper.Map(productRequest, product);

                if (productRequest.Categories != null && productRequest.Categories.Any())
                {
                    var categories = await _unitOfWork.Category.GetAll(
                        category => productRequest.Categories != null
                        && productRequest.Categories.Select(opt => opt.CategoryId).Contains(category.Id));

                    if (categories != null && categories.Any())
                    {
                        product.Categories = categories.ToList();
                    }
                }
                await _unitOfWork.Product.Add(product);
                await _unitOfWork.SaveAsync();

                ProductDetailsResponse productResponse = _mapper.Map<ProductDetailsResponse>(product);

                return new APIResponse<ProductDetailsResponse>(productResponse);
            }
            catch (Exception) { throw; }
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            try
            {
                Product? productDetails = await _unitOfWork.Product.GetFirstOrDefault(product => product.Id == productId);
                if (productDetails == null)
                    throw new Exception("Product not found");

                // delete product media
                await _mediaOperation.DeleteMediaCollection(null, productId);

                // delete product composition and prices
                await _compositionOperation.DeleteProductCompositions(productId);
                await _priceOperation.DeleteProductPrices(productId);

                _unitOfWork.Product.Remove(productDetails);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<APIResponse<IEnumerable<ProductResponse>>> GetAllProductsAsync(bool? active, bool? featured, int? categoryId, int? page)
        {
            try
            {
                IEnumerable<Product>? listOfProducts = Enumerable.Empty<Product>();
                if (active.HasValue && featured.HasValue && categoryId.HasValue)
                {
                    listOfProducts = await GetActiveAndFeaturedProductsByCategoryId(active, featured, categoryId.Value, listOfProducts, page);
                }
                else if (active.HasValue && categoryId.HasValue)
                {
                    listOfProducts = await GetActiveProductsByCategoryId(active, categoryId.Value, listOfProducts, page);
                }
                else if (featured.HasValue && categoryId.HasValue)
                {
                    listOfProducts = await GetFeaturedProductsByCategoryId(featured, categoryId.Value, listOfProducts, page);
                }
                else if (active.HasValue && featured.HasValue)
                {
                    listOfProducts = await GetActiveAndFeaturedProducts(active, featured, listOfProducts, page);
                }
                else if (categoryId.HasValue)
                {
                    listOfProducts = await GetProductsByCategoryId(categoryId.Value, listOfProducts, page);
                }
                else if (active.HasValue)
                {
                    listOfProducts = await GetActiveProducts(active, listOfProducts, page);
                }
                else if (featured.HasValue)
                {
                    listOfProducts = await GetFeaturedProducts(featured, listOfProducts, page);
                }
                else
                {
                    listOfProducts = await GetAllProducts(listOfProducts, page);
                }

                IEnumerable<ProductResponse>? listOfProductResponse = new List<ProductResponse>();
                if (listOfProducts is not null && listOfProducts.Any())
                {
                    _mapper.Map(listOfProducts, listOfProductResponse);
                }

                return new APIResponse<IEnumerable<ProductResponse>>(listOfProductResponse);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<APIResponse<ProductDetailsResponse>> GetProductByIdAsync(int productId)
        {
            try
            {
                Product? product = await _unitOfWork.Product.GetFirstOrDefault(product => product.Id == productId,
                    includeProperties: $"{nameof(Product.Categories)},{nameof(Product.ProductCompositions)},ProductCompositions.CompositionDataCollection,{nameof(Product.MediaCollection)},{nameof(Product.Prices)}");

                ProductDetailsResponse response = new();
                if (product is not null)
                {
                    response = _mapper.Map<ProductDetailsResponse>(product);
                }

                return new APIResponse<ProductDetailsResponse>(response);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<APIResponse<ProductDetailsResponse>> UpdateProductAsync(int productId, UpdateProductRequest productRequest)
        {
            try
            {
                Product? productFromDb = await _unitOfWork.Product.GetFirstOrDefault(productDetails => productDetails.Id == productId,
                    includeProperties: $"{nameof(Product.ProductCompositions)},ProductCompositions.CompositionDataCollection,{nameof(Product.Prices)},{nameof(Product.MediaCollection)}");
                if (productFromDb is null) throw new Exception("Product not found");

                _priceOperation.UpdatePrice(productRequest.Prices, productFromDb.Prices);
                _mediaOperation.UpdateMediaCollection(productRequest.MediaCollection, productFromDb.MediaCollection);
                _compositionOperation.UpdateComposition(productRequest.ProductCompositions, productFromDb.ProductCompositions);

                var categories = await _categoryOperation.GetCategoriesFromListOfCategoryIdAsync(productRequest.Categories);
                if (categories != null && categories.Any())
                {
                    productFromDb.Categories = categories.ToList();
                }
                bool tempActiveValue = productFromDb.Active;
                _mapper.Map(productRequest, productFromDb);
                // restore Active value
                if (productRequest.Active == null)
                {
                    productFromDb.Active = tempActiveValue;
                }

                productFromDb.DateLastModified = DateTime.UtcNow;
                await _unitOfWork.SaveAsync();

                ProductDetailsResponse response = _mapper.Map<ProductDetailsResponse>(productFromDb);

                return new APIResponse<ProductDetailsResponse>(response);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<IEnumerable<Product>> GetAllProducts(IEnumerable<Product>? listOfProducts, int? page)
        {
            listOfProducts = await _unitOfWork.Product.GetAll(
                                    includeProperties: $"{nameof(Product.Categories)},{nameof(Product.MediaCollection)},{nameof(Product.Prices)}", 
                                    skip: ((page ?? 1) - 1) * Pagination.DefaultItemsPerPage,
                                    take: Pagination.DefaultItemsPerPage);

            return listOfProducts;
        }

        /// <summary>
        /// Get products by featured query
        /// </summary>
        /// <param name="featured">Featured query</param>
        /// <param name="listOfProducts">IEnumerable</param>
        /// <returns>List of filtered products from DB</returns>
        private async Task<IEnumerable<Product>> GetFeaturedProducts(bool? featured, IEnumerable<Product>? listOfProducts, int? page)
        {
            listOfProducts = await _unitOfWork.Product.GetAll(product => product.Featured == featured,
                                    includeProperties: $"{nameof(Product.Categories)},{nameof(Product.MediaCollection)},{nameof(Product.Prices)}", 
                                    skip: ((page ?? 1) - 1) * Pagination.DefaultItemsPerPage,
                                    take: Pagination.DefaultItemsPerPage);

            return listOfProducts;
        }

        /// <summary>
        /// Get products by active query
        /// </summary>
        /// <param name="active">Active query</param>
        /// <param name="listOfProducts">IEnumerable</param>
        /// <returns>List of filtered products from DB</returns>
        private async Task<IEnumerable<Product>> GetActiveProducts(bool? active, IEnumerable<Product>? listOfProducts, int? page)
        {
            listOfProducts = await _unitOfWork.Product.GetAll(product => product.Active == active,
                                    includeProperties: $"{nameof(Product.Categories)},{nameof(Product.MediaCollection)},{nameof(Product.Prices)}", 
                                    skip: ((page ?? 1) - 1) * Pagination.DefaultItemsPerPage,
                                    take: Pagination.DefaultItemsPerPage);

            return listOfProducts;
        }

        /// <summary>
        /// Get products by both active and featured query
        /// </summary>
        /// <param name="active">Active query</param>
        /// <param name="featured">Featured query</param>
        /// <param name="listOfProducts">IEnumerable</param>
        /// <returns>List of filtered products from DB</returns>
        private async Task<IEnumerable<Product>> GetActiveAndFeaturedProducts(
            bool? active,
            bool? featured, 
            IEnumerable<Product>? listOfProducts,
            int? page)
        {
            listOfProducts = await _unitOfWork.Product.GetAll(
                 product => product.Active == active && product.Featured == featured,
                 includeProperties: $"{nameof(Product.Categories)},{nameof(Product.MediaCollection)},{nameof(Product.Prices)}",
                 skip: ((page ?? 1) - 1) * Pagination.DefaultItemsPerPage,
                 take: Pagination.DefaultItemsPerPage);

            return listOfProducts;
        }

        /// <summary>
        /// Get Featured products by category Id
        /// </summary>
        /// <param name="featured">Featured query</param>
        /// <param name="categoryId">Category Id</param>
        /// <param name="listOfProducts">IEnumerable</param>
        /// <returns>List of filtered products by category</returns>
        private async Task<IEnumerable<Product>> GetFeaturedProductsByCategoryId(
            bool? featured,
            int categoryId,
            IEnumerable<Product>? listOfProducts,
            int? page)
        {
            listOfProducts = await _unitOfWork.Product.GetAll(
                                    product => product.Featured == featured
                                    && product.Categories!.Any(category => category.Id == categoryId),
                                    includeProperties: $"{nameof(Product.Categories)},{nameof(Product.MediaCollection)},{nameof(Product.Prices)}",
                                    skip: ((page ?? 1) - 1) * Pagination.DefaultItemsPerPage,
                                    take: Pagination.DefaultItemsPerPage);

            return listOfProducts;
        }

        /// <summary>
        /// Get Active products by category Id
        /// </summary>
        /// <param name="active">Active query</param>
        /// <param name="categoryId">Category Id</param>
        /// <param name="listOfProducts">IEnumerable</param>
        /// <returns>List of filtered products by category</returns>
        private async Task<IEnumerable<Product>> GetActiveProductsByCategoryId(bool? active, int categoryId, IEnumerable<Product>? listOfProducts, int? page)
        {
            listOfProducts = await _unitOfWork.Product.GetAll(
                                    product => product.Active == active
                                    && product.Categories!.Any(category => category.Id == categoryId),
                                    includeProperties: $"{nameof(Product.Categories)},{nameof(Product.MediaCollection)},{nameof(Product.Prices)}",
                                    skip: ((page ?? 1) - 1) * Pagination.DefaultItemsPerPage,
                                    take: Pagination.DefaultItemsPerPage);

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
        private async Task<IEnumerable<Product>> GetActiveAndFeaturedProductsByCategoryId(
            bool? active,
            bool? featured,
            int categoryId,
            IEnumerable<Product>? listOfProducts,
            int? page)
        {
            listOfProducts = await _unitOfWork.Product.GetAll(
                                    product => product.Active == active && product.Featured == featured
                                    && product.Categories!.Any(category => category.Id == categoryId),
                                    includeProperties: $"{nameof(Product.Categories)},{nameof(Product.MediaCollection)},{nameof(Product.Prices)}",
                                    skip: ((page ?? 1) - 1) * Pagination.DefaultItemsPerPage,
                                    take: Pagination.DefaultItemsPerPage);

            return listOfProducts;
        }

        /// <summary>
        /// Get products by category Id
        /// </summary>
        /// <param name="categoryId">Category Id</param>
        /// <param name="listOfProducts">IEnumerable</param>
        /// <returns>List of filtered products by category</returns>
        private async Task<IEnumerable<Product>> GetProductsByCategoryId(int categoryId, IEnumerable<Product>? listOfProducts, int? page)
        {
            listOfProducts = await _unitOfWork.Product.GetAll(
                                    filter: product => product.Categories!.Any(category => category.Id == categoryId),
                                   includeProperties: $"{nameof(Product.Categories)},{nameof(Product.MediaCollection)},{nameof(Product.Prices)}",
                                   skip: ((page ?? 1) - 1) * Pagination.DefaultItemsPerPage,
                                   take: Pagination.DefaultItemsPerPage);

            return listOfProducts;
        }
    }
}
