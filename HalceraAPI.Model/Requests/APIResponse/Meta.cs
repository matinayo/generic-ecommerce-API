namespace HalceraAPI.Models.Requests.APIResponse
{
    public record Meta(int Total, int PerPage, int Page)
    {
        public int PageCount { get; init; } = (int)Math.Ceiling((double)Total / PerPage);
    }
}
