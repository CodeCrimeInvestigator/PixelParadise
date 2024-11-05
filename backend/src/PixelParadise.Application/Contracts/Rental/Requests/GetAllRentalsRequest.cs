namespace PixelParadise.Application.Contracts.Rental.Requests;

public class GetAllRentalsRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? PriceLowerLimit { get; set; }
    public int? PriceUpperLimit { get; set; }
    public string? OwnerUsername { get; set; }
    public string? SortBy { get; init; }
}