namespace PixelParadise.Domain.Entities;

public class User : BaseEntity
{
    public User(string userName, string nickName, string email, int age)
    {
        UserName = userName;
        NickName = nickName;
        Email = email;
        Age = age;
    }

    protected User() { }
    
    public string UserName { get; set; }
    public string NickName { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
    public List<Rental> Rentals { get; set; } = [];
    public List<Booking> Bookings { get; set; } = [];
}