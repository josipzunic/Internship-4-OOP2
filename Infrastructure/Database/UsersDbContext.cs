using Domain.Entities.Companies;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Configurations;

public class UsersDbContext : DbContext
{
    public UsersDbContext(DbContextOptions<UsersDbContext> options)
        : base(options)
    {
        
    }
    
    public DbSet<User>  Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(UsersDbContext).Assembly);
        builder.HasDefaultSchema(Schemas.Default);
    }
}