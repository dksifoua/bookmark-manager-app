using bookmark_manager_app.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bookmark_manager_app.Persistence.Configurations;

public class BookmarkConfiguration : IEntityTypeConfiguration<Bookmark>
{
    public void Configure(EntityTypeBuilder<Bookmark> builder)
    {
        builder.ToTable("bookmarks");
        builder.HasKey(x => x.BookmarkId);
        builder.HasIndex(x => new { x.UserId, x.Title, x.Url }).IsUnique();

        builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Url).IsRequired().HasMaxLength(2048);
        builder.Property(x => x.Description).HasMaxLength(1024);
        builder.Property(x => x.IsPinned).HasDefaultValue(false);
        builder.Property(x => x.IsArchived).HasDefaultValue(false);
        builder.Property(x => x.CreationTime).ValueGeneratedOnAdd();
        builder.Property(x => x.LastModifiedTime).ValueGeneratedOnUpdate();
        
        builder.HasOne(x => x.User)
            .WithMany(x => x.Bookmarks)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(x => x.Tags)
            .WithMany(x => x.Bookmarks)
            .UsingEntity(join => join.ToTable("bookmark_tags"));
    }
}