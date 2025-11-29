

using Domain.Entities.Users;
using Domain.Persistence.User;
using Infrastructure.Database.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : Repository<User, int>, IUserRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IDapperManager _dapperManager;
    
    public UserRepository(DbContext dbContext, IDapperManager dapperManager) : base(dbContext)
    {
        _dapperManager = dapperManager;
    }

    public async Task<User> GetById(int id)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User> UsernameExistsAsync(string username)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User> EmailExistsAsync(string email)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<List<User>> GetAllActiveUsersAsync()
    {
        return await _dbContext.Users.Where(u => u.IsActive).ToListAsync();
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _dbContext.Users.ToListAsync();
    }
}