namespace PixelParadise.Application.Contracts.Requests;

public class CreateUserRequest
{
    public string UserName { get; init; }

    public string NickName { get; init; }

    public string Email { get; init; }

    public int Age { get; init; }
}