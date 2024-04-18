using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Services.Dtos.Composition.MaterialData
{
    public class UpdateMaterialDataRequest
    {
        public int? Id { get; set; }

        [StringLength(20, ErrorMessage = "Data field has a maximum length of '20'")]
        public string? Data { get; set; }
    }
}
