using Microsoft.EntityFrameworkCore;
using PixelParadise.Domain.Entities;

namespace PixelParadise.Infrastructure;

public class PixelParadiseContext(DbContextOptions<PixelParadiseContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Rental> Rentals => Set<Rental>();
    public DbSet<Booking> Bookings => Set<Booking>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // initialise Booking 
        modelBuilder
            .Entity<Booking>()
            .Property(b => b.Status)
            .HasConversion(
                b => b.ToString(),
                b => Enum.Parse<BookingStatus>(b));
        
        base.OnModelCreating(modelBuilder);
    }
}