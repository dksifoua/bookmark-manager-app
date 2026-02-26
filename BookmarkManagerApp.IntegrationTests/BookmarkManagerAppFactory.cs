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
        
        await dbContext.Users.AddAsync(new User
        {
            Fullname = "Test User",
            Email = "test.user@example.com",
            Password = new PasswordHasher<IdentityUser>().HashPassword(new IdentityUser(), "Pass123!")
        }, CancellationToken.None);

        await dbContext.SaveChangesAsync();
    }

    public new async ValueTask DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
        await base.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}