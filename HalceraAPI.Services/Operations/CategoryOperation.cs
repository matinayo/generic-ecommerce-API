using AutoMapper;
using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;
using HalceraAPI.Models.Requests.Category;
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

        public CategoryOperation(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CategoryResponse> CreateCategory(CategoryRequest categoryRequest)
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
                Category? categoryDetails = await _unitOfWork.Category.GetFirstOrDefault(category => category.Id == categoryId);
                if (categoryDetails == null)
                    throw new Exception("Category not found");

                IEnumerable<Media> relatedMediaCollection = await _unitOfWork.Media.GetAll(media => media.CategoryId == categoryId);
                _unitOfWork.Media.RemoveRange(relatedMediaCollection);

                //IEnumerable<Product> relatedProducts = await _unitOfWork.Product.GetAll(product => product.CategoryId == categoryId);
                //_unitOfWork.Product.RemoveRange(relatedProducts);

                _unitOfWork.Category.Remove(categoryDetails);

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
                IEnumerable<Category>? listOfCategories = await _unitOfWork.Category.GetAll(includeProperties: $"{nameof(Category.Products)},{nameof(Category.MediaCollection)}");
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
                Category? categoryDetails = await _unitOfWork.Category.GetFirstOrDefault(category => category.Id == categoryId);
                if (categoryDetails == null)
                    throw new Exception("Category not found");

                CategoryResponse response = new();
                _mapper.Map(categoryDetails, response);

                return response;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public async Task<CategoryResponse> UpdateCategory(CategoryRequest category)
        {
            try
            {
                Category? categoryDetails = await _unitOfWork.Category.GetFirstOrDefault(); // categoryDb => categoryDb.Id == category.Id
                if (categoryDetails == null)
                    throw new Exception("Category not found");

                categoryDetails.Title = category.Title ?? categoryDetails.Title;

                _unitOfWork.Category.Update(categoryDetails);
                return new();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }
    }
}
