using AutoMapper;
using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;
using HalceraAPI.Models.Requests.Media;
using HalceraAPI.Models.Requests.Price;
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

        public async Task<bool> DeleteMediaCollection(int? categoryId, int? productId)
        {
            try
            {
                IEnumerable<Media>? relatedMediaCollection = null;

                if(categoryId != null)
                    relatedMediaCollection = await _unitOfWork.Media.GetAll(media => media.CategoryId == categoryId);
                else if(productId != null)
                    relatedMediaCollection = await _unitOfWork.Media.GetAll(media => media.ProductId == productId);

                if (relatedMediaCollection != null && relatedMediaCollection.Any())
                {
                    _unitOfWork.Media.RemoveRange(relatedMediaCollection);
                    return true;
                }
                return false;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public async Task<ICollection<Media>?> UpdateMediaCollection(IEnumerable<UpdateMediaRequest>? mediaCollection)
        {
            try
            {
                if (mediaCollection is not null && mediaCollection.Any())
                {
                    // Retrieve existing media from the database
                    IEnumerable<Media> existingMedia = await _unitOfWork.Media.GetAll(media => mediaCollection.Select(u => u.Id).Contains(media.Id));
                    List<Media>? mediaResponse = new();

                    foreach (var mediaRequest in mediaCollection)
                    {
                        // Find existing media with the same ID in the database
                        Media? existingMediaItem = existingMedia.FirstOrDefault(em => em.Id == mediaRequest.Id);

                        if (existingMediaItem != null)
                        {
                            // If the media already exists, update its properties
                            _mapper.Map(mediaRequest, existingMediaItem);
                            mediaResponse.Add(existingMediaItem);
                        }
                        else
                        {
                            // If the media does not exist, create a new Media object and map the properties
                            Media newMedia = _mapper.Map<Media>(mediaRequest);
                            mediaResponse.Add(newMedia);
                        }
                    }
                    return mediaResponse;
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
