using HalceraAPI.Models;
using HalceraAPI.Services.Dtos.Category;
using HalceraAPI.Services.Dtos.Composition;
using HalceraAPI.Services.Dtos.Composition.ComponentData;
using HalceraAPI.Services.Dtos.Media;
using HalceraAPI.Services.Dtos.Price;

namespace HalceraAPI.Services.Dtos.Product
{
    public class ProductDetailsResponse
    {
        public int Id { get; set; }
        public string? Title { get; set; }

        public string? Description { get; set; }

        public bool Active { get; set; }

        public bool? Featured { get; set; }

        public ICollection<CompositionResponse>? Compositions { get; set; }

        public ICollection<ComponentDataResponse>? MaterialsAndDetails { get; set; }

        public ICollection<CategoryLabelResponse>? Categories { get; set; }
    }
}
