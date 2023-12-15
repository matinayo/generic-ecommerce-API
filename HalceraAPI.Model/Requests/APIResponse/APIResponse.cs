namespace HalceraAPI.Models.Requests.APIResponse
{
    public record APIResponse<T>
        where T : class
    {
        public APIResponse(T data, Meta? meta = null)
        {
            Data = data;
            Meta = meta;
        }
        public T? Data { get; init; }
        public Meta? Meta { get; init; }
    }
}
