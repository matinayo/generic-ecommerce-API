using HalceraAPI.Models.Requests.Media;
using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Models.Requests.Category
{
    /// <summary>
    /// Create category request
    /// </summary>
    public class CreateCategoryRequest
    {
        [Required]
        [StringLength(20, ErrorMessage = "Field has a minimum length of '2' and maximum length of '10'", MinimumLength = 2)]
        public string? Title { get; set; }

        /// <summary>
        /// Category media files
        /// </summary>
        public ICollection<CreateMediaRequest>? MediaCollection { get; set; }
    }
}
