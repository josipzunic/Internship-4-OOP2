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
        base.OnModelCreating(builder);
        
        builder.ApplyConfiguration(new CompaniesConfiguration());
    }
}