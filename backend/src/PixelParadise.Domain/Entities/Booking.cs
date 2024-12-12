namespace PixelParadise.Domain.Entities;

public class Booking : BaseEntity
{
    public Booking(Guid accommodationId, Guid userId, DateTimeOffset checkIn,
        DateTimeOffset checkOut, decimal amountPaid, BookingStatus status = BookingStatus.Pending)
    {
        AccommodationId = accommodationId;
        UserId = userId;
        CheckIn = checkIn;
        CheckOut = checkOut;
        AmountPaid = amountPaid;
        Status = status;
    }

    public Booking()
    {
    }

    public Guid AccommodationId { get; set; }
    public Accommodation Accommodation { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }

    public DateTimeOffset CheckIn { get; set; }
    public DateTimeOffset CheckOut { get; set; }
    public decimal AmountPaid { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Pending;
}