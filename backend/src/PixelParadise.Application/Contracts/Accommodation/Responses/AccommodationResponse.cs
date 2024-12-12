namespace PixelParadise.Application.Contracts.Accommodation.Responses;

public class AccommodationResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
    public Guid OwnerId { get; set; }
    public string CoverImageUrl { get; set; }
    public List<string> Images { get; set; }
}