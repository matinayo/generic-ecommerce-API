namespace HalceraAPI.Models.Requests.APIResponse
{
    public record APIResponse<T>
        where T : class
    {
        public APIResponse(T data, Meta? meta = null, string? message = null)
        {
            Data = data;
            Meta = meta;
            Message = message;
        }
        public T? Data { get; init; }
        public Meta? Meta { get; init; }
        public string? Message { get; set; }
    }
}
