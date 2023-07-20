using HalceraAPI.Models.Enums;
using HalceraAPI.Models.Requests.Composition.CompositionData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalceraAPI.Models.Requests.Composition
{
    /// <summary>
    /// Update Composition Request
    /// </summary>
    public class UpdateCompositionRequest
    {
        public int? Id { get; set; }
        /// <summary>
        /// Type of product composition
        /// </summary>
        public CompositionType? CompositionType { get; set; }

        [StringLength(10, ErrorMessage = "Field has a maximum length of '10'")]
        public string? Name { get; set; }
        public ICollection<UpdateCompositionDataRequest>? CompositionDataCollection { get; set; }
    }
}
