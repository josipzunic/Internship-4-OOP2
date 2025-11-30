using Domain.Entities.Companies;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Configurations;

public class CompaniesDbContext : DbContext
{
    public CompaniesDbContext(DbContextOptions<UsersDbContext> options)
        : base(options)
    {
        
    }
    
    public DbSet<Company>  Companies { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(UsersDbContext).Assembly);
        builder.HasDefaultSchema(Schemas.Default);
    }
}