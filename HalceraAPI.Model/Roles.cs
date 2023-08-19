using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Models
{
    public class Roles
    {
        [Key]
        public int Id { get; set; }
        [StringLength(20)]
        public string Title { get; set; } = string.Empty;
        public ICollection<ApplicationUser>? ApplicationUsers { get; set; }
    }
}
