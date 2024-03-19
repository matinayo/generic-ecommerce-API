namespace HalceraAPI.Models.Requests.Payment
{
    public record InitializePaymentResponse
    {
        public string? AuthorizationUrl { get; init; }
        public string? AccessCode { get; set; }
        public string? Reference { get; set; }
    }
}