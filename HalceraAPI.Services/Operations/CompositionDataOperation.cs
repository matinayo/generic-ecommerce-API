using AutoMapper;
using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;
using HalceraAPI.Services.Dtos.Composition.MaterialData;
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
                IEnumerable<MaterialData>? compositionDataCollection = await _unitOfWork.CompositionData.GetAll(
                    compositionData => compositionData.Id != null
                    && compositionIdCollection.Contains(compositionData.Id));

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
                MaterialData compositionDataToDelete = await _unitOfWork.CompositionData
                    .GetFirstOrDefault(
                    compositionData => compositionData.Id == compositionDataId
                    && compositionData.Id == compositionId, includeProperties: nameof(MaterialData))
                    ?? throw new Exception("No composition data available for this composition");

                //if (compositionDataToDelete.Composition is null || compositionDataToDelete.Composition.ProductId != productId)
                //{
                //    throw new Exception("No composition available for this product");
                //}

                _unitOfWork.CompositionData.Remove(compositionDataToDelete);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateCompositionData(
            IEnumerable<UpdateMaterialDataRequest>? compositionDataRequests,
            ICollection<MaterialData>? existingCompositionDataFromDb)
        {
            try
            {
                if (compositionDataRequests is not null && compositionDataRequests.Any())
                {
                    existingCompositionDataFromDb ??= new List<MaterialData>();
                    foreach (var compositionDataRequest in compositionDataRequests)
                    {
                        // Find existing price with the same ID in the database
                        MaterialData? existingCompositionData = existingCompositionDataFromDb?.FirstOrDefault(em => em.Id == compositionDataRequest.Id);
                        if (existingCompositionData != null)
                        {
                            _mapper.Map(compositionDataRequest, existingCompositionData);
                        }
                        else
                        {
                            MaterialData newCompositionData = _mapper.Map<MaterialData>(compositionDataRequest);
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
