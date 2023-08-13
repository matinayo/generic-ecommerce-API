using HalceraAPI.Models.Requests.RefreshToken;
using HalceraAPI.Models.Requests.Role;

namespace HalceraAPI.Models.Requests.ApplicationUser
{
    /// <summary>
    /// User response
    /// </summary>
    public class UserResponse
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public bool? Active { get; set; }
        public DateTime? LockoutEnd { get; set; }
        public DateTime? UserCreatedDate { get; set; }
        public DateTime? DateLastModified { get; set; }
        public string? Token { get; set; }
        public RefreshTokenResponse? RefreshToken { get; set; }
        public ICollection<RoleResponse>? Roles { get; set; }
    }
}
