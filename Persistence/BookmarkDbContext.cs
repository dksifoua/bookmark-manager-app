using System.Text.Json;
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

    private static readonly JsonSerializerOptions JsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

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

        SeedDatabase(optionsBuilder);
    }

    private static void SeedDatabase(DbContextOptionsBuilder optionsBuilder)
    {
        var passwordHasher = new PasswordHasher<IdentityUser>();

        const string fullname = "Dimitri Sifoua";
        const string email = "dimitri.sifoua@gmail.com";
        const string password = "Password123";

        var bookmarksJsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Persistence/bookmarks.json");
        if (!File.Exists(bookmarksJsonPath)) return;

        var hashedPassword = passwordHasher.HashPassword(new IdentityUser(), password);
        optionsBuilder
            .UseAsyncSeeding(async (context, _, cancellationToken) =>
            {
                var adminUser = await context.Set<User>().FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
                if (adminUser != null) return;

                adminUser = new User { Fullname = fullname, Email = email, Password = hashedPassword };
                await context.Set<User>().AddAsync(adminUser, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
                
                if (adminUser.UserId is not { } adminUserId) return;
                var json = await File.ReadAllTextAsync(bookmarksJsonPath, cancellationToken);
                var seedItems = JsonSerializer.Deserialize<List<BookmarkSeedItem>>(json, JsonSerializerOptions) ?? [];
                foreach (var item in seedItems)
                {
                    var (title, url, description) = (item.Title.Trim(), item.Url.Trim(), item.Description.Trim());
                    if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(url)) continue;

                    if (
                        await context.Set<Bookmark>()
                            .AsNoTracking()
                            .AnyAsync(
                                b => b.UserId == adminUserId && b.Title == title && b.Url == url,
                                cancellationToken
                            )
                    ) continue;

                    var bookmark = new Bookmark
                        { UserId = adminUserId, Title = title, Url = url, Description = description };
                    await context.Set<Bookmark>().AddAsync(bookmark, cancellationToken);
                    await context.SaveChangesAsync(cancellationToken);

                    var tagNames = item.Tags
                        .Where(t => !string.IsNullOrWhiteSpace(t))
                        .Select(t => t.Trim())
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .ToArray();

                    foreach (var tagName in tagNames)
                    {
                        if (tagName.Length > 25) continue;

                        var tag = await context.Set<Tag>()
                            .FirstOrDefaultAsync(t => t.Name == tagName, cancellationToken);

                        if (tag?.TagId is not { } tagId)
                        {
                            tag = new Tag { Name = tagName };
                            await context.Set<Tag>().AddAsync(tag, cancellationToken);
                            await context.SaveChangesAsync(cancellationToken);
                            tagId = tag.TagId!.Value;
                        }

                        var linkExists = await context.Set<BookmarkTag>()
                            .AsNoTracking()
                            .AnyAsync(
                                bt => bt.BookmarkId == bookmark.BookmarkId!.Value && bt.TagId == tagId,
                                cancellationToken
                            );
                        if (linkExists) continue;

                        await context.Set<BookmarkTag>().AddAsync(
                            new BookmarkTag { BookmarkId = bookmark.BookmarkId!.Value, TagId = tagId },
                            cancellationToken
                        );
                        await context.SaveChangesAsync(cancellationToken);
                    }
                }
            })
            .UseSeeding((context, _) =>
            {
                var adminUser = context.Set<User>().FirstOrDefault(x => x.Email == email);
                if (adminUser != null) return;

                adminUser = new User { Fullname = fullname, Email = email, Password = hashedPassword };
                context.Set<User>().Add(adminUser);
                context.SaveChanges();

                if (adminUser.UserId is not { } adminUserId) return;
                var json = File.ReadAllText(bookmarksJsonPath);
                var seedItems = JsonSerializer.Deserialize<List<BookmarkSeedItem>>(json, JsonSerializerOptions) ?? [];
                foreach (var item in seedItems)
                {
                    var (title, url, description) = (item.Title.Trim(), item.Url.Trim(), item.Description.Trim());
                    if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(url)) continue;

                    if (
                        context.Set<Bookmark>()
                            .AsNoTracking()
                            .Any(
                                b => b.UserId == adminUserId && b.Title == title && b.Url == url
                            )
                    ) continue;

                    var bookmark = new Bookmark
                        { UserId = adminUserId, Title = title, Url = url, Description = description };
                    context.Set<Bookmark>().Add(bookmark);
                    context.SaveChanges();

                    var tagNames = item.Tags
                        .Where(t => !string.IsNullOrWhiteSpace(t))
                        .Select(t => t.Trim())
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .ToArray();

                    foreach (var tagName in tagNames)
                    {
                        if (tagName.Length > 25) continue;

                        var tag = context.Set<Tag>()
                            .FirstOrDefault(t => t.Name == tagName);

                        if (tag?.TagId is not { } tagId)
                        {
                            tag = new Tag { Name = tagName };
                            context.Set<Tag>().Add(tag);
                            context.SaveChanges();
                            tagId = tag.TagId!.Value;
                        }

                        var linkExists = context.Set<BookmarkTag>()
                            .AsNoTracking()
                            .Any(
                                bt => bt.BookmarkId == bookmark.BookmarkId!.Value && bt.TagId == tagId
                            );
                        if (linkExists) continue;

                        context.Set<BookmarkTag>().AddAsync(
                            new BookmarkTag { BookmarkId = bookmark.BookmarkId!.Value, TagId = tagId }
                        );
                        context.SaveChanges();
                    }
                }
            });
    }

    // ReSharper disable once ClassNeverInstantiated.Local
    private sealed record BookmarkSeedItem(string Title, string Url, string Description, string[] Tags);
}