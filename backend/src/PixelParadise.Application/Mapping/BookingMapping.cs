using PixelParadise.Application.Contracts.Booking.Requests;
using PixelParadise.Application.Contracts.Booking.Responses;
using PixelParadise.Domain.Entities;
using PixelParadise.Domain.Options;
using PixelParadise.Infrastructure.Repositories.Results;

namespace PixelParadise.Application.Mapping;

public static class BookingMapping
{
    public static Booking MapToBooking(this CreateBookingRequest request)
    {
        return new Booking
        {
            Id = Guid.NewGuid(),
            AccommodationId = request.AccommodationId,
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
            AccommodationId = booking.AccommodationId,
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
            AccommodationId = request.AccommodationId,
            UserId = request.UserId,
            CheckIn = request.CheckIn,
            CheckOut = request.CheckOut,
            Status = request.Status is null ? BookingStatus.All : Enum.Parse<BookingStatus>(request.Status),
            SortField = request.SortBy?.Trim('+', '-'),
            SortOrder = request.SortBy is null ? SortOrder.Unsorted :
                request.SortBy.StartsWith('-') ? SortOrder.Descending : SortOrder.Ascending,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }

    public static BookingsResponse MapToResponse(this PaginatedResult<Booking> bookings)
    {
        return new BookingsResponse
        {
            Items = bookings.Items.Select(MapToResponse).ToList(),
            Page = bookings.Page,
            PageSize = bookings.PageSize,
            Total = bookings.TotalCount
        };
    }
}