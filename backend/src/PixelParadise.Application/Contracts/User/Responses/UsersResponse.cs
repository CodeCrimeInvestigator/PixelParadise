namespace PixelParadise.Application.Contracts.Responses;

public class UsersResponse
{
    public required IEnumerable<UserResponse> Users { get; init; } = [];
}