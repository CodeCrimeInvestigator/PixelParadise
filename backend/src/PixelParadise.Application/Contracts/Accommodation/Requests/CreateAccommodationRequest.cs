namespace PixelParadise.Application.Contracts.Accommodation.Requests;

public class CreateAccommodationRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
    public Guid OwnerId { get; set; }
}