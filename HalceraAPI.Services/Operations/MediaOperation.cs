using AutoMapper;
using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;
using HalceraAPI.Models.Requests.Media;
using HalceraAPI.Services.Contract;

namespace HalceraAPI.Services.Operations
{
    public class MediaOperation : IMediaOperation
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MediaOperation(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task DeleteMediaCollection(int? categoryId, int? productId)
        {
            try
            {
                IEnumerable<Media>? relatedMediaCollection = null;

                if (categoryId != null)
                    relatedMediaCollection = await _unitOfWork.Media.GetAll(media => media.CategoryId == categoryId);
                else if (productId != null)
                    relatedMediaCollection = await _unitOfWork.Media.GetAll(media => media.ProductId == productId);

                if (relatedMediaCollection != null && relatedMediaCollection.Any())
                {
                    _unitOfWork.Media.RemoveRange(relatedMediaCollection);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteMediaFromCategoryByMediaIdAsync(int categoryId, int mediaId)
        {
            try
            {
                Media mediaToDelete = await _unitOfWork.Media
                    .GetFirstOrDefault(
                    media => media.Id == mediaId
                    && media.CategoryId == categoryId)
                    ?? throw new Exception("No media available for this product");

                _unitOfWork.Media.Remove(mediaToDelete);
                await _unitOfWork.SaveAsync();
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
                Media mediaToDelete = await _unitOfWork.Media
                    .GetFirstOrDefault(
                    media => media.Id == mediaId
                    && media.ProductId == productId)
                    ?? throw new Exception("No media available for this product");

                _unitOfWork.Media.Remove(mediaToDelete);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateMediaCollection(IEnumerable<UpdateMediaRequest>? mediaCollection, ICollection<Media>? mediaCollectionFromDb)
        {
            try
            {
                if (mediaCollection is not null && mediaCollection.Any())
                {
                    // Retrieve existing media from the database
                    mediaCollectionFromDb ??= new List<Media>();
                    foreach (var mediaRequest in mediaCollection)
                    {
                        // Find existing media with the same ID in the database
                        Media? existingMediaItem = mediaCollectionFromDb.FirstOrDefault(em => em.Id == mediaRequest.Id);

                        if (existingMediaItem != null)
                        {
                            // If the media already exists, update its properties
                            _mapper.Map(mediaRequest, existingMediaItem);
                        }
                        else
                        {
                            // If the media does not exist, create a new Media object and map the properties
                            if (mediaRequest.Type != null)
                            {
                                Media newMedia = _mapper.Map<Media>(mediaRequest);
                                mediaCollectionFromDb.Add(newMedia);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
