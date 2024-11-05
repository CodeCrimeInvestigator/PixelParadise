namespace PixelParadise.Application.Contracts.Rental.Responses;

public class RentalsResponse
{
    public required List<RentalResponse> Rentals { get; init; } = [];
}