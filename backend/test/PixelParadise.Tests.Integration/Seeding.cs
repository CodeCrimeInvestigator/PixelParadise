using PixelParadise.Domain.Entities;
using PixelParadise.Infrastructure;

namespace PixelParadise.Tests.Integration;

internal class Seeding
{
    public static async Task InitialiseTestDB(PixelParadiseContext db)
    {
        db.Users.AddRange(GetUsers());
        await db.SaveChangesAsync();  // Ensure to await async calls
    }

    private static IEnumerable<User> GetUsers()
    {
        return new List<User>
        {
            new User("usr1", "nick1", "user1@gmail.com", 22) {Id = Guid.Parse("eedca410-4304-4020-a175-ccea232fd8ca"), ProfileImageUrl = "/Users/usr1"},
            new User("usr2", "nick2", "user2@gmail.com", 23) {Id = Guid.Parse("cc9b35f8-23ad-4dc9-bcfa-46b5b8ceafb6"), ProfileImageUrl = "/Users/usr2"},
            new User("usr3", "nick3", "user3@gmail.com", 24) {Id = Guid.Parse("dad6d9dc-5a46-4fd5-a7ab-aa969480d47f"), ProfileImageUrl = "/Users/usr3"}
        };
    }
}