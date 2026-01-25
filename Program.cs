using BookmarkManagerApp.Exceptions;
using BookmarkManagerApp.Exceptions.Handlers;
using BookmarkManagerApp.Persistence;
using BookmarkManagerApp.Repositories;
using BookmarkManagerApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;
        context.ProblemDetails.Extensions["timestamp"] = DateTime.UtcNow;
        context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
    };
});
builder.Services.AddExceptionHandler<ResourceAlreadyExistsExceptionHandler>();
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddDbContext<BookmarkDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connectionString);
});
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddSingleton<PasswordHasher<IdentityUser>>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddControllers();

var app = builder.Build();
app.MapGet("/", (IConfiguration configuration) =>
{
    var appName = configuration["AppSettings:ApplicationName"];
    var appVersion = configuration["AppSettings:ApplicationVersion"];
    return Results.Ok(new { appName, appVersion });
});


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// Suppress diagnostics only for specific exception types (ApiException)
app.UseExceptionHandler(new ExceptionHandlerOptions
{
    SuppressDiagnosticsCallback = context => context.Exception is ApiException
});
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();