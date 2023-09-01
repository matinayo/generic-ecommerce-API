using AutoMapper;
using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;
using HalceraAPI.Models.Requests.Category;
using HalceraAPI.Services.Contract;
using System.Linq.Expressions;

namespace HalceraAPI.Services.Operations
{
    /// <summary>
    /// Category Operations
    /// </summary>
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

        public async Task<CategoryResponse> CreateCategory(CreateCategoryRequest categoryRequest)
        {
            try
            {
                Category category = new();
                _mapper.Map(categoryRequest, category);

                await _unitOfWork.Category.Add(category);
                await _unitOfWork.SaveAsync();

                // Return category response
                CategoryResponse categoryResponse = new();
                _mapper.Map(category, categoryResponse);

                return categoryResponse;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteCategory(int categoryId)
        {
            try
            {
                Category? categoryDetailsFromDb = await _unitOfWork.Category.GetFirstOrDefault(category => category.Id == categoryId);
                if (categoryDetailsFromDb == null)
                    throw new Exception("Category not found");

                _ = await _mediaOperation.DeleteMediaCollection(categoryId, null);
                _unitOfWork.Category.Remove(categoryDetailsFromDb);

                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<CategoryResponse>?> GetAllCategories(bool? active, bool? featured)
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
                return await _unitOfWork.Category.GetAll<CategoryResponse>(filter: filterExpression, includeProperties: nameof(Category.MediaCollection));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Category>?> GetCategoriesFromListOfCategoryId(IEnumerable<ProductCategoryRequest>? categoryRequests)
        {
            try
            {
                if (categoryRequests != null && categoryRequests.Any())
                {
                    var categories = await _unitOfWork.Category.GetAll(category => categoryRequests.Select(opt => opt.CategoryId).Contains(category.Id));
                    return categories;
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CategoryResponse?> GetCategory(int categoryId)
        {
            try
            {
                Category? categoryDetailsFromDb = await _unitOfWork.Category.GetFirstOrDefault(category => category.Id == categoryId, includeProperties: nameof(Category.MediaCollection));
                if (categoryDetailsFromDb == null)
                    throw new Exception("Category not found");

                CategoryResponse response = new();
                _mapper.Map(categoryDetailsFromDb, response);

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CategoryResponse> UpdateCategory(int categoryId, UpdateCategoryRequest category)
        {
            try
            {
                Category? categoryDetailsFromDb = await _unitOfWork.Category.GetFirstOrDefault(categoryDb => categoryDb.Id == categoryId, includeProperties: $"{nameof(Category.MediaCollection)}");
                if (categoryDetailsFromDb == null)
                    throw new Exception("Category not found");
                
                _mediaOperation.UpdateMediaCollection(category.MediaCollection, categoryDetailsFromDb.MediaCollection);

                _mapper.Map(category, categoryDetailsFromDb);
                categoryDetailsFromDb.DateLastModified = DateTime.UtcNow;
                await _unitOfWork.SaveAsync();

                CategoryResponse response = new();
                _mapper.Map(categoryDetailsFromDb, response);

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
