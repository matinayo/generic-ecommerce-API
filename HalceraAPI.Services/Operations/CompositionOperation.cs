using AutoMapper;
using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;
using HalceraAPI.Services.Contract;
using HalceraAPI.Services.Dtos.Composition;

namespace HalceraAPI.Services.Operations
{
    public class CompositionOperation : ICompositionOperation
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPriceOperation _priceOperation;
        private readonly IMediaOperation _mediaOperation;
        private readonly IProductSizeOperation _productSizeOperation;

        public CompositionOperation(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IProductSizeOperation productSizeOperation,
            IMediaOperation mediaOperation,
            IPriceOperation priceOperation)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _priceOperation = priceOperation;
            _mediaOperation = mediaOperation;
            _productSizeOperation = productSizeOperation;
        }

        public async Task DeleteCompositionFromProductByCompositionIdAsync(int productId, int compositionId)
        {
            try
            {
                Composition compositionToDelete = await _unitOfWork.Composition
                    .GetFirstOrDefault(
                    composition => composition.Id == compositionId
                    && composition.Id == productId)
                    ?? throw new Exception("No composition available for this product");

                //await _compositionDataOperation.DeleteCompositionDataCollectionAsync(new List<int>() { compositionToDelete.Id });

                _unitOfWork.Composition.Remove(compositionToDelete);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteProductCompositions(int productId)
        {
            try
            {
                IEnumerable<Composition>? productCompositions = await _unitOfWork.Composition.GetAll(
                    composition => composition.Id == productId);
                if (productCompositions is not null && productCompositions.Any())
                {
                    // delete product composition data
                    //await _compositionDataOperation.DeleteCompositionDataCollectionAsync(productCompositions.Select(comp => comp.Id));

                    _unitOfWork.Composition.RemoveRange(productCompositions);
                    await _unitOfWork.SaveAsync();
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateComposition(
            IEnumerable<UpdateCompositionRequest>? compositionCollection,
            ICollection<Composition>? existingCompositionsFromDb)
        {
            if (compositionCollection is not null && compositionCollection.Any())
            {
                existingCompositionsFromDb ??= new List<Composition>();
                foreach (var compositionRequest in compositionCollection)
                {
                    Composition? existingComposition = existingCompositionsFromDb?.FirstOrDefault(em => em.Id == compositionRequest.Id);

                    if (existingComposition != null)
                    {
                        _mapper.Map(compositionRequest, existingComposition);
                    }
                    else
                    {
                        existingComposition = _mapper.Map<Composition>(compositionRequest);

                        existingComposition.Sizes = new List<ProductSize>();
                        existingComposition.MediaCollection = new List<Media>();
                        existingComposition.Prices = new List<Price>();

                        existingCompositionsFromDb?.Add(existingComposition);
                    }

                    _productSizeOperation.UpdateProductSize(compositionRequest.Sizes, existingComposition.Sizes);
                    _mediaOperation.UpdateMediaCollection(compositionRequest.MediaCollection, existingComposition.MediaCollection);
                    _priceOperation.UpdatePrice(compositionRequest.Prices, existingComposition?.Prices);
                }
            }
        }
    }
}
