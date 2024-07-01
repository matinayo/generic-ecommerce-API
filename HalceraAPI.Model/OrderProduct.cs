using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Models
{
    public class OrderProduct
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
    }
}
