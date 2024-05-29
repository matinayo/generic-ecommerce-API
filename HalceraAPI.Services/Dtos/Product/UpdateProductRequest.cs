using HalceraAPI.Services.Dtos.Category;
using HalceraAPI.Services.Dtos.Composition;
using HalceraAPI.Services.Dtos.Composition.MaterialData;
using HalceraAPI.Services.Dtos.Media;
using HalceraAPI.Services.Dtos.Price;
using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Services.Dtos.Product
{
    public class UpdateProductRequest
    {
        [StringLength(20, ErrorMessage = "Field has a minimum length of '2' and maximum length of '20'", MinimumLength = 2)]
        public string? Title { get; set; }

        [StringLength(100, ErrorMessage = "Field has a minimum length of 10 and maximum length of '100'", MinimumLength = 10)]
        public string? Description { get; set; }

        public bool? Active { get; set; }

        public bool? Featured { get; set; }

    }
}
