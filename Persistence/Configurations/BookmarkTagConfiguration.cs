using BookmarkManagerApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookmarkManagerApp.Persistence.Configurations;

public class BookmarkTagConfiguration : IEntityTypeConfiguration<BookmarkTag>
{
    public void Configure(EntityTypeBuilder<BookmarkTag> builder)
    {
        builder.ToTable("bookmark_tags");
        builder.HasKey(x => new { x.BookmarkId, x.TagId });
        
        builder.Property(x => x.CreationTime).ValueGeneratedOnAdd();
        builder.Property(x => x.LastModifiedTime).ValueGeneratedOnUpdate();
        
        builder.HasOne(x => x.Bookmark)
            .WithMany(x => x.BookmarkTags)
            .HasForeignKey(x => x.BookmarkId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(x => x.Tag)
            .WithMany(x => x.BookmarkTags)
            .HasForeignKey(x => x.TagId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}