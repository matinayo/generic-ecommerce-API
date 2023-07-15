using AutoMapper;
using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;
using HalceraAPI.Models.Requests.Media;
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

        public Task<ICollection<Media>?> UpdateCompositionData(IEnumerable<UpdateMediaRequest>? mediaCollection)
        {
            throw new NotImplementedException();
        }
    }
}
