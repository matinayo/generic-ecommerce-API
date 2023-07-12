using HalceraAPI.Models.Enums;
using HalceraAPI.Models.Requests.Composition.CompositionData;

namespace HalceraAPI.Models.Requests.Composition
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
