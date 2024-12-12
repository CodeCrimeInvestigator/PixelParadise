using Microsoft.EntityFrameworkCore;
using PixelParadise.Domain.Entities;

namespace PixelParadise.Infrastructure;

public class PixelParadiseContext(DbContextOptions<PixelParadiseContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Accommodation> Accommodations => Set<Accommodation>();
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

        modelBuilder.Entity<Accommodation>()
            .HasOne(r => r.Owner)
            .WithMany(u => u.Accommodations)
            .HasForeignKey(r => r.OwnerId)
            .IsRequired();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.User)
            .WithMany(u => u.Bookings)
            .HasForeignKey(r => r.UserId)
            .IsRequired();

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Accommodation)
            .WithMany(r => r.Bookings)
            .HasForeignKey(r => r.AccommodationId)
            .IsRequired();

        base.OnModelCreating(modelBuilder);
    }
}