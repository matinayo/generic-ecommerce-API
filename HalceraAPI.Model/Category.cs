using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalceraAPI.Model
{
    /// <summary>
    /// Product Category class
    /// </summary>
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ImageURL { get; set; }
        public string? VideoURL { get; set; }
    }
}
