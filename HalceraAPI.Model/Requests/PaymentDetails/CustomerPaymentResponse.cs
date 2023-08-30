using HalceraAPI.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalceraAPI.Models.Requests.PaymentDetails
{
    /// <summary>
    /// Customer Payment Response
    /// </summary>
    public class CustomerPaymentResponse
    {
        public decimal AmountPaid { get; set; }
        public Currency? Currency { get; set; }
    }
}
