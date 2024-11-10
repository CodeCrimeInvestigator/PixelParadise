namespace PixelParadise.Application.Contracts.User;

/// <summary>
///     Represents a paginated response for a collection of items, including metadata for pagination.
/// </summary>
/// <typeparam name="T">The type of items in the paginated response.</typeparam>
public class PagedResponse<T>
{
    public List<T> Items { get; set; } = [];
    public int PageSize { get; set; }
    public int Page { get; set; }
    public int Total { get; set; }
    public bool HasPrevious => Page > 1;
    public bool HasNext => Total > Page * PageSize;
}