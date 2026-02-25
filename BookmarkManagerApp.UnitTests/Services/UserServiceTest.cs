using BookmarkManagerApp.Exceptions;
using BookmarkManagerApp.Models;
using BookmarkManagerApp.Repositories.Contracts;
using BookmarkManagerApp.Services;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace BookmarkManagerApp.UnitTests.Services;

public class UserServiceTest
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly PasswordHasher<IdentityUser> _passwordHasher;
    private readonly UserService _userService;

    public UserServiceTest()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordHasher = new PasswordHasher<IdentityUser>();
        _userService = new UserService(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        const long userId = 1L;
        const string email = "test@example.com";
        var expectedUser = new User
        {
            UserId = userId,
            Email = email,
            Password = _passwordHasher.HashPassword(new IdentityUser(), "password")
        };

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(userId))
            .ReturnsAsync(expectedUser);

        // Act
        var result = await _userService.GetUserByIdAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userId, result.UserId);
        Assert.Equal(email, result.Email);
        _userRepositoryMock.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldThrowNotFoundException_WhenUserDoesNotExist()
    {
        // Arrange
        const long userId = 999L;

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(userId))
            .ReturnsAsync(null as User);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _userService.GetUserByIdAsync(userId));
        _userRepositoryMock.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
    }
}