using BookmarkManagerApp.Models;
using BookmarkManagerApp.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace BookmarkManagerApp.Persistence;

public class BookmarkDbContext(DbContextOptions<BookmarkDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

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

        const string fullname = "Dimitri Sifoua";
        const string email = "dimitri.sifoua@gmail.com";
        const string password = "Password123";
        optionsBuilder
            .UseAsyncSeeding(async (context, _, cancellationToken) =>
            {
                var adminUser = await context.Set<User>().FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
                if (adminUser != null) return;

                adminUser = new User { FullName = fullname, Email = email, Password = password };
                await context.Set<User>().AddAsync(adminUser, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            })
            .UseSeeding((context, _) =>
            {
                var adminUser = context.Set<User>().FirstOrDefault(x => x.Email == email);
                if (adminUser != null) return;

                adminUser = new User { FullName = fullname, Email = email, Password = password };
                context.Set<User>().Add(adminUser);
                context.SaveChanges();
            });
    }
}