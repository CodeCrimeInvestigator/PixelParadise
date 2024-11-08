using PixelParadise.Domain.Entities;

namespace PixelParadise.Domain.Options;

public class GetAllBookingOptions
{
    public Guid? RentalId { get; set; }
    public Guid? UserId { get; set; }
    public DateTimeOffset? CheckIn { get; set; }
    public DateTimeOffset? CheckOut { get; set; }
    public BookingStatus? Status { get; set; }
    public string? SortField { get; set; }
    public SortOrder? SortOrder { get; set; }
}