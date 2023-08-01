using HalceraAPI.Models.Enums;
using HalceraAPI.Models.Requests.Composition.CompositionData;
using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Models.Requests.Composition
{
    public class CreateCompositionRequest
    {
        /// <summary>
        /// Type of product composition
        /// </summary>
        [Required]
        public CompositionType? CompositionType { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "Field has a maximum length of '20'")]
        public string? Name { get; set; }
        public ICollection<CreateCompositionDataRequest>? CompositionDataCollection { get; set; }
    }
}
