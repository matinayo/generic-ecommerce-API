﻿using HalceraAPI.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalceraAPI.Models
{
    public class Media
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(80)]
        public string? URL { get; set; }

        [Required]
        public MediaType Type { get; set; }

        [StringLength(20)]
        public string? Name { get; set; }

        public int? ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }

        public int? CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public Category? Category { get; set; }
    }
}
