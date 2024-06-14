using AutoMapper;
using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;
using HalceraAPI.Services.Contract;
using HalceraAPI.Services.Dtos.Composition;

namespace HalceraAPI.Services.Operations
{
    public class ProductSizeOperation : IProductSizeOperation
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductSizeOperation(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task DeleteSizeByListOfCompositionIdAsync(List<int> compositionIds)
        {
            IEnumerable<ProductSize>? sizeCollection = await _unitOfWork.ProductSize.GetAll(
                                                        size => size.CompositionId != null
                                                        && compositionIds.Contains(size.CompositionId ?? 0));

            if (sizeCollection is not null && sizeCollection.Any())
            {
                _unitOfWork.ProductSize.RemoveRange(sizeCollection);
                //await _unitOfWork.SaveAsync();
            }
        }

        public void UpdateProductSize(
            IEnumerable<UpdateProductSizeRequest>? productSizeRequests,
            ICollection<ProductSize>? existingProductSizesFromDb)
        {
            if (productSizeRequests is not null && productSizeRequests.Any())
            {
                existingProductSizesFromDb ??= new List<ProductSize>();
                foreach (var productSizeRequest in productSizeRequests)
                {
                    ProductSize? existingProductSize = existingProductSizesFromDb?.FirstOrDefault(em => em.Id == productSizeRequest.Id);
                    if (existingProductSize != null)
                    {
                        _mapper.Map(productSizeRequest, existingProductSize);
                    }
                    else
                    {
                        var newProductSize = _mapper.Map<ProductSize>(productSizeRequest);
                        existingProductSizesFromDb?.Add(newProductSize);
                    }
                }
            }
        }
    }
}
