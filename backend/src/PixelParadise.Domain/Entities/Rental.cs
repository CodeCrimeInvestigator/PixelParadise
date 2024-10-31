namespace PixelParadise.Domain.Entities;

public class Rental : BaseEntity
{
    public Rental(string name, string description, int price, User owner)
    {
        Name = name;
        Description = description;
        Price = price;
        Owner = owner;
        Bookings = [];
    }
    
    protected Rental() { }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
    public User Owner { get; set; }
    public List<Booking> Bookings { get; set; } = [];
}