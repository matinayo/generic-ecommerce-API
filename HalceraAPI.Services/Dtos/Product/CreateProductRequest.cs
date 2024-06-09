using HalceraAPI.Models;
using HalceraAPI.Services.Dtos.Category;
using HalceraAPI.Services.Dtos.Composition;
using HalceraAPI.Services.Dtos.Composition.ComponentData;
using HalceraAPI.Services.Dtos.Media;
using HalceraAPI.Services.Dtos.Price;
using HalceraAPI.Services.Dtos.Rating;
using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Services.Dtos.Product
{
    public record CreateProductRequest
    {
        [Required]
        [StringLength(20, ErrorMessage = "Field has a minimum length of '2' and maximum length of '20'", MinimumLength = 2)]
        public string? Title { get; init; }

        [Required]
        [StringLength(100, ErrorMessage = "Field has a minimum length of 10 and maximum length of '100'", MinimumLength = 10)]
        public string? Description { get; init; }

        public bool Active { get; init; } = true;

        public bool? Featured { get; init; }

        public ICollection<CreateCompositionRequest>? Compositions { get; init; }
        
        public ICollection<CreateComponentDataRequest>? MaterialsAndDetails { get; set; }

        public ICollection<ProductCategoryRequest>? Categories { get; init; }
    }
}
