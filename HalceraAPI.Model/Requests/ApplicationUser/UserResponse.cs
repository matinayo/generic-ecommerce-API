using HalceraAPI.Models.Requests.BaseAddress;
using HalceraAPI.Models.Requests.Role;

namespace HalceraAPI.Models.Requests.ApplicationUser
{
    public record UserResponse
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public bool Active { get; set; }
        public DateTime? LockoutEnd { get; set; }
        public DateTime? UserCreatedDate { get; set; }
        public DateTime? DateLastModified { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public bool? AccountDeleted { get; set; }
        public DateTime? DateAccountDeleted { get; set; }
        public ICollection<RoleResponse>? Roles { get; set; }
        public AddressResponse? Address { get; set; }
    }
}
