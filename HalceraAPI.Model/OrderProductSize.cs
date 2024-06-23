using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Models
{
    public class OrderProductSize
    {
        public int Id { get; set; }
        [Required]
        public string? Size { get; set; }
    }
}
