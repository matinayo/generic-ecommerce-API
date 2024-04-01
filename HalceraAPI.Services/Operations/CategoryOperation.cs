using AutoMapper;
using HalceraAPI.Common.Utilities;
using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;
using HalceraAPI.Services.Dtos.APIResponse;
using HalceraAPI.Services.Dtos.Category;
using HalceraAPI.Services.Contract;
using System.Linq.Expressions;

namespace HalceraAPI.Services.Operations
{
    public class CategoryOperation : ICategoryOperation
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediaOperation _mediaOperation;

        public CategoryOperation(IUnitOfWork unitOfWork, IMapper mapper, IMediaOperation mediaService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediaOperation = mediaService;
        }

        public async Task<APIResponse<CategoryResponse>> CreateCategoryAsync(CreateCategoryRequest categoryRequest)
        {
            Category category = new();
            _mapper.Map(categoryRequest, category);

            await _unitOfWork.Category.Add(category);
            await _unitOfWork.SaveAsync();

            return new APIResponse<CategoryResponse>(
                _mapper.Map<CategoryResponse>(category));
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            Category? categoryDetailsFromDb = await _unitOfWork.Category.GetFirstOrDefault(category => category.Id == categoryId)
                ?? throw new Exception("Category not found");

            await _mediaOperation.DeleteMediaCollection(categoryId, null);
            _unitOfWork.Category.Remove(categoryDetailsFromDb);

            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteCategoryFromProductByCategoryIdAsync(int productId, int categoryId)
        {
            Product product = await _unitOfWork.Product.GetFirstOrDefault(
                filter: product => product.Id == productId,
                includeProperties: nameof(Product.Categories))
                ?? throw new Exception("This product cannot be found");

            if (product.Categories == null || !product.Categories.Any())
            {
                throw new Exception("No categories available for this product");
            }

            Category categoryToDelete = product.Categories.FirstOrDefault(category => category.Id == categoryId)
                ?? throw new Exception("This category cannot be found");

            product.Categories.Remove(categoryToDelete);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteMediaFromCategoryByMediaIdAsync(int categoryId, int mediaId)
        {
            await _mediaOperation.DeleteMediaFromCategoryByMediaIdAsync(categoryId, mediaId);

        }

        public async Task<APIResponse<IEnumerable<CategoryResponse>>> GetAllCategoriesAsync(
            bool? active,
            bool? featured,
            int? page)
        {
            Expression<Func<Category, bool>>? filterExpression = null;
            if (active.HasValue && featured != null)
            {
                filterExpression = category => category.Active == active && category.Featured == featured;
            }
            else if (active != null)
            {
                filterExpression = category => category.Active == active;
            }
            else if (featured != null)
            {
                filterExpression = category => category.Featured == featured;
            }

            int totalItems = await _unitOfWork.Category.CountAsync(filterExpression);
            var result = await _unitOfWork.Category.GetAll<CategoryResponse>(
                filter: filterExpression,
                includeProperties: nameof(Category.MediaCollection),
                skip: ((page ?? 1) - 1) * Pagination.DefaultItemsPerPage,
                take: Pagination.DefaultItemsPerPage);

            var meta = new Meta(totalItems, Pagination.DefaultItemsPerPage, page ?? 1);

            return new APIResponse<IEnumerable<CategoryResponse>>(result, meta);

        }

        public async Task<IEnumerable<Category>?> GetCategoriesFromListOfCategoryIdAsync(
            IEnumerable<ProductCategoryRequest>? categoryRequests)
        {
            if (categoryRequests != null && categoryRequests.Any())
            {
                var categories = await _unitOfWork.Category.GetAll(
                    category => categoryRequests.Select(opt => opt.CategoryId).Contains(category.Id));

                return categories;
            }

            return null;
        }

        public async Task<APIResponse<CategoryResponse>> GetCategoryAsync(int categoryId)
        {
            Category categoryDetailsFromDb = await _unitOfWork.Category.GetFirstOrDefault(
                category => category.Id == categoryId,
                includeProperties: nameof(Category.MediaCollection))
                ?? throw new Exception("Category not found");

            CategoryResponse response = _mapper.Map<CategoryResponse>(categoryDetailsFromDb);

            return new APIResponse<CategoryResponse>(response);
        }

        public async Task<APIResponse<CategoryResponse>> UpdateCategoryAsync(
            int categoryId,
            UpdateCategoryRequest category)
        {
            Category? categoryDetailsFromDb = await _unitOfWork.Category.GetFirstOrDefault(
                categoryDb => categoryDb.Id == categoryId,
                includeProperties: $"{nameof(Category.MediaCollection)}")
                ?? throw new Exception("Category not found");

            _mediaOperation.UpdateMediaCollection(category.MediaCollection, categoryDetailsFromDb.MediaCollection);

            _mapper.Map(category, categoryDetailsFromDb);
            categoryDetailsFromDb.DateLastModified = DateTime.UtcNow;
            await _unitOfWork.SaveAsync();

            CategoryResponse response = _mapper.Map<CategoryResponse>(categoryDetailsFromDb);

            return new APIResponse<CategoryResponse>(response);
        }
    }
}
