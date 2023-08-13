using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Models.Requests.RefreshToken
{
    public class RefreshTokenRequest
    {
        [Required]
        public string Token { get; set; } = string.Empty;
    }
}
