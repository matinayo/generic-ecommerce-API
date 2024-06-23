using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Models
{
    public class OrderComposition
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Field has a maximum length of '20'")]
        public required string ColorName { get; set; }

        [Required]
        public required string ColorCode { get; set; }
    }
}
