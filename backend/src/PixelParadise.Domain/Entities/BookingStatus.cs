namespace PixelParadise.Domain.Entities;

public enum BookingStatus
{
    Pending,
    AwaitingPayment,
    Confirmed,
    Cancelled,
    Refunded 
}