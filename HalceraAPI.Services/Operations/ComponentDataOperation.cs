using AutoMapper;
using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;
using HalceraAPI.Services.Dtos.Composition.ComponentData;
using HalceraAPI.Services.Contract;

namespace HalceraAPI.Services.Operations
{
    public class ComponentDataOperation : IComponentDataOperation
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ComponentDataOperation(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task DeletecomponentDataCollectionAsync(IEnumerable<int> compositionIdCollection)
        {
            try
            {
                IEnumerable<ComponentData>? componentDataCollection = await _unitOfWork.ComponentData.GetAll(
                    componentData => componentData.Id != null
                    && compositionIdCollection.Contains(componentData.Id));

                if (componentDataCollection is not null && componentDataCollection.Any())
                {
                    _unitOfWork.ComponentData.RemoveRange(componentDataCollection);
                    await _unitOfWork.SaveAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeletecomponentDataFromProductCompositionAsync(int productId, int compositionId, int componentDataId)
        {
            try
            {
                ComponentData componentDataToDelete = await _unitOfWork.ComponentData
                    .GetFirstOrDefault(
                    componentData => componentData.Id == componentDataId
                    && componentData.Id == compositionId, includeProperties: nameof(ComponentData))
                    ?? throw new Exception("No composition data available for this composition");

                //if (componentDataToDelete.Composition is null || componentDataToDelete.Composition.ProductId != productId)
                //{
                //    throw new Exception("No composition available for this product");
                //}

                _unitOfWork.ComponentData.Remove(componentDataToDelete);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task DeleteCompositionDataCollectionAsync(IEnumerable<int> compositionIdCollection)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCompositionDataFromProductCompositionAsync(int productId, int compositionId, int compositionDataId)
        {
            throw new NotImplementedException();
        }

        public void UpdateComponentData(
            IEnumerable<UpdateComponentDataRequest>? componentDataRequests,
            ICollection<ComponentData>? existingcomponentDataFromDb)
        {
            try
            {
                if (componentDataRequests is not null && componentDataRequests.Any())
                {
                    existingcomponentDataFromDb ??= new List<ComponentData>();
                    foreach (var componentDataRequest in componentDataRequests)
                    {
                        // Find existing price with the same ID in the database
                        ComponentData? existingcomponentData = existingcomponentDataFromDb?.FirstOrDefault(em => em.Id == componentDataRequest.Id);
                        if (existingcomponentData != null)
                        {
                            _mapper.Map(componentDataRequest, existingcomponentData);
                        }
                        else
                        {
                            ComponentData newcomponentData = _mapper.Map<ComponentData>(componentDataRequest);
                            existingcomponentDataFromDb?.Add(newcomponentData);
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
