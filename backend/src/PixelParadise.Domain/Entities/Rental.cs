using System.ComponentModel.DataAnnotations.Schema;

namespace PixelParadise.Domain.Entities;

public class Rental : BaseEntity
{
    public Rental(string name, string description, int price, Guid ownerId)
    {
        Name = name;
        Description = description;
        Price = price;
        OwnerId = ownerId;
        Bookings = [];
    }

    public Rental()
    {
    }

    public string Name { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
    public Guid OwnerId { get; set; }
    [NotMapped] public User Owner { get; set; }
    public List<Booking> Bookings { get; set; } = [];
}