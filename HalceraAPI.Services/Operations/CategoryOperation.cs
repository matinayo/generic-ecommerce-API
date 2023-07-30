using AutoMapper;
using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;
using HalceraAPI.Models.Requests.Category;
using HalceraAPI.Services.Contract;

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
                IEnumerable<CategoryResponse> response;
                IEnumerable<Category>? listOfCategories;

                if (active.HasValue && featured != null)
                {
                    listOfCategories = await _unitOfWork.Category.GetAll(
                        category => category.Active == active && category.Featured == featured, includeProperties: nameof(Category.MediaCollection));
                }
                else if (active != null)
                {
                    listOfCategories = await _unitOfWork.Category.GetAll(category => category.Active == active, includeProperties: nameof(Category.MediaCollection));
                }
                else if (featured != null)
                {
                    listOfCategories = await _unitOfWork.Category.GetAll(category => category.Featured == featured, includeProperties: nameof(Category.MediaCollection));
                }
                else
                {
                    listOfCategories = await _unitOfWork.Category.GetAll(includeProperties: nameof(Category.MediaCollection));
                }

                if (listOfCategories is not null && listOfCategories.Any())
                    response = _mapper.Map<IEnumerable<CategoryResponse>>(listOfCategories);
                else
                    response = Enumerable.Empty<CategoryResponse>();

                return response;
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
