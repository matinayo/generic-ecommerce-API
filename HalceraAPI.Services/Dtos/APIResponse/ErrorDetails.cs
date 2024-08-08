using System.Text.Json;

namespace HalceraAPI.Services.Dtos.APIResponse
{
    public record ErrorDetails(List<string> Errors, int StatusCode, string Message, bool Success = false)
    {
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
