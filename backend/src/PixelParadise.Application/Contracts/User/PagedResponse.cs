namespace PixelParadise.Application.Contracts.User;

public class PagedResponse<TResponse>
{
    public IEnumerable<TResponse> Items { get; set; } = [];
    public int PageSize { get; set; }
    public int Page { get; set; }
    public int Total { get; set; }
    public bool HasPrevious => Page > 1;
    public bool HasNext => Total > Page * PageSize;
}