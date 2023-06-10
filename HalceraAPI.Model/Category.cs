using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalceraAPI.Models
{
    /// <summary>
    /// Product Category class
    /// </summary>
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "Field has a minimum length of '2' and maximum length of '10'", MinimumLength = 2)]
        public string? Title { get; set; }

        /// <summary>
        /// Products for this category
        /// </summary>
        public ICollection<Product>? Products { get; set; }

        /// <summary>
        /// Category Medias
        /// </summary>
        public ICollection<Media>? MediaCollection { get; set; }
    }
}
