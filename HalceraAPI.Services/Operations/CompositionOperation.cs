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
        
        public async Task DeleteProductCompositions(int productId)
        {
            IEnumerable<Composition>? productCompositions = await _unitOfWork.Composition.GetAll(
                composition => composition.ProductId == productId);

            if (productCompositions is null || !productCompositions.Any())
            {
                return;
            }

            List<int> compositionIds = productCompositions.Select(u => u.Id).ToList();
            await _priceOperation.DeletePricesByListOfCompositionIdAsync(compositionIds);
            await _mediaOperation.DeleteMediaByListOfCompositionIdAsync(compositionIds);
            await _productSizeOperation.DeleteSizeByListOfCompositionIdAsync(compositionIds);

            _unitOfWork.Composition.RemoveRange(productCompositions);
        }

        public async Task DeleteCompositionFromProductByCompositionIdAsync(int productId, int compositionId)
        {
            Composition compositionToDelete = await _unitOfWork.Composition
                .GetFirstOrDefault(
                composition => composition.Id == compositionId
                && composition.Id == productId)
                ?? throw new Exception("No composition available for this product");

            var compositionIds = new List<int>() { compositionToDelete.Id };

            await _priceOperation.DeletePricesByListOfCompositionIdAsync(compositionIds);
            await _mediaOperation.DeleteMediaByListOfCompositionIdAsync(compositionIds);
            await _productSizeOperation.DeleteSizeByListOfCompositionIdAsync(compositionIds);

            _unitOfWork.Composition.Remove(compositionToDelete);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteMediaFromCompositionByMediaIdAsync(int compositionId, int mediaId)
        {
            await _mediaOperation.DeleteMediaFromCompositionByMediaIdAsync(compositionId, mediaId);
        }

        public async Task DeletePriceFromCompositionByPriceIdAsync(int compositionId, int priceId)
        {
            await _priceOperation.DeletePriceFromCompositionByPriceIdAsync(compositionId, priceId);
        }

        public async Task DeleteSizeFromCompositionBySizeIdAsync(int compositionId, int sizeId)
        {
            await _productSizeOperation.DeleteSizeFromCompositionBySizeIdAsync(compositionId, sizeId);
        }
    }
}
