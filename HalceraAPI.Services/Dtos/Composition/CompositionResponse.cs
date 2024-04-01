using HalceraAPI.Common.Enums;
using HalceraAPI.Services.Dtos.Composition.CompositionData;

namespace HalceraAPI.Services.Dtos.Composition
{
    /// <summary>
    /// Composition Response
    /// </summary>
    public class CompositionResponse
    {
        public int Id { get; set; }
        /// <summary>
        /// Type of product composition
        /// </summary>
        public CompositionType? CompositionType { get; set; }
        public string? Name { get; set; }
        public ICollection<CompositionDataResponse>? CompositionDataCollection { get; set; }
    }
}
