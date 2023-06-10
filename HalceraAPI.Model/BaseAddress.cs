using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Required]
        [StringLength(40, ErrorMessage = "Field has a maximum length of '40'")]
        public string? City { get; set; }
        [StringLength(80, ErrorMessage = "Field has a maximum length of '80'")]
        public string? State { get; set; }
        [StringLength(10, ErrorMessage = "Field has a maximum length of '10'")]
        public string? PostalCode { get; set; }

        [Required]
        public string? ApplicationUserId { get; set; }
        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
