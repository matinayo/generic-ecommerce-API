using HalceraAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalceraAPI.Models
{
    /// <summary>
    /// Order and User data
    /// </summary>
    public class OrderHeader
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public OrderStatus OrderStatus { get; set; }

        public DateTime? OrderDate { get; set; } = DateTime.UtcNow;
        public DateTime? CancelledDate { get; set; }

        public int? PaymentDetailId { get; set; }
        [ForeignKey(nameof(PaymentDetailId))]
        public PaymentDetails? PaymentDetails { get; set; }

        public int? ShippingDetailsId { get; set; }
        [ForeignKey(nameof(ShippingDetailsId))]
        public ShippingDetails? ShippingDetails { get; set; }

        /// <summary>
        /// Corresponding order details
        /// </summary>
        public ICollection<OrderDetails>? OrderDetails { get; set; }

        [Required]
        public string? ApplicationUserId { get; set; }
        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser? ApplicationUser { get; set; }

        public void CancelOrder()
        {
            if (OrderIsEligbileForCancellation())
            {
                OrderStatus = OrderStatus.Cancelled;
                CancelledDate = DateTime.UtcNow;
                if(ShippingDetails != null)
                {
                    ShippingDetails.ShippingStatus = ShippingStatus.Cancelled;
                    ShippingDetails.CancelledDate = DateTime.UtcNow;
                }
            }
        }

        public void ShipOrder()
        {
            OrderStatus = OrderStatus.Shipped;
            if (ShippingDetails != null)
            {
                ShippingDetails.ShippingStatus = ShippingStatus.Shipped;
                ShippingDetails.ShippingDate = DateTime.UtcNow;
                ShippingDetails.CancelledDate = null;
            }
        }

        private bool OrderIsEligbileForCancellation()
        {
            if (OrderStatus == OrderStatus.Cancelled || OrderStatus == OrderStatus.Completed || OrderStatus == OrderStatus.Shipped)
            {
                throw new Exception($"This order has already been {OrderStatus.ToString().ToLower()}");
            }

            if (ShippingDetails != null)
            {
                if (ShippingDetails.ShippingStatus == ShippingStatus.Shipped ||
                    ShippingDetails.ShippingStatus == ShippingStatus.Completed ||
                    ShippingDetails.ShippingStatus == ShippingStatus.Cancelled)
                {
                    throw new Exception($"This order has already been {ShippingDetails?.ShippingStatus?.ToString().ToLower()}");
                }
            }

            return true;
        }

        public bool OrderIdEquals(string orderId)
        {
            return orderId.ToLower().Equals(Id.ToLower());
        }
    }

}
