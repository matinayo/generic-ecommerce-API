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

        public async Task<bool> DeleteCompositionData(IEnumerable<int> compositionIdCollection)
        {
            try
            {
                IEnumerable<CompositionData>? compositionDataCollection = await _unitOfWork.CompositionData.GetAll(compositionData => compositionData.CompositionId != null
                && compositionIdCollection.Contains(compositionData.CompositionId ?? 0));
                if (compositionDataCollection is not null && compositionDataCollection.Any())
                {
                    _unitOfWork.CompositionData.RemoveRange(compositionDataCollection);
                    await _unitOfWork.SaveAsync();
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateCompositionData(IEnumerable<UpdateCompositionDataRequest>? compositionDataRequests, ICollection<CompositionData>? existingCompositionDataFromDb)
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
