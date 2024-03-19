using HalceraAPI.Models.Enums;

namespace HalceraAPI.Models.Requests.Payment
{
    public record VerifyPaymentRequest
    {
        public required string Reference { get; init; }
        public Currency Currency { get; init; }
        public PaymentProvider PaymentProvider { get; init; }
    }
}
