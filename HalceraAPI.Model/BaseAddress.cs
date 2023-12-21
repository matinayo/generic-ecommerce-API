using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Models
{
    /// <summary>
    /// User Base Addresses
    /// </summary>
    public class BaseAddress
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(80, ErrorMessage = "Field has a maximum length of '80'")]
        public string? StreetAddress { get; set; }
        [Required(ErrorMessage = $"The {nameof(City)} field is required")]
        [StringLength(40, ErrorMessage = "Field has a maximum length of '40'")]
        public string? City { get; set; }
        [StringLength(80, ErrorMessage = "Field has a maximum length of '80'")]
        public string? State { get; set; }
        public string? Country { get; set; }
        [StringLength(10, ErrorMessage = "Field has a maximum length of '10'")]
        public string? PostalCode { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
