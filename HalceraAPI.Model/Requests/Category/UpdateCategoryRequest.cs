using HalceraAPI.Models.Requests.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalceraAPI.Models.Requests.Category
{
    /// <summary>
    /// Update category
    /// </summary>
    public class UpdateCategoryRequest
    {
        [StringLength(20, ErrorMessage = "Field has a minimum length of '2' and maximum length of '10'", MinimumLength = 2)]
        public string? Title { get; set; }

        /// <summary>
        /// Category Medias
        /// </summary>
        public ICollection<UpdateMediaRequest>? MediaCollection { get; set; }

        public bool? IsActive { get; set; }
        public bool? IsFeatured { get; set; }
    }
}
