using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Model;
using HalceraAPI.Services.Contract;

namespace HalceraAPI.Services.Operations
{
    public class CategoryOperation : ICategoryOperation
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryOperation(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Category> CreateCategory(Category category)
        {
            try
            {
                await _unitOfWork.CategoryRepository.Add(category);
                await _unitOfWork.SaveAsync();
                return category;
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
                Category? categoryDetails = await _unitOfWork.CategoryRepository.GetFirstOrDefault(category => category.Id == categoryId);
                if (categoryDetails == null)
                    throw new Exception("Category not found");


                _unitOfWork.CategoryRepository.Remove(categoryDetails);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public async Task<IEnumerable<Category>?> GetAllCategories()
        {
            try
            {
                IEnumerable<Category>? listOfCategories = await _unitOfWork.CategoryRepository.GetAll();
                return listOfCategories;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public async Task<Category?> GetCategory(int categoryId)
        {
            try
            {
                Category? categoryDetails = await _unitOfWork.CategoryRepository.GetFirstOrDefault(category => category.Id == categoryId);
                if (categoryDetails == null)
                    throw new Exception("Category not found");
                return categoryDetails;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public async Task<Category> UpdateCategory(Category category)
        {
            try
            {
                Category? categoryDetails = await _unitOfWork.CategoryRepository.GetFirstOrDefault(categoryDb => categoryDb.Id == category.Id);
                if (categoryDetails == null)
                    throw new Exception("Category not found");

                categoryDetails.Title = category.Title ?? categoryDetails.Title;
                categoryDetails.Description = category.Description ?? categoryDetails.Description;
                categoryDetails.ImageURL = category.ImageURL ?? categoryDetails.ImageURL;
                categoryDetails.VideoURL = category.VideoURL ?? categoryDetails.VideoURL;

                _unitOfWork.CategoryRepository.Update(categoryDetails);
                return categoryDetails;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }
    }
}
