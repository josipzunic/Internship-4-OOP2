using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Configurations;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        
    }
    
    public DbSet<User>  Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        builder.HasDefaultSchema(Schemas.Default);
    }
}