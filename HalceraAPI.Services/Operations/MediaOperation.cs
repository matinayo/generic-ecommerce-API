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
