using HalceraAPI.Models.Enums;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HalceraAPI.Models.Requests.Media
{
    /// <summary>
    /// Create Media Request
    /// </summary>
    public class CreateMediaRequest
    {
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
