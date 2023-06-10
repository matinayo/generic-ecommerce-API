using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalceraAPI.Models
{
    /// <summary>
    /// Product Composition
    /// </summary>
    public class Composition
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(10, ErrorMessage = "Field has a maximum length of '10'")]
        public string? Name { get; set; }
        public ICollection<CompositionData>? CompositionDataCollection { get; set; }
    }
}
