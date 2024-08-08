using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalceraAPI.Services.Dtos.OrderHeader.OrderDetails
{
    public class OrderCompositionResponse
    {
        public string? ColorName { get; set; }

        public string? ColorCode { get; set; }
    }
}
