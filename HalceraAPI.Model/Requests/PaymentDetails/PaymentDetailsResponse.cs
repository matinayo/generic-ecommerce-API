using HalceraAPI.Models.Enums;

namespace HalceraAPI.Models.Requests.PaymentDetails
{
    public class PaymentDetailsResponse
    {
        public decimal AmountPaid { get; set; }
        public Currency? Currency { get; set; }
        public decimal? TotalAmount { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? PaymentDueDate { get; set; }
        public string? TransactionId { get; set; }
    }
}
