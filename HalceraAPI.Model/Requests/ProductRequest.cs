using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalceraAPI.Model.Requests
{
    public class ProductRequest
    {
        [Required]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? ImageURL { get; set; }
        public string? GlbModelURL { get; set; }
        public string? Description { get; set; }
    }
}
