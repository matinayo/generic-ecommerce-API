using HalceraAPI.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Services.Dtos.Composition.MaterialData
{
    public class CreateMaterialDataRequest
    {
        public CompositionType Type { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Data field has a maximum length of '20'")]
        public string? Data { get; set; }
    }
}