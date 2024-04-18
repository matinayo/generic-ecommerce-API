using HalceraAPI.Common.Enums;
using HalceraAPI.Services.Dtos.Composition.MaterialData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalceraAPI.Services.Dtos.Composition
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

        [StringLength(20, ErrorMessage = "Field has a maximum length of '20'")]
        public string? Name { get; set; }
        public ICollection<UpdateMaterialDataRequest>? CompositionDataCollection { get; set; }
    }
}
