using AutoMapper;
using HalceraAPI.Common.Utilities;
using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;
using HalceraAPI.Services.Contract;
using HalceraAPI.Services.Dtos.APIResponse;
using HalceraAPI.Services.Dtos.Product;
using HalceraAPI.Services.Exceptions;
using HalceraAPI.Services.Exceptions.ErrorMessages;
using System.Linq.Expressions;

namespace HalceraAPI.Services.Operations
{
    public class ProductOperation : IProductOperation
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediaOperation _mediaOperation;
        private readonly ICompositionOperation _compositionOperation;
        private readonly ICompositionDataOperation _compositionDataOperation;
        private readonly IPriceOperation _priceOperation;
        private readonly ICategoryOperation _categoryOperation;

        public ProductOperation(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IMediaOperation mediaOperation,
            ICompositionOperation compositionOperation,
            ICompositionDataOperation compositionDataOperation,
            IPriceOperation priceOperation,
            ICategoryOperation categoryOperation)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediaOperation = mediaOperation;
            _compositionOperation = compositionOperation;
            _compositionDataOperation = compositionDataOperation;
            _priceOperation = priceOperation;
            _categoryOperation = categoryOperation;
        }

        public async Task<APIResponse<ProductDetailsResponse>> CreateProductAsync(CreateProductRequest productRequest)
        {
            Product product = _mapper.Map<Product>(productRequest);
            product.ValidateProductForCreate();
            product.Categories = await _categoryOperation.GetCategoriesByIdAsync(productRequest.Categories);

            await _unitOfWork.Product.Add(product);
            await _unitOfWork.SaveAsync();

            ProductDetailsResponse productResponse = _mapper.Map<ProductDetailsResponse>(product);

            return new APIResponse<ProductDetailsResponse>(productResponse);
        }

        public async Task DeleteProductAsync(int productId)
        {
            try
            {
                Product? productDetails = await _unitOfWork.Product.GetFirstOrDefault(product => product.Id == productId)
                    ?? throw new Exception("Product not found");

                // delete product media
                await _mediaOperation.DeleteMediaCollection(null, productId);

                // delete product composition and prices
                await _compositionOperation.DeleteProductCompositions(productId);
                await _priceOperation.DeleteProductPricesAsync(productId);

                _unitOfWork.Product.Remove(productDetails);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<APIResponse<IEnumerable<ProductResponse>>> GetAllProductsAsync(
            bool? active,
            bool? featured,
            int? categoryId,
            int? page)
        {
            try
            {
                Expression<Func<Product, bool>>? filterExpression = null;

                if (active.HasValue && featured.HasValue && categoryId.HasValue)
                {
                    filterExpression = GetActiveAndFeaturedProductsByCategoryId(active, featured, categoryId.Value);
                }
                else if (active.HasValue && categoryId.HasValue)
                {
                    filterExpression = GetActiveProductsByCategoryId(active, categoryId.Value);
                }
                else if (featured.HasValue && categoryId.HasValue)
                {
                    filterExpression = GetFeaturedProductsByCategoryId(featured, categoryId.Value);
                }
                else if (active.HasValue && featured.HasValue)
                {
                    filterExpression = GetActiveAndFeaturedProducts(active, featured);
                }
                else if (categoryId.HasValue)
                {
                    filterExpression = GetProductsByCategoryId(categoryId.Value);
                }
                else if (active.HasValue)
                {
                    filterExpression = GetActiveProducts(active);
                }
                else if (featured.HasValue)
                {
                    filterExpression = GetFeaturedProducts(featured);
                }

                int totalItems = await _unitOfWork.Product.CountAsync(filterExpression);
                IEnumerable<ProductResponse>? listOfProductResponse = await _unitOfWork.Product.GetAll<ProductResponse>(
                 filter: filterExpression,
                 skip: ((page ?? 1) - 1) * Pagination.DefaultItemsPerPage,
                 take: Pagination.DefaultItemsPerPage);

                var meta = new Meta(totalItems, Pagination.DefaultItemsPerPage, page ?? 1);

                return new APIResponse<IEnumerable<ProductResponse>>(listOfProductResponse, meta);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<APIResponse<ProductDetailsResponse>> GetProductByIdAsync(int productId)
        {
            ProductDetailsResponse response = await _unitOfWork.Product
                .GetFirstOrDefault<ProductDetailsResponse>(product => product.Id == productId)
                ?? throw new NotFoundException(message: ProductErrorMessage.ProductCannotBeFound);

            return new APIResponse<ProductDetailsResponse>(response);
        }

        public async Task<APIResponse<ProductDetailsResponse>> UpdateProductAsync(
            int productId,
            UpdateProductRequest productRequest)
        {
            Product productFromDb = await _unitOfWork.Product
                .GetFirstOrDefault(productDetails => productDetails.Id == productId,
                includeProperties: 
                    $"{nameof(Product.Compositions)}" +
                    ",Compositions.Sizes,Compositions.Prices,Compositions.MediaCollection" +
                    $",{nameof(Product.Categories)}" +
                    $",{nameof(Product.MaterialsAndDetails)}")
                ?? throw new NotFoundException(ProductErrorMessage.ProductCannotBeFound);

            _mapper.Map(productRequest, productFromDb);
            productFromDb.DateLastModified = DateTime.UtcNow;

            await _unitOfWork.SaveAsync();
            ProductDetailsResponse response = _mapper.Map<ProductDetailsResponse>(productFromDb);

            return new APIResponse<ProductDetailsResponse>(response);
        }

        private void CanUpdateProductRequest(Product productFromDb, UpdateProductRequest productRequest)
        {
            //// prevent duplicate CompositionType
            //List<Price> tempPrices = productFromDb.Prices?.ToList() ?? new();
            //tempPrices.AddRange(_mapper.Map<List<Price>>(productRequest.Prices));

            //var tempCompositions = productFromDb.ProductCompositions?.ToList() ?? new();
            //tempCompositions.AddRange(_mapper.Map<List<Composition>>(productRequest.ProductCompositions));

            //Product tempValidation = new()
            //{
            //    Prices = tempPrices,
            //    ProductCompositions = tempCompositions
            //};
            //tempValidation.ValidateProductForCreate();
        }

        public async Task DeleteCategoryFromProductByCategoryIdAsync(int productId, int categoryId)
        {
            try
            {
                await _categoryOperation.DeleteCategoryFromProductByCategoryIdAsync(productId, categoryId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeletePriceFromProductByPriceIdAsync(int productId, int priceId)
        {
            try
            {
                await _priceOperation.DeletePriceFromProductByPriceIdAsync(productId, priceId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteMediaFromProductByMediaIdAsync(int productId, int mediaId)
        {
            try
            {
                await _mediaOperation.DeleteMediaFromProductByMediaIdAsync(productId, mediaId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteCompositionFromProductByCompositionIdAsync(int productId, int compositionId)
        {
            try
            {
                await _compositionOperation.DeleteCompositionFromProductByCompositionIdAsync(productId, compositionId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteProductCompositionDataByCompositionDataIdAsync(
            int productId,
            int compositionId,
            int compositionDataId)
        {
            try
            {
                await _compositionDataOperation.DeleteCompositionDataFromProductCompositionAsync(
                    productId,
                    compositionId,
                    compositionDataId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task ResetDiscountOfProductPriceByPriceIdAsync(int productId, int priceId)
        {
            try
            {
                await _priceOperation.ResetDiscountOfProductPriceByPriceIdAsync(productId, priceId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static Expression<Func<Product, bool>> GetFeaturedProducts(bool? featured) => product => product.Featured == featured;

        private static Expression<Func<Product, bool>> GetActiveProducts(bool? active) => product => product.Active == active;

        private static Expression<Func<Product, bool>> GetActiveAndFeaturedProducts(
            bool? active, bool? featured) => product => product.Active == active && product.Featured == featured;

        private static Expression<Func<Product, bool>> GetFeaturedProductsByCategoryId(
            bool? featured, int categoryId) =>
            product => product.Featured == featured && product.Categories!.Any(category => category.Id == categoryId);

        private static Expression<Func<Product, bool>> GetActiveProductsByCategoryId(
            bool? active, int categoryId) =>
            product => product.Active == active && product.Categories!.Any(category => category.Id == categoryId);

        private static Expression<Func<Product, bool>> GetActiveAndFeaturedProductsByCategoryId(
            bool? active, bool? featured, int categoryId) =>
            product => product.Active == active && product.Featured == featured
            && product.Categories!.Any(category => category.Id == categoryId);

        private static Expression<Func<Product, bool>> GetProductsByCategoryId(int categoryId) =>
            product => product.Categories!.Any(category => category.Id == categoryId);
    }
}
