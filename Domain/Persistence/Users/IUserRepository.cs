using System.Threading.Tasks;
using Domain.Persistence.Common;

namespace Domain.Persistence.User;
using Domain.Entities.Users;

public interface IUserRepository : IRepository<User, int>
{
    Task<User> GetById(int id);
    Task<User> UsernameExistsAsync(string username);
    Task<User> EmailExistsAsync(string email);
    Task<List<User>> GetAllActiveUsersAsync();

}