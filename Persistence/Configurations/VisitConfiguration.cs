using BookmarkManagerApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookmarkManagerApp.Persistence.Configurations;

public class VisitConfiguration : IEntityTypeConfiguration<Visit>
{
    public void Configure(EntityTypeBuilder<Visit> builder)
    {
        builder.ToTable("visits");
        builder.HasKey(x => x.VisitId);
        
        builder.Property(x => x.VisitTime).IsRequired();
        builder.Property(x => x.CreationTime).ValueGeneratedOnAdd();
        builder.Property(x => x.LastModifiedTime).ValueGeneratedOnUpdate();
        
        builder.HasOne(x => x.Bookmark)
            .WithMany(x => x.Visits)
            .HasForeignKey(x => x.BookmarkId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}