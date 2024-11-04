namespace PixelParadise.Application.Contracts.Requests;

public class UpdateUserRequest
{
    public required string UserName { get; init; }
    public required string NickName { get; init; }
    public required string Email { get; init; }
    public required int Age { get; init; }
}