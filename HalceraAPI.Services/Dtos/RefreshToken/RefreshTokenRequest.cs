using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Services.Dtos.RefreshToken
{
    public class RefreshTokenRequest
    {
        [Required]
        public string Token { get; set; } = string.Empty;
    }
}
