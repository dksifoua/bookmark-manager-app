using bookmark_manager_app.Models;
using bookmark_manager_app.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace bookmark_manager_app.Persistence;

public class BookmarkDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Bookmark> Bookmarks { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Visit> Visits { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("bookmark-manager");
        modelBuilder.UseIdentityByDefaultColumns();
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookmarkDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new CreationTimeInterceptor(), new LastModifiedTimeInterceptor());
    }
}