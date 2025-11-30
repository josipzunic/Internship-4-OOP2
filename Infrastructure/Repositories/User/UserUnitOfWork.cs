using Domain.Persistence.User;
using Infrastructure.Database.Configurations;

namespace Infrastructure.Repositories;

public class UserUnitOfWork : IUserUnitOfWork
{
    private readonly UsersDbContext _dbContext;
    public IUserRepository Repository { get; set; }
    
    public UserUnitOfWork(UsersDbContext dbContext,  IUserRepository repository)
    {
        _dbContext = dbContext;
        Repository = repository;
    }

    public async Task CreateTransaction()
    {
        await _dbContext.Database.BeginTransactionAsync();
    }

    public async Task Commit()
    {
        await _dbContext.SaveChangesAsync();
        await _dbContext.Database.CommitTransactionAsync();
    }

    public async Task Rollback()
    {
        await _dbContext.Database.RollbackTransactionAsync();
    }

    public async Task SaveAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}