using HalceraAPI.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "Field has a minimum length of '2' and maximum length of '20'",
            MinimumLength = 2)]
        public string? Title { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Field has a minimum length of 10 and maximum length of '100'", MinimumLength = 10)]
        public string? Description { get; set; }

        public bool Active { get; set; } = true;

        public bool? Featured { get; set; }

        public ICollection<Composition>? Compositions { get; set; }

        public ICollection<ComponentData>? ComponentDataCollection { get; set; }

        public ICollection<Category>? Categories { get; set; }

        public DateTime? DateAdded { get; set; } = DateTime.UtcNow;

        public DateTime? DateLastModified { get; set; }
    }
}
