using PixelParadise.Application.Contracts.Booking.Requests;
using PixelParadise.Application.Contracts.Booking.Responses;
using PixelParadise.Domain.Entities;
using PixelParadise.Domain.Options;

namespace PixelParadise.Application.Mapping;

public static class BookingMapping
{
    public static Booking MapToBooking(this CreateBookingRequest request)
    {
        return new Booking
        {
            Id = Guid.NewGuid(),
            RentalId = request.RentalId,
            UserId = request.UserId,
            CheckIn = request.CheckIn,
            CheckOut = request.CheckOut,
            AmountPaid = request.AmountPaid,
            Status = BookingStatus.Pending
        };
    }

    public static BookingResponse MapToResponse(this Booking booking)
    {
        return new BookingResponse
        {
            BookingId = booking.Id,
            RentalId = booking.RentalId,
            UserId = booking.UserId,
            CheckIn = booking.CheckIn,
            CheckOut = booking.CheckOut,
            AmountPaid = booking.AmountPaid,
            Status = booking.Status.ToString()
        };
    }

    public static GetAllBookingOptions MapToOptions(this GetAllBookingRequest request)
    {
        return new GetAllBookingOptions
        {
            RentalId = request.RentalId,
            UserId = request.UserId,
            CheckIn = request.CheckIn,
            CheckOut = request.CheckOut,
            Status = request.Status is null ? BookingStatus.All : Enum.Parse<BookingStatus>(request.Status),
            SortField = request.SortBy?.Trim('+', '-'),
            SortOrder = request.SortBy is null ? SortOrder.Unsorted :
                request.SortBy.StartsWith('-') ? SortOrder.Descending : SortOrder.Ascending
        };
    }
    
    public static BookingsResponse MapToResponse(this IEnumerable<Booking> bookings)
    {
        return new BookingsResponse
        {
            Bookings = bookings.Select(MapToResponse).ToList()
        };
    }
}