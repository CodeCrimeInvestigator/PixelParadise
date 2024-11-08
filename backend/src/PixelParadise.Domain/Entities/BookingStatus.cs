namespace PixelParadise.Domain.Entities;

public enum BookingStatus
{
    All,
    Pending,
    AwaitingPayment,
    Confirmed,
    Cancelled,
    Refunded 
}