using Testcontainers.PostgreSql;

namespace BookmarkManagerApp.IntegrationTests;

// ReSharper disable once ClassNeverInstantiated.Global
public class DatabaseTestContainer : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder("postgres:alpine")
        .WithDatabase("bookmark_test")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .WithPortBinding(PostgreSqlBuilder.PostgreSqlPort, true)
        .Build();

    public string GetConnectionString() => _container.GetConnectionString();
    
    public async ValueTask InitializeAsync() => await _container.StartAsync();

    public async ValueTask DisposeAsync()
    {
        await _container.StopAsync();
        GC.SuppressFinalize(this);
    }
}