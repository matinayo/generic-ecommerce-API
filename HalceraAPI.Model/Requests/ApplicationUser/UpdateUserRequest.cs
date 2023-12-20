using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Models.Requests.ApplicationUser
{
    public record UpdateUserRequest
    {
        [StringLength(256)]
        public string? Name { get; init; }

        [EmailAddress]
        [StringLength(256)]
        public string? Email { get; init; }
    }
}
