namespace PixelParadise.Domain.Entities;

public class BaseEntity 
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;
}