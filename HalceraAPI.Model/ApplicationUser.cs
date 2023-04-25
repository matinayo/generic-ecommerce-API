using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Model
{
    /// <summary>
    /// Application User Model
    /// </summary>
    public class ApplicationUser
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string? Name { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        public string? UserName { get; set; }
    }
}
