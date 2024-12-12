namespace PixelParadise.Application.Contracts.Accommodation.Requests;

public class GetAllAccommodationsRequest : PagedRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? PriceLowerLimit { get; set; }
    public int? PriceUpperLimit { get; set; }
    public string? OwnerUsername { get; set; }
    public string? SortBy { get; set; }
}