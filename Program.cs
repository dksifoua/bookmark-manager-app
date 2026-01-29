using System.Security.Claims;
using System.Text;
using BookmarkManagerApp.Exceptions;
using BookmarkManagerApp.Exceptions.Handlers;
using BookmarkManagerApp.Persistence;
using BookmarkManagerApp.Repositories;
using BookmarkManagerApp.Services;
using BookmarkManagerApp.Services.Utils;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        var allowedOrigins = builder.Configuration.GetSection("AllowedCorsOrigins").Get<string[]>();
        if (allowedOrigins != null)
        {
            policy.WithOrigins(allowedOrigins)
                .AllowCredentials()
                .AllowAnyMethod()
                .AllowAnyHeader();
        }
    });
});

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

builder.Services.AddExceptionHandler<ResourceNotFoundExceptionHandler>();
builder.Services.AddExceptionHandler<BadRequestExceptionHandler>();
builder.Services.AddExceptionHandler<ConflictExceptionHandler>();
builder.Services.AddExceptionHandler<UnauthorizedExceptionHandler>();
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddDbContext<BookmarkDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connectionString);
});

builder.Services.AddSingleton<PasswordHasher<IdentityUser>>();

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<BookmarkRepository>();
builder.Services.AddScoped<TagRepository>();
builder.Services.AddScoped<BookmarkTagRepository>();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<BookmarkService>();
builder.Services.AddScoped<TagService>();
builder.Services.AddScoped<BookmarkTagService>();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"] ?? string.Empty)),
    };
    
    options.MapInboundClaims = false; // Disable Microsoft MapInboundClaims to keep JWT claim names as is.
    
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Cookies.TryGetValue("token", out var token))
            {
                context.Token = token;
            }
            return Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {
            if (context.Exception is SecurityTokenExpiredException)
            {
                context.Response.Headers.Append("Token-Expired", "true");
            }
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogWarning("Authentication failed: {ExceptionMessage}", context.Exception.Message);

            return Task.CompletedTask;
        }
    };
});
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ClaimsPrincipal>(sp =>
{
    var accessor = sp.GetRequiredService<IHttpContextAccessor>();
    return accessor.HttpContext?.User ?? new ClaimsPrincipal(new ClaimsIdentity());
});
builder.Services.AddScoped<UserContext>();

builder.Services.AddControllers();

var app = builder.Build();
app.MapGet("/", (IConfiguration configuration) =>
{
    var appName = configuration["AppSettings:ApplicationName"];
    var appVersion = configuration["AppSettings:ApplicationVersion"];
    return Results.Ok(new { appName, appVersion });
});

app.UseCors("CorsPolicy");


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("Bookmark Manager API")
            .WithTheme(ScalarTheme.Kepler)
            .ExpandAllTags();
    });
}

// Suppress diagnostics only for specific exception types (ApiException)
app.UseExceptionHandler(new ExceptionHandlerOptions
{
    SuppressDiagnosticsCallback = context => context.Exception is ApiException
});
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();