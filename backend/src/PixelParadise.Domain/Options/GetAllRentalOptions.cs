namespace PixelParadise.Domain.Options;

public class GetAllRentalOptions
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? PriceLowerLimit { get; set; }
    public int? PriceUpperLimit { get; set; }
    public string? OwnerUsername { get; set; }
    public string? SortField { get; set; }
    public SortOrder SortOrder { get; set; }
}