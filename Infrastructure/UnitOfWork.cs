using Domain.Persistence.Common;
using Infrastructure.Database.Configurations;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly UsersDbContext _dbContext;
    private IDbContextTransaction _transaction;

    public UnitOfWork(UsersDbContext dbContext) => _dbContext = dbContext;
    
    
    public async Task CreateTransaction()
    {
        _transaction = await _dbContext.Database.BeginTransactionAsync();
    }

    public async Task Commit()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
        }
    }

    public async Task Rollback()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
        }
    }

    public async Task SaveAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}