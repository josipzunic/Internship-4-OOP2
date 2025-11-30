using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations.Users;

internal sealed class UsersConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.Email).HasColumnName("email");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.Username).HasColumnName("username");
        builder.Property(x => x.AdressStreet).HasColumnName("adress_street");
        builder.Property(x => x.AdressCity).HasColumnName("adress_city");
        builder.Property(x => x.Website).HasColumnName("website");
        builder.Property(x=> x.GeoLat).HasColumnName("geo_lat");
        builder.Property(x => x.GeoLng).HasColumnName("geo_lng");
        builder.Property(x => x.Password).HasColumnName("password");
        builder.Property(x => x.IsActive).HasColumnName("is_active");
        builder.Property(x => x.CreatedAt).HasColumnName("created_at");
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        
    }
}