using AutoMapper;
using HalceraAPI.Common.Utilities;
using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;
using HalceraAPI.Models.Requests.APIResponse;
using HalceraAPI.Models.Requests.Category;
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
            try
            {
                Category category = new();
                _mapper.Map(categoryRequest, category);

                await _unitOfWork.Category.Add(category);
                await _unitOfWork.SaveAsync();

                return new APIResponse<CategoryResponse>(
                    _mapper.Map<CategoryResponse>(category));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteCategoryAsync(int categoryId)
        {
            try
            {
                Category? categoryDetailsFromDb = await _unitOfWork.Category.GetFirstOrDefault(category => category.Id == categoryId)
                    ?? throw new Exception("Category not found");

                await _mediaOperation.DeleteMediaCollection(categoryId, null);
                _unitOfWork.Category.Remove(categoryDetailsFromDb);

                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<APIResponse<IEnumerable<CategoryResponse>>> GetAllCategoriesAsync(bool? active, bool? featured, int? page)
        {
            try
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
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Category>?> GetCategoriesFromListOfCategoryIdAsync(IEnumerable<ProductCategoryRequest>? categoryRequests)
        {
            try
            {
                if (categoryRequests != null && categoryRequests.Any())
                {
                    var categories = await _unitOfWork.Category.GetAll(
                        category => categoryRequests.Select(opt => opt.CategoryId).Contains(category.Id));

                    return categories;
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<APIResponse<CategoryResponse>> GetCategoryAsync(int categoryId)
        {
            try
            {
                Category categoryDetailsFromDb = await _unitOfWork.Category.GetFirstOrDefault(
                    category => category.Id == categoryId, 
                    includeProperties: nameof(Category.MediaCollection)) 
                    ?? throw new Exception("Category not found");

                CategoryResponse response = _mapper.Map<CategoryResponse>(categoryDetailsFromDb);

                return new APIResponse<CategoryResponse>(response);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<APIResponse<CategoryResponse>> UpdateCategoryAsync(int categoryId, UpdateCategoryRequest category)
        {
            try
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
            catch (Exception)
            {
                throw;
            }
        }
    }
}
