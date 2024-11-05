using PixelParadise.Domain.Entities;

namespace PixelParadise.Application.Contracts.Rental.Responses;

public class RentalResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
    public Guid OwnerId {get;set;} 
}