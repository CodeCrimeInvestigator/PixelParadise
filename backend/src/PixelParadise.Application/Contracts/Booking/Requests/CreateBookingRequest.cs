namespace PixelParadise.Application.Contracts.Booking.Requests;

public class CreateBookingRequest
{
    public Guid AccommodationId { get; set; }
    public Guid UserId { get; set; }
    public DateTimeOffset CheckIn { get; set; }
    public DateTimeOffset CheckOut { get; set; }
    public decimal AmountPaid { get; set; }
}