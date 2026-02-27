using BookmarkManagerApp.Models;
using BookmarkManagerApp.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BookmarkManagerApp.IntegrationTests;

// ReSharper disable once ClassNeverInstantiated.Global
public class BookmarkManagerAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly DatabaseTestContainer _dbContainer = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<BookmarkDbContext>>();
            services.AddDbContext<BookmarkDbContext>(options =>
            {
                options.UseNpgsql(_dbContainer.GetConnectionString());
            });
        });
    }

    public async ValueTask InitializeAsync()
    {
        await _dbContainer.InitializeAsync();

        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BookmarkDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
        
        await SeedTestData(dbContext);
    }

    public new async ValueTask DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
        await base.DisposeAsync();
        GC.SuppressFinalize(this);
    }

    private static async Task SeedTestData(BookmarkDbContext dbContext)
    {
        var user = new User
        {
            Fullname = "Test User",
            Email = "test.user@example.com",
            Password = new PasswordHasher<IdentityUser>().HashPassword(new IdentityUser(), "Pass123!")
        };
        await dbContext.Users.AddAsync(user, CancellationToken.None);
        
        var tags = new List<Tag>
        {
            new() { Name = "Tools" },
            new() { Name = "Community" },
            new() { Name = "Git" }
        };
        await dbContext.Tags.AddRangeAsync(tags, CancellationToken.None);
        
        var bookmarks = new List<Bookmark>
        {
            new (){
                User = user,
                Title = "GitHub",
                Url = "https://github.com",
                Description = "Where the world builds software. Millions of developers and companies build, ship, and maintain their software on GitHub.",
                Tags = tags.TakeLast(2).ToList()
            },
            new (){
                User = user,
                Title = "Stack Overflow",
                Url = "https://stackoverflow.com",
                Description = "Where the world builds software. Millions of developers and companies build, ship, and maintain their software on GitHub.",
                Tags = tags.Take(2).ToList()
            },
        };
        await dbContext.Bookmarks.AddRangeAsync(bookmarks, CancellationToken.None);

        var visit = new Visit
        {
            Bookmark = bookmarks.First(),
            VisitTime = DateTimeOffset.Parse("2026-02-27T09:00:00Z")
        };
        await dbContext.Visits.AddAsync(visit, CancellationToken.None);

        await dbContext.SaveChangesAsync();
    }
    
}