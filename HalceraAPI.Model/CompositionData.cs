﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalceraAPI.Models
{
    /// <summary>
    /// Composition Data
    /// </summary>
    public class CompositionData
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "Data field has a maximum length of '20'")]
        public string? Data { get; set; }

        public int? CompositionId { get; set; }
        [ForeignKey(nameof(CompositionId))]
        public Composition? Composition { get; set; }
    }
}
