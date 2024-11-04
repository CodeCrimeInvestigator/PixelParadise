using PixelParadise.Domain.Entities;

namespace PixelParadise.Application.Contracts.Responses;

public class UserResponse
{
    public Guid Id { get; init; }

    public string UserName { get; init; }

    public string NickName { get; init; }

    public string Email { get; init; }

    public int Age { get; init; }
}