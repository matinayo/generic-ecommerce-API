using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalceraAPI.Models
{
    /// <summary>
    /// Application User Models
    /// </summary>
    public class ApplicationUser
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [StringLength(256)]
        public string? Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public bool Active { get; set; } = true;
        /// <summary>
        /// End date if account is locked
        /// </summary>
        public DateTime? LockoutEnd { get; set; }
        public DateTime? UserCreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? DateLastModified { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public ICollection<Roles>? Roles { get; set; }
        public int? RefreshTokenId { get; set; }
        [ForeignKey(nameof(RefreshTokenId))]
        public RefreshToken? RefreshToken { get; set; }

        public int? AddressId { get; set; }
        [ForeignKey(nameof(AddressId))]
        public BaseAddress? Address { get; set; }
    }
}
