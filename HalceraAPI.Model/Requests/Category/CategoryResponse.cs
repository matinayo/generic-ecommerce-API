using HalceraAPI.Models.Requests.Media;

namespace HalceraAPI.Models.Requests.Category
{
    /// <summary>
    /// Category Response object
    /// </summary>
    public class CategoryResponse
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        /// <summary>
        /// Category Medias
        /// </summary>
        public ICollection<MediaResponse>? MediaCollection { get; set; }

        public bool? IsActive { get; set; }
        public bool? IsFeatured { get; set; }
    }
}
