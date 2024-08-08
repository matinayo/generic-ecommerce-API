using AutoMapper;
using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;
using HalceraAPI.Services.Contract;
using HalceraAPI.Services.Dtos.Media;

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

        public async Task DeleteMediaByListOfCompositionIdAsync(List<int> compositionIds)
        {
            IEnumerable<Media>? mediaCollection = await _unitOfWork.Media.GetAll(
                                                        media => media.CompositionId != null
                                                        && compositionIds.Contains(media.CompositionId ?? 0));

            if (mediaCollection is not null && mediaCollection.Any())
            {
                _unitOfWork.Media.RemoveRange(mediaCollection);
            }
        }

        public async Task DeleteMediaCollection(int? categoryId)
        {
            IEnumerable<Media>? relatedMediaCollection = null;

            if (categoryId != null)
                relatedMediaCollection = await _unitOfWork.Media.GetAll(media => media.CategoryId == categoryId);

            if (relatedMediaCollection != null && relatedMediaCollection.Any())
            {
                _unitOfWork.Media.RemoveRange(relatedMediaCollection);
            }
        }

        public async Task DeleteMediaFromCategoryByMediaIdAsync(int categoryId, int mediaId)
        {
            Media mediaToDelete = await _unitOfWork.Media
                .GetFirstOrDefault(
                media => media.Id == mediaId
                && media.CategoryId == categoryId)
                ?? throw new Exception("No media available for this product");

            _unitOfWork.Media.Remove(mediaToDelete);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteMediaFromCompositionByMediaIdAsync(int compositionId, int mediaId)
        {
            Media mediaToDelete = await _unitOfWork.Media
                .GetFirstOrDefault(
                media => media.Id == mediaId && media.CompositionId == compositionId)
                ?? throw new Exception("No media available for this composition");

            _unitOfWork.Media.Remove(mediaToDelete);
            await _unitOfWork.SaveAsync();
        }

        public void UpdateMediaCollection(IEnumerable<UpdateMediaRequest>? mediaCollection, ICollection<Media>? mediaCollectionFromDb)
        {
            if (mediaCollection is not null && mediaCollection.Any())
            {
                mediaCollectionFromDb ??= new List<Media>();
                foreach (var mediaRequest in mediaCollection)
                {
                    Media? existingMediaItem = mediaCollectionFromDb.FirstOrDefault(em => em.Id == mediaRequest.Id);
                    if (existingMediaItem != null)
                    {
                        _mapper.Map(mediaRequest, existingMediaItem);
                    }
                    else
                    {
                        if (mediaRequest.Type != null)
                        {
                            existingMediaItem = _mapper.Map<Media>(mediaRequest);
                            mediaCollectionFromDb.Add(existingMediaItem);
                        }
                    }
                }
            }

        }
    }
}
