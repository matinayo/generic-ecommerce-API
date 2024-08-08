using HalceraAPI.Common.Enums;

namespace HalceraAPI.Services.Dtos.Payment
{
    public record VerifyPaymentRequest
    {
        public required string Reference { get; init; }
        public Currency Currency { get; init; }
        public PaymentProvider PaymentProvider { get; init; }
    }
}
