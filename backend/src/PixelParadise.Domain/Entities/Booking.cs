namespace PixelParadise.Domain.Entities;

public class Booking : BaseEntity
{
    public Booking(Guid rentalId, Guid userId, DateTimeOffset checkIn,
        DateTimeOffset checkOut, decimal amountPaid, BookingStatus status = BookingStatus.Pending)
    {
        RentalId = rentalId;
        UserId = userId;
        CheckIn = checkIn;
        CheckOut = checkOut;
        AmountPaid = amountPaid;
        Status = status;
    }

    public Booking()
    {
    }

    public Guid RentalId { get; set; }
    public Rental Rental { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }

    public DateTimeOffset CheckIn { get; set; }
    public DateTimeOffset CheckOut { get; set; }
    public decimal AmountPaid { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Pending;
}