using Application.DTOs.Users;

namespace Infrastructure.Persistence;

public interface IRetrieveExternalUsers
{
    Task<List<UserDto>> RetrieveExternalUsersAsync();
}