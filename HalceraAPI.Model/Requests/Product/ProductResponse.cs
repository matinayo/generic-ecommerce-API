﻿using HalceraAPI.Models.Requests.Category;
using HalceraAPI.Models.Requests.Composition;
using HalceraAPI.Models.Requests.Composition.CompositionData;
using HalceraAPI.Models.Requests.Media;
using HalceraAPI.Models.Requests.Price;
using HalceraAPI.Models.Requests.Rating;

namespace HalceraAPI.Models.Requests.Product
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public string? Title { get; set; }

        /// <summary>
        /// if product is active
        /// </summary>
        public bool Active { get; set; }
        /// <summary>
        /// Featured Product
        /// </summary>
        public bool? Featured { get; set; }

        public ICollection<PriceResponse>? Prices { get; set; }

        /// <summary>
        /// Product Medias
        /// </summary>
        public ICollection<MediaResponse>? MediaCollection { get; set; }

        /// <summary>
        /// Product Categories
        /// </summary>
        public ICollection<CategoryLabelResponse>? Categories { get; set; }
    }
}
