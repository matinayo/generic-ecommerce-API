namespace HalceraAPI.Models.Requests.RefreshToken
{
    public class RefreshTokenResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime DateExpires { get; set; }
    }
}
