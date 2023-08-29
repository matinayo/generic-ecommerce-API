﻿namespace HalceraAPI.Models.Enums
{
    public enum PaymentStatus : int
    {
        PaymentPending = 1,
        PaymentSucceeded = 2,
        PaymentFailed = 3,
        PaymentCanceled = 4,
        RefundRequested = 5,
        RefundSucceded = 6,
        RefundFailed = 7
    }
}
