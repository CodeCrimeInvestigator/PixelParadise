namespace PixelParadise.Domain.Entities;

public class User : BaseEntity
{
    public User(string username, string nickname, string email, int age)
    {
        Username = username;
        Nickname = nickname;
        Email = email;
        Age = age;
    }

    public User()
    {
    }

    public string Username { get; set; }
    public string Nickname { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
    public string ProfileImageUrl { get; set; }
    public List<Rental> Rentals { get; set; } = [];
    public List<Booking> Bookings { get; set; } = [];
}