using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Services.Dtos.Rating
{
    /// <summary>
    /// Create Rating Request
    /// </summary>
    public class CreateRatingRequest
    {
        [Required]
        [Range(1, 5)]
        public double? Rate { get; set; }
        [StringLength(100, ErrorMessage = "Comment has a minimum length of '10' and maximum length of '100'", MinimumLength = 10)]
        public string? Comment { get; set; }
    }
}
