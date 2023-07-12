using AutoMapper;
using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;
using HalceraAPI.Models.Requests.Category;
using HalceraAPI.Models.Requests.Media;
using HalceraAPI.Services.Contract;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Services.Operations
{
    /// <summary>
    /// Category Operations
    /// </summary>
    public class CategoryOperation : ICategoryOperation
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediaOperation _mediaService;

        public CategoryOperation(IUnitOfWork unitOfWork, IMapper mapper, IMediaOperation mediaService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediaService = mediaService;
        }

        public async Task<CategoryResponse> CreateCategory(CreateCategoryRequest categoryRequest)
        {
            try
            {
                // Validate model
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(categoryRequest);
                bool isValid = Validator.TryValidateObject(categoryRequest, validationContext, validationResults, true);
                if (!isValid)
                {
                    string errorMessage = JsonConvert.SerializeObject(validationResults);
                    throw new Exception(errorMessage);
                }

                Category category = new();
                _mapper.Map(categoryRequest, category);

                await _unitOfWork.Category.Add(category);
                await _unitOfWork.SaveAsync();

                // Return category response
                CategoryResponse categoryResponse = new();
                _mapper.Map(category, categoryResponse);

                return categoryResponse;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public async Task<bool> DeleteCategory(int categoryId)
        {
            try
            {
                Category? categoryDetailsFromDb = await _unitOfWork.Category.GetFirstOrDefault(category => category.Id == categoryId);
                if (categoryDetailsFromDb == null)
                    throw new Exception("Category not found");

                IEnumerable<Media>? relatedMediaCollection = await _unitOfWork.Media.GetAll(media => media.CategoryId == categoryId);
                if(relatedMediaCollection != null && relatedMediaCollection.Any())
                    _unitOfWork.Media.RemoveRange(relatedMediaCollection);

                _unitOfWork.Category.Remove(categoryDetailsFromDb);

                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public async Task<IEnumerable<CategoryResponse>?> GetAllCategories()
        {
            try
            {
                IEnumerable<CategoryResponse> response = new List<CategoryResponse>();
                IEnumerable<Category>? listOfCategories = await _unitOfWork.Category.GetAll(includeProperties: nameof(Category.MediaCollection));
                if (listOfCategories is not null && listOfCategories.Any())
                    _mapper.Map(listOfCategories, response);

                return response;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
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
                Category? categoryDetailsFromDb = await _unitOfWork.Category.GetFirstOrDefault(categoryDb => categoryDb.Id == categoryId);
                if (categoryDetailsFromDb == null)
                    throw new Exception("Category not found");

                categoryDetailsFromDb.MediaCollection = await _mediaService.UpdateMediaCollection(category.MediaCollection);

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
