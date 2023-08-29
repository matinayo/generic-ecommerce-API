using HalceraAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalceraAPI.Models
{
    /// <summary>
    /// Order Shipping Details
    /// </summary>
    public class ShippingDetails
    {
        [Key]
        public int Id { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Carrier { get; set; }
        public ShippingStatus? ShippingStatus { get; set; }
        public DateTime? ShippingDate { get; set; }
        public DateTime? DateShipped { get; set; }


        [Required]
        public int? AddressId { get; set; }
        [ForeignKey(nameof(AddressId))]
        public BaseAddress? ShippingAddress { get; set; }
    }
}
