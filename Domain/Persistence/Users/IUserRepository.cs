using System.Threading.Tasks;
using Domain.Persistence.Common;

namespace Domain.Persistence.User;
using Domain.Entities.Users;
using System.Collections.Generic;
public interface IUserRepository : IRepository<User, int>
{
    Task<User> GetById(int id);
    Task<User> UsernameExistsAsync(string username);
    Task<User> EmailExistsAsync(string email);
    Task<List<User>> GetAllActiveUsersAsync();
    Task<List<User>> GetAllUsersAsync();
    Task<User> GetByUsername(string username);

}