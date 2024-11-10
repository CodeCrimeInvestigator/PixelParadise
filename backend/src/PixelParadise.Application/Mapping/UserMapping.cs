using PixelParadise.Application.Contracts.Requests;
using PixelParadise.Application.Contracts.Responses;
using PixelParadise.Domain.Entities;
using PixelParadise.Domain.Options;
using PixelParadise.Infrastructure.Repositories.Results;

namespace PixelParadise.Application.Mapping;

public static class UserMapping
{
    public static User MapToUser(this CreateUserRequest request)
    {
        return new User(request.UserName, request.NickName, request.Email, request.Age);
    }

    public static User MapToUser(this UpdateUserRequest request, Guid id)
    {
        return new User
        {
            Id = id,
            Username = request.UserName,
            Nickname = request.NickName,
            Email = request.Email,
            Age = request.Age
        };
    }

    public static UserResponse MapToResponse(this User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            UserName = user.Username,
            NickName = user.Nickname,
            Email = user.Email,
            Age = user.Age
        };
    }


    public static UsersResponse MapToResponse(this PaginatedResult<User> users)
    {
        return new UsersResponse
        {
            Items = users.Items.Select(MapToResponse).ToList(),
            Page = users.Page,
            PageSize = users.PageSize,
            Total = users.TotalCount
        };
    }

    public static GetAllUsersOptions MapToOptions(this GetAllUsersRequest request)
    {
        return new GetAllUsersOptions
        {
            Username = request.Username,
            Nickname = request.Nickname,
            Email = request.Email,
            SortField = request.SortBy?.Trim('+', '-'),
            SortOrder = request.SortBy is null ? SortOrder.Unsorted :
                request.SortBy.StartsWith('-') ? SortOrder.Descending : SortOrder.Ascending,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}