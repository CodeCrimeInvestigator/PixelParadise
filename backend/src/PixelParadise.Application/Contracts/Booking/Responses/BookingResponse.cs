namespace PixelParadise.Application.Contracts.Booking.Responses;

public class BookingResponse
{
    public Guid BookingId { get; set; }
    public Guid RentalId { get; set; }
    public Guid UserId { get; set; }
    public DateTimeOffset CheckIn { get; set; }
    public DateTimeOffset CheckOut { get; set; }
    public decimal AmountPaid { get; set; }
    public string Status { get; set; }
}