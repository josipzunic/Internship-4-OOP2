using Application.DTOs.Users;

namespace Application.Common.Model;

public class GetAllUserResponse
{
    public IEnumerable<UserDto> Users { get; set; }
}