using System.ComponentModel.DataAnnotations.Schema;

namespace PixelParadise.Domain.Entities;

public class Accommodation : BaseEntity
{
    public Accommodation(string name, string description, int price, Guid ownerId)
    {
        Name = name;
        Description = description;
        Price = price;
        OwnerId = ownerId;
        Bookings = [];
    }

    public Accommodation()
    {
    }

    public string Name { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
    public Guid OwnerId { get; set; }
    [NotMapped] public User Owner { get; set; }
    public List<Booking> Bookings { get; set; } = [];
    public string CoverImage { get; set; }
    public List<string> Images { get; set; } = [];
}