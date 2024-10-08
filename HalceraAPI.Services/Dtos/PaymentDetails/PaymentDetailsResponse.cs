﻿using HalceraAPI.Common.Enums;

namespace HalceraAPI.Services.Dtos.PaymentDetails
{
    public class PaymentDetailsResponse
    {
        public decimal AmountPaid { get; set; }
        public Currency? Currency { get; set; }
        public decimal? TotalAmountToBePaid { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? PaymentDueDate { get; set; }
        public string? TransactionId { get; set; }
        public string? Reference { get; set; }
        public PaymentProvider? PaymentProvider { get; set; }
        public string? Channel { get; set; }
    }
}
