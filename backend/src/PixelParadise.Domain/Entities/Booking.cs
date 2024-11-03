namespace PixelParadise.Domain.Entities;

public class Booking : BaseEntity
{
    public Booking(Guid rentalId, Rental rental, Guid userId, User user, DateTimeOffset checkIn,
        DateTimeOffset checkOut, decimal amountPaid, BookingStatus status = BookingStatus.Pending)
    {
        RentalId = rentalId;
        Rental = rental;
        UserId = userId;
        User = user;
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