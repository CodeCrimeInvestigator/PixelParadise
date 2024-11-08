namespace PixelParadise.Application.Contracts.Booking.Requests;

public class GetAllBookingRequest
{
    public Guid? RentalId { get; set; }
    public Guid? UserId { get; set; }
    public DateTimeOffset? CheckIn { get; set; }
    public DateTimeOffset? CheckOut { get; set; }
    public string? Status { get; set; }
    public string? SortBy { get; set; }
}