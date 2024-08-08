using HalceraAPI.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalceraAPI.Services.Dtos.Payment
{
    public record VerifyPaymentResponse
    {
        public string? TransactionId { get; set; }
        public string? Reference { get; set; }
        public PaymentProvider? PaymentProvider { get; set; }
        public string? Channel { get; set; }
        public decimal AmountPaid { get; set; }
        public Currency Currency { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? PaymentDueDate { get; set; }
    }
}
