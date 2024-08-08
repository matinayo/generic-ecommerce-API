using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Models
{
    public class OrderProductSize
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Size { get; set; }
        public int Quantity { get; set; }
    }
}
