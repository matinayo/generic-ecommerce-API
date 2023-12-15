using AutoMapper;
using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;
using HalceraAPI.Models.Requests.Composition.CompositionData;
using HalceraAPI.Services.Contract;

namespace HalceraAPI.Services.Operations
{
    public class CompositionDataOperation : ICompositionDataOperation
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CompositionDataOperation(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task DeleteCompositionDataCollectionAsync(IEnumerable<int> compositionIdCollection)
        {
            try
            {
                IEnumerable<CompositionData>? compositionDataCollection = await _unitOfWork.CompositionData.GetAll(
                    compositionData => compositionData.CompositionId != null
                    && compositionIdCollection.Contains(compositionData.CompositionId ?? 0));

                if (compositionDataCollection is not null && compositionDataCollection.Any())
                {
                    _unitOfWork.CompositionData.RemoveRange(compositionDataCollection);
                    await _unitOfWork.SaveAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteCompositionDataFromProductCompositionAsync(int productId, int compositionId, int compositionDataId)
        {
            try
            {
                CompositionData compositionDataToDelete = await _unitOfWork.CompositionData
                    .GetFirstOrDefault(
                    compositionData => compositionData.Id == compositionDataId
                    && compositionData.CompositionId == compositionId, includeProperties: nameof(CompositionData.Composition))
                    ?? throw new Exception("No composition data available for this composition");

                if (compositionDataToDelete.Composition is null || compositionDataToDelete.Composition.ProductId != productId)
                {
                    throw new Exception("No composition available for this product");
                }

                _unitOfWork.CompositionData.Remove(compositionDataToDelete);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateCompositionData(
            IEnumerable<UpdateCompositionDataRequest>? compositionDataRequests,
            ICollection<CompositionData>? existingCompositionDataFromDb)
        {
            try
            {
                if (compositionDataRequests is not null && compositionDataRequests.Any())
                {
                    existingCompositionDataFromDb ??= new List<CompositionData>();
                    foreach (var compositionDataRequest in compositionDataRequests)
                    {
                        // Find existing price with the same ID in the database
                        CompositionData? existingCompositionData = existingCompositionDataFromDb?.FirstOrDefault(em => em.Id == compositionDataRequest.Id);
                        if (existingCompositionData != null)
                        {
                            _mapper.Map(compositionDataRequest, existingCompositionData);
                        }
                        else
                        {
                            CompositionData newCompositionData = _mapper.Map<CompositionData>(compositionDataRequest);
                            existingCompositionDataFromDb?.Add(newCompositionData);
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
