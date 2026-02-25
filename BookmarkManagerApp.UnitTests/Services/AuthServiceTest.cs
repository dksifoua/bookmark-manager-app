using BookmarkManagerApp.Exceptions;
using BookmarkManagerApp.Models;
using BookmarkManagerApp.Repositories.Contracts;
using BookmarkManagerApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;

namespace BookmarkManagerApp.UnitTests.Services;

public class AuthServiceTest
{
    private readonly Mock<IUserRepository> _mockUserRepository;

    private readonly PasswordHasher<IdentityUser> _passwordHasher;
    private readonly AuthService _authService;

    public AuthServiceTest()
    {
        _mockUserRepository = new Mock<IUserRepository>();

        _passwordHasher = new PasswordHasher<IdentityUser>();

        var inMemorySettings = new Dictionary<string, string>
        {
            { "Jwt:Key", "ThisIsASecretKeyForJwtTokenGenerationWith256Bits" },
            { "Jwt:Issuer", "TestIssuer" },
            { "Jwt:Audience", "TestAudience" },
            { "Jwt:DurationInMinutes", "5" }
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();
        _authService = new AuthService(_mockUserRepository.Object, _passwordHasher, configuration);
    }

    [Fact]
    public async Task RegisterAsync_ShouldCreateUser_WhenEmailDoesNotExist()
    {
        // Arrange
        const string fullname = "John Doe";
        const string email = "john.doe@example.com";
        const string password = "password123";
        var hashedPassword = _passwordHasher.HashPassword(new IdentityUser(), password);

        _mockUserRepository
            .Setup(repo => repo.EmailExistsAsync(email.ToLower()))
            .ReturnsAsync(false);
        _mockUserRepository
            .Setup(repo => repo.CreateAsync(It.IsAny<User>()))
            .ReturnsAsync(new User
                { UserId = 1, Email = email.ToLower(), Fullname = fullname, Password = hashedPassword });

        // Act
        var result = await _authService.RegisterAsync(fullname, email, password);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(email.ToLower(), result.Email);
        Assert.Equal(fullname, result.Fullname);
        Assert.Equal(hashedPassword, result.Password);

        _mockUserRepository.Verify(repo => repo.EmailExistsAsync(email.ToLower()), Times.Once);
        _mockUserRepository.Verify(repo => repo.CreateAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task RegisterAsync_ShouldThrowConflictException_WhenEmailAlreadyExists()
    {
        // Arrange
        const string fullname = "John Doe";
        const string email = "john.doe@example.com";
        const string password = "password123";

        _mockUserRepository
            .Setup(repo => repo.EmailExistsAsync(email.ToLower()))
            .ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(() => _authService.RegisterAsync(fullname, email, password));

        _mockUserRepository.Verify(repo => repo.EmailExistsAsync(email.ToLower()), Times.Once);
        _mockUserRepository.Verify(repo => repo.CreateAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task AuthenticateUserAsync_ShouldReturnJwtToken_WhenCredentialsAreValid()
    {
        // Arrange
        const string email = "john.doe@example.com";
        const string password = "password123";
        var user = new User
        {
            UserId = 1, Email = email.ToLower(), Fullname = "John Doe",
            Password = _passwordHasher.HashPassword(new IdentityUser(), password)
        };

        _mockUserRepository
            .Setup(repo => repo.GetByEmailAsync(email.ToLower()))
            .ReturnsAsync(user);

        // Act
        var result = await _authService.AuthenticateUserAsync(email, password);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Token);
        Assert.Equal(user.Fullname, result.Fullname);
        Assert.Equal(user.Email, result.Email);

        _mockUserRepository.Verify(repo => repo.GetByEmailAsync(email.ToLower()), Times.Once);
    }

    [Fact]
    public async Task AuthenticateUserAsync_ShouldThrowUnauthorizedException_WhenUserDoesNotExist()
    {
        // Arrange
        const string email = "nonexistent@example.com";
        const string password = "password123";

        _mockUserRepository
            .Setup(repo => repo.GetByEmailAsync(email.ToLower()))
            .ReturnsAsync(null as User);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedException>(() => _authService.AuthenticateUserAsync(email, password));

        _mockUserRepository.Verify(repo => repo.GetByEmailAsync(email.ToLower()), Times.Once);
    }

    [Fact]
    public async Task AuthenticateUserAsync_ShouldThrowUnauthorizedException_WhenPasswordIsInvalid()
    {
        // Arrange
        const string email = "john.doe@example.com";
        const string goodPassword = "goodPassword";
        const string wrongPassword = "wrongPassword";
        var user = new User
        {
            UserId = 1, Email = email.ToLower(), Fullname = "John Doe",
            Password = _passwordHasher.HashPassword(new IdentityUser(), goodPassword)
        };

        _mockUserRepository
            .Setup(repo => repo.GetByEmailAsync(email.ToLower()))
            .ReturnsAsync(user);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedException>(() => _authService.AuthenticateUserAsync(email, wrongPassword));

        _mockUserRepository.Verify(repo => repo.GetByEmailAsync(email.ToLower()), Times.Once);
    }
}