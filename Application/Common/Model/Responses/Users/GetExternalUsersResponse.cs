using Application.DTOs.Users;

namespace Application.Common.Model;

public class GetExternalUsersResponse
{
    public List<UserDto> Users { get; set; }
}