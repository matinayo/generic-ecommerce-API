using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Models.Requests.Composition.CompositionData
{
    /// <summary>
    /// Create Composition Data Request
    /// </summary>
    public class CreateCompositionDataRequest
    {
        [Required]
        [StringLength(20, ErrorMessage = "Data field has a maximum length of '20'")]
        public string? Data { get; set; }
    }
}