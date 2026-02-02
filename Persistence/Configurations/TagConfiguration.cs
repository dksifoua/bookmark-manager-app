using bookmark_manager_app.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bookmark_manager_app.Persistence.Configurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable("tags");
        builder.HasKey(x => x.TagId);
        builder.HasIndex(x => x.Name).IsUnique();

        builder.Property(x => x.Name).IsRequired().HasMaxLength(25);
        builder.Property(x => x.CreationTime).ValueGeneratedOnAdd();
        builder.Property(x => x.LastModifiedTime).ValueGeneratedOnUpdate();
    }
}