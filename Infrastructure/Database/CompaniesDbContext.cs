using Domain.Entities.Companies;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Configurations;

public class CompaniesDbContext : DbContext
{
    public CompaniesDbContext(DbContextOptions<CompaniesDbContext> options)
        : base(options)
    {
        
    }
    
    public DbSet<Company>  Companies { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(CompaniesDbContext).Assembly);
        builder.HasDefaultSchema(Schemas.Default);
    }
}