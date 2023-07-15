using AutoMapper;
using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;
using HalceraAPI.Models.Requests.Media;
using HalceraAPI.Services.Contract;

namespace HalceraAPI.Services.Operations
{
    public class CompositionOperation : ICompositionOperation
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly ICompositionDataOperation _compositionDataOperation;

        public CompositionOperation(IUnitOfWork unitOfWork, IMapper mapper, ICompositionDataOperation compositionDataOperation)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _compositionDataOperation = compositionDataOperation;
        }

        public async Task<bool> DeleteProductCompositions(int productId)
        {
            try
            {
                IEnumerable<Composition>? productCompositions = await _unitOfWork.Composition.GetAll(composition => composition.ProductId == productId);
                if (productCompositions is not null && productCompositions.Any())
                {
                    // delete product composition data
                    await _compositionDataOperation.DeleteCompositionData(productCompositions.Select(comp => comp.Id));

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

        public Task<ICollection<Media>?> UpdateComposition(IEnumerable<UpdateMediaRequest>? mediaCollection)
        {
            throw new NotImplementedException();
        }
    }
}
