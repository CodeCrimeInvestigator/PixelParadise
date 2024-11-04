namespace PixelParadise.Domain.Options;

public class GetAllUsersOptions
{
    public string? Username { get; set; }
    public string? Nickname { get; set; }
    public string? Email { get; set; }
    public string? SortField { get; set; }
    public SortOrder SortOrder { get; set; }
}

public enum SortOrder
{
    Unsorted,
    Ascending,
    Descending
}