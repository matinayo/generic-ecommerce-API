using HalceraAPI.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Services.Dtos.Media
{
    /// <summary>
    /// Media Response Object
    /// </summary>
    public class MediaResponse
    {
        public int Id { get; set; }
        /// <summary>
        /// Content URL
        /// </summary>
        [Required]
        [StringLength(80)]
        public string? URL { get; set; }
        /// <summary>
        /// Content Type
        /// </summary>
        [Required]
        public MediaType Type { get; set; }
        /// <summary>
        /// Name or Caption of Media
        /// </summary>
        [StringLength(10)]
        public string? Name { get; set; }
    }
}
