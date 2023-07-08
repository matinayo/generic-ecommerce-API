using HalceraAPI.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalceraAPI.Models.Requests.Media
{
    /// <summary>
    /// Update Media
    /// </summary>
    public class UpdateMediaRequest
    {
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// Content URL
        /// </summary>
        [StringLength(80)]
        public string? URL { get; set; }
        /// <summary>
        /// Content Type
        /// </summary>
        public MediaType Type { get; set; }
        /// <summary>
        /// Name or Caption of Media
        /// </summary>
        [StringLength(10)]
        public string? Name { get; set; }
    }
}
