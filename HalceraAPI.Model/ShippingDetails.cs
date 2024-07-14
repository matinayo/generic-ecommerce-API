using Enums = HalceraAPI.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalceraAPI.Models
{
    public class ShippingDetails
    {
        [Key]
        public int Id { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Carrier { get; set; }
        public Enums.ShippingStatus? ShippingStatus { get; set; } = Enums.ShippingStatus.Pending;
        public DateTime? ShippingDate { get; set; }
        public DateTime? DateShipped { get; set; }
        public DateTime? CancelledDate { get; set; }
        public int? AddressId { get; set; }
        [ForeignKey(nameof(AddressId))]
        public BaseAddress? ShippingAddress { get; set; }

        public bool CanUpdateShippingAddress()
        {
            if(ShippingStatus == Enums.ShippingStatus.Shipped || ShippingStatus == Enums.ShippingStatus.Completed)
            {
                throw new Exception($"Cannot update shipping address because order has already been {ShippingStatus?.ToString().ToLower()}");
            }

            return true;
        }
    }
}
