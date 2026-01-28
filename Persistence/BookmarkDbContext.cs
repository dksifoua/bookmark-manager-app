using BookmarkManagerApp.Models;
using BookmarkManagerApp.Persistence.Interceptors;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookmarkManagerApp.Persistence;

public class BookmarkDbContext(DbContextOptions<BookmarkDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Bookmark> Bookmarks => Set<Bookmark>();
    public DbSet<Visit> Visits => Set<Visit>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<BookmarkTag> BookmarkTags => Set<BookmarkTag>();

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

        SeedAdminUser(optionsBuilder);
    }

    private static void SeedAdminUser(DbContextOptionsBuilder optionsBuilder)
    {
        var passwordHasher = new PasswordHasher<IdentityUser>();

        const string fullname = "Dimitri Sifoua";
        const string email = "dimitri.sifoua@gmail.com";
        const string password = "Password123";

        var hashedPassword = passwordHasher.HashPassword(new IdentityUser(), password);
        optionsBuilder
            .UseAsyncSeeding(async (context, _, cancellationToken) =>
            {
                var adminUser = await context.Set<User>().FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
                if (adminUser != null) return;

                adminUser = new User { Fullname = fullname, Email = email, Password = hashedPassword };
                await context.Set<User>().AddAsync(adminUser, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            })
            .UseSeeding((context, _) =>
            {
                var adminUser = context.Set<User>().FirstOrDefault(x => x.Email == email);
                if (adminUser != null) return;

                adminUser = new User { Fullname = fullname, Email = email, Password = hashedPassword };
                context.Set<User>().Add(adminUser);
                context.SaveChanges();
            });
    }
}