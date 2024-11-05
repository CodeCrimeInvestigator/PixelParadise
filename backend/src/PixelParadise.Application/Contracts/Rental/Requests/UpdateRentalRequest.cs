namespace PixelParadise.Application.Contracts.Rental.Requests;

public class UpdateRentalRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
    public Guid OwnerId { get; set; }
}