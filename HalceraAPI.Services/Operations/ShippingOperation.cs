using AutoMapper;
using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;
using HalceraAPI.Models.Enums;
using HalceraAPI.Models.Requests.Shipping;
using HalceraAPI.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalceraAPI.Services.Operations
{
    public class ShippingOperation : IShippingOperation
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIdentityOperation _identityOperation;
        private readonly IMapper _mapper;

        public ShippingOperation(IUnitOfWork unitOfWork, IIdentityOperation identityOperation, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _identityOperation = identityOperation;
            _mapper = mapper;
        }

        public Task GetAllShippingRequests(ShippingStatus? shippingStatus)
        {
            throw new NotImplementedException();
        }

        public Task UpdateShippingDetailsAsync(int shippingId, UpdateShippingDetailsRequest shippingDetailsRequest)
        {
            throw new NotImplementedException();
        }
    }
}
