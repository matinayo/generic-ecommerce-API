using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Models.Requests.Composition.CompositionData
{
    /// <summary>
    /// Update composition data
    /// </summary>
    public class UpdateCompositionDataRequest
    {
        public int? Id { get; set; }

        [StringLength(20, ErrorMessage = "Data field has a maximum length of '20'")]
        public string? Data { get; set; }
    }
}
