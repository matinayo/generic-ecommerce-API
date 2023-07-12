using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Models.Requests.Rating
{
    /// <summary>
    /// Rating Response
    /// </summary>
    public class RatingResponse
    {
        public int Id { get; set; }
        public double? Rate { get; set; }
        [StringLength(100, ErrorMessage = "Comment has a minimum length of '10' and maximum length of '100'", MinimumLength = 10)]
        public string? Comment { get; set; }
    }
}
