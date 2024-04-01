using HalceraAPI.Common.Enums;
using HalceraAPI.Services.Dtos.Composition.CompositionData;
using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Services.Dtos.Composition
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
