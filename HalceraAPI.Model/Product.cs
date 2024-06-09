using HalceraAPI.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "Field has a minimum length of '2' and maximum length of '20'",
            MinimumLength = 2)]
        public string? Title { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Field has a minimum length of 10 and maximum length of '100'", MinimumLength = 10)]
        public string? Description { get; set; }

        public bool Active { get; set; } = true;

        public bool? Featured { get; set; }

        public ICollection<Composition>? Compositions { get; set; }

        public ICollection<ComponentData>? ComponentDataCollection { get; set; }

        public ICollection<Category>? Categories { get; set; }

        public DateTime? DateAdded { get; set; } = DateTime.UtcNow;

        public DateTime? DateLastModified { get; set; }

        public void ValidateProductForCreate()
        {
            CheckCompositionDuplicateType();
            //CheckPriceDuplicateCurrency();
        }

        private void CheckCompositionDuplicateType()
        {
            //if (CompositionDataCollection is not null && CompositionDataCollection.Any())
            //{
            //    HashSet<CompositionType?> seenTypes = new();
            //    foreach (var composition in CompositionDataCollection)
            //    {
            //        if (!seenTypes.Add(composition.CompositionType))
            //        {
            //            throw new Exception($"Duplicate composition type: {composition.CompositionType?.ToString()} specified");
            //        }
            //    }
            //}
        }

        //private void CheckPriceDuplicateCurrency()
        //{
        //    if (Prices is not null && Prices.Any())
        //    {
        //        HashSet<Currency?> seenTypes = new();
        //        foreach (var price in Prices)
        //        {
        //            if (!seenTypes.Add(price.Currency))
        //            {
        //                throw new Exception($"Duplicate currency: {price.Currency?.ToString().ToUpper()} specified");
        //            }
        //        }
        //    }
        //}
    }
}
