using Microsoft.Extensions.Options;
using PixelParadise.Application.Contracts.Requests;
using PixelParadise.Application.Contracts.Responses;
using PixelParadise.Application.Options;
using PixelParadise.Domain.Entities;
using PixelParadise.Domain.Options;
using PixelParadise.Infrastructure.Repositories.Results;

namespace PixelParadise.Application.Mapping;


public interface IUserMapper
{
    User MapToUser(CreateUserRequest request, IOptions<StorageOptions> options);
    User MapToUser(UpdateUserRequest request, Guid id);
    UserResponse MapToResponse(User user);
    UsersResponse MapToResponse(PaginatedResult<User> users);
    GetAllUsersOptions MapToOptions(GetAllUsersRequest request);
}


public class UserMapper : IUserMapper
{
    public User MapToUser(CreateUserRequest request, IOptions<StorageOptions> options)
    {
        return new User(request.UserName, request.NickName, request.Email, request.Age)
        {
            ProfileImageUrl = options.Value.RelDefaultUserImagePath
        };
    }

    public User MapToUser(UpdateUserRequest request, Guid id)
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
    
    public UserResponse MapToResponse(User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            UserName = user.Username,
            NickName = user.Nickname,
            Email = user.Email,
            Age = user.Age,
            ProfileImageUrl = user.ProfileImageUrl
        };
    }
    
    public UsersResponse MapToResponse(PaginatedResult<User> users)
    {
        return new UsersResponse
        {
            Items = users.Items.Select(MapToResponse).ToList(),
            Page = users.Page,
            PageSize = users.PageSize,
            Total = users.TotalCount
        };
    }

    public GetAllUsersOptions MapToOptions(GetAllUsersRequest request)
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