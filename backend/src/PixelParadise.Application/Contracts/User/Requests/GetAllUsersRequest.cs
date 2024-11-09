namespace PixelParadise.Application.Contracts.Requests;

public class GetAllUsersRequest : PagedRequest
{
    public string? Username { get; init; }
    public string? Nickname { get; init; }
    public string? Email { get; init; }
    public string? SortBy { get; init; }
}