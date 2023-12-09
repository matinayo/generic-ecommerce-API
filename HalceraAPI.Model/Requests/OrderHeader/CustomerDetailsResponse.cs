namespace HalceraAPI.Models.Requests.OrderHeader
{
    public record CustomerDetailsResponse
    {
        public string? Id { get; init; }
        public string? Name { get; init; }

        public string Email { get; init; } = string.Empty;

        public bool Active { get; init; } = true;

        public DateTime? LockoutEnd { get; init; }
        public DateTime? UserCreatedDate { get; init; }
    }
}
