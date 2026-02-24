using System.Security.Claims;
using System.Text;
using BookmarkManagerApp.Exceptions.Handlers;
using BookmarkManagerApp.Persistence;
using BookmarkManagerApp.Repositories;
using BookmarkManagerApp.Repositories.Contracts;
using BookmarkManagerApp.Services;
using BookmarkManagerApp.Services.Utils;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();

builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Instance = context.HttpContext.Request.Path;
        context.ProblemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;
        context.ProblemDetails.Extensions["timestamp"] = DateTimeOffset.UtcNow.ToString();
        context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
    };
});

builder.Services.AddExceptionHandler<UnauthorizedExceptionHandler>();
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<NotFoundExceptionHandler>();
builder.Services.AddExceptionHandler<BadRequestExceptionHandler>();
builder.Services.AddExceptionHandler<ConflictExceptionHandler>();
builder.Services.AddExceptionHandler<ForbiddenExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddDbContext<BookmarkDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connectionString);
});

builder.Services.AddSingleton<PasswordHasher<IdentityUser>>();

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<BookmarkRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<VisitRepository>();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<BookmarkService>();
builder.Services.AddScoped<TagService>();
builder.Services.AddScoped<VisitService>();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var jwt = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwt["Issuer"],
        ValidAudience = jwt["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwt["Key"]!)
        )
    };

    options.MapInboundClaims = false; // Disable Microsoft MapInboundClaims to keep JWT claim names as is.

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Cookies.TryGetValue("token", out var token) && !string.IsNullOrWhiteSpace(token))
            {
                context.Token = token;
            }

            context.HttpContext.Items["TokenProvided"] = !string.IsNullOrWhiteSpace(context.Token);

            return Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {
            if (context.Exception is SecurityTokenExpiredException)
            {
                context.HttpContext.Items["TokenExpired"] = true;
            }
            else
            {
                context.HttpContext.Items["TokenInvalid"] = true;
            }

            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            var endpoint = context.HttpContext.GetEndpoint();
            var isProtectedEndpoint = endpoint?.Metadata.GetMetadata<IAuthorizeData>() != null;
            if (!isProtectedEndpoint)
            {
                return Task.CompletedTask;
            }

            var tokenProvided = context.HttpContext.Items.TryGetValue("TokenProvided", out var providedObj)
                                && providedObj is true;
            if (!tokenProvided)
            {
                context.Response.Headers["Token-Missing"] = "true";
                return Task.CompletedTask;
            }

            var tokenInvalid = context.HttpContext.Items.TryGetValue("TokenInvalid", out var invalidObj)
                               && invalidObj is true;
            if (tokenInvalid)
            {
                context.Response.Headers["Token-Invalid"] = "true";
                return Task.CompletedTask;
            }
            
            var tokenExpired = context.HttpContext.Items.TryGetValue("TokenExpired", out var expiredObj)
                               && expiredObj is true;
            if (!tokenExpired) return Task.CompletedTask;
            context.Response.Headers["Token-Expired"] = "true";
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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        var allowedOrigins = builder.Configuration.GetSection("AllowedCorsOrigins").Get<string[]>();
        if (allowedOrigins != null)
        {
            policy.WithOrigins(allowedOrigins)
                .WithExposedHeaders("Token-Invalid", "Token-Missing", "Token-Expired", "WWW-Authenticate")
                .AllowCredentials()
                .AllowAnyMethod()
                .AllowAnyHeader();
        }
    });
});

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.UseExceptionHandler();
app.MapHealthChecks("/api/health");
app.MapControllers();
app.Run();