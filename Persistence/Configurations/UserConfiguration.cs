using bookmark_manager_app.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bookmark_manager_app.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
         builder.ToTable("users");
        builder.HasKey(x => x.UserId);
        builder.HasIndex(x => x.Email).IsUnique();

        builder.Property(x => x.Fullname).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Email).IsRequired().HasMaxLength(320);
        builder.Property(x => x.Password).IsRequired().HasMaxLength(255);
        builder.Property(x => x.CreationTime).ValueGeneratedOnAdd();
        builder.Property(x => x.LastModifiedTime).ValueGeneratedOnUpdate();
    }
}