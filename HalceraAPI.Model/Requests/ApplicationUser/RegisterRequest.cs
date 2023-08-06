using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Models.Requests.ApplicationUser
{
    /// <summary>
    /// User Register Request
    /// </summary>
    public class RegisterRequest
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string? ConfirmPassword { get; set; }
    }
}
