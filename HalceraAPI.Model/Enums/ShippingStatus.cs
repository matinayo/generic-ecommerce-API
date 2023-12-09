namespace HalceraAPI.Models.Enums
{
    /// <summary>
    /// Order Shipping Status
    /// </summary>
    public enum ShippingStatus : int
    {
        Pending = 1,
        Approved = 2,
        Shipped = 3,
        Completed = 4,
        Redelivery = 5,
        Cancelled = 6
    }
}
