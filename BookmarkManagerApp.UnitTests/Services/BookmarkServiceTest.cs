using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BookmarkManagerApp.Exceptions;
using BookmarkManagerApp.Models;
using BookmarkManagerApp.Repositories.Contracts;
using BookmarkManagerApp.Services;
using BookmarkManagerApp.Services.Utils;
using Moq;

namespace BookmarkManagerApp.UnitTests.Services;

public class BookmarkServiceTest
{
    private readonly Mock<IBookmarkRepository> _bookmarkRepositoryMock;
    private readonly Mock<ITagRepository> _tagRepositoryMock;
    private readonly BookmarkService _bookmarkService;
    private const long UserId = 1L;

    public BookmarkServiceTest()
    {
        _bookmarkRepositoryMock = new Mock<IBookmarkRepository>();
        _tagRepositoryMock = new Mock<ITagRepository>();

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, UserId.ToString())
        };
        var identity = new ClaimsIdentity(claims);
        var claimsPrincipal = new ClaimsPrincipal(identity);
        _bookmarkService = new BookmarkService(_bookmarkRepositoryMock.Object, new UserContext(claimsPrincipal),
            _tagRepositoryMock.Object);
    }

    [Fact]
    public async Task UpdateAsync_WithValidData_UpdatesBookmark()
    {
        // Arrange
        const long bookmarkId = 1L;
        var existingBookmark = new Bookmark
            { BookmarkId = bookmarkId, Title = "Old Title", Url = "https://old.com", Tags = new List<Tag>() };
        var command =
            new CreateOrUpdateBookmarkCommand("New Title", "https://new.com", "Description", ["tag1", "tag2"]);
        var existingTag = new Tag { Name = "tag1" };

        _bookmarkRepositoryMock
            .Setup(x => x.GetByIdForUpdateAsync(bookmarkId))
            .ReturnsAsync(existingBookmark);
        _bookmarkRepositoryMock
            .Setup(x => x.ExistsByUserIdAndTitle(UserId, command.Title))
            .ReturnsAsync(false);
        _bookmarkRepositoryMock
            .Setup(x => x.ExistsByUserIdAndUrl(UserId, command.Url))
            .ReturnsAsync(false);
        _tagRepositoryMock
            .Setup(x => x.GetByNamesForUpdate(command.TagNames))
            .ReturnsAsync([existingTag]);

        // Act
        await _bookmarkService.UpdateAsync(bookmarkId, command);

        // Assert
        Assert.Equal("New Title", existingBookmark.Title);
        Assert.Equal("https://new.com", existingBookmark.Url);
        Assert.Equal("Description", existingBookmark.Description);
        Assert.Equal(2, existingBookmark.Tags.Count);
        _bookmarkRepositoryMock.Verify(x => x.UpdateAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithNonExistentBookmark_ThrowsNotFoundException()
    {
        // Arrange
        _bookmarkRepositoryMock
            .Setup(x => x.GetByIdForUpdateAsync(It.IsAny<long>()))
            .ReturnsAsync(null as Bookmark);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _bookmarkService.UpdateAsync(It.IsAny<long>(), It.IsAny<CreateOrUpdateBookmarkCommand>()));
    }

    [Fact]
    public async Task UpdateAsync_WithDuplicateTitle_ThrowsConflictException()
    {
        // Arrange
        const long bookmarkId = 1L;
        var existingBookmark = new Bookmark
            { BookmarkId = bookmarkId, Title = "Old Title", Url = "https://url.com", Tags = new List<Tag>() };
        var command = new CreateOrUpdateBookmarkCommand("New Title", "https://url.com", "Description", []);

        _bookmarkRepositoryMock
            .Setup(x => x.GetByIdForUpdateAsync(bookmarkId))
            .ReturnsAsync(existingBookmark);
        _bookmarkRepositoryMock
            .Setup(x => x.ExistsByUserIdAndTitle(UserId, command.Title))
            .ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(() => _bookmarkService.UpdateAsync(bookmarkId, command));
    }

    [Fact]
    public async Task UpdateAsync_WithDuplicateUrl_ThrowsConflictException()
    {
        // Arrange
        const long bookmarkId = 1L;
        var existingBookmark = new Bookmark
            { BookmarkId = bookmarkId, Title = "Title", Url = "https://old.com", Tags = new List<Tag>() };
        var command =
            new CreateOrUpdateBookmarkCommand("Title", "https://new.com", "Description", []);
    
        _bookmarkRepositoryMock
            .Setup(x => x.GetByIdForUpdateAsync(bookmarkId))
            .ReturnsAsync(existingBookmark);
        _bookmarkRepositoryMock
            .Setup(x => x.ExistsByUserIdAndTitle(UserId, command.Title))
            .ReturnsAsync(false);
        _bookmarkRepositoryMock
            .Setup(x => x.ExistsByUserIdAndUrl(UserId, command.Url))
            .ReturnsAsync(true);
    
        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(() => _bookmarkService.UpdateAsync(bookmarkId, command));
    }

    [Fact]
    public async Task DeleteAsync_WithArchivedBookmark_DeletesBookmark()
    {
        // Arrange
        const long bookmarkId = 1L;
        var bookmark = new Bookmark { BookmarkId = bookmarkId, IsArchived = true };
        _bookmarkRepositoryMock.Setup(x => x.GetByIdAsync(bookmarkId)).ReturnsAsync(bookmark);

        // Act
        await _bookmarkService.DeleteAsync(bookmarkId);

        // Assert
        _bookmarkRepositoryMock.Verify(x => x.DeleteAsync(bookmarkId), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithNonExistentBookmark_ThrowsNotFoundException()
    {
        // Arrange
        const long bookmarkId = 1L;
        _bookmarkRepositoryMock.Setup(x => x.GetByIdAsync(bookmarkId)).ReturnsAsync(null as Bookmark);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _bookmarkService.DeleteAsync(bookmarkId));
    }

    [Fact]
    public async Task DeleteAsync_WithNonArchivedBookmark_ThrowsForbiddenException()
    {
        // Arrange
        const long bookmarkId = 1L;
        var bookmark = new Bookmark { BookmarkId = bookmarkId, IsArchived = false };
        _bookmarkRepositoryMock.Setup(x => x.GetByIdAsync(bookmarkId)).ReturnsAsync(bookmark);

        // Act & Assert
        await Assert.ThrowsAsync<ForbiddenException>(() => _bookmarkService.DeleteAsync(bookmarkId));
    }

    [Fact]
    public async Task TogglePinAsync_WithValidBookmark_TogglesPin()
    {
        // Arrange
        const long bookmarkId = 1L;
        var bookmark = new Bookmark { BookmarkId = bookmarkId, IsArchived = false };
        _bookmarkRepositoryMock.Setup(x => x.GetByIdAsync(bookmarkId)).ReturnsAsync(bookmark);

        // Act
        await _bookmarkService.TogglePinAsync(bookmarkId);

        // Assert
        _bookmarkRepositoryMock.Verify(x => x.TogglePinAsync(bookmarkId), Times.Once);
    }

    [Fact]
    public async Task TogglePinAsync_WithNonExistentBookmark_ThrowsNotFoundException()
    {
        // Arrange
        const long bookmarkId = 1L;
        _bookmarkRepositoryMock.Setup(x => x.GetByIdAsync(bookmarkId)).ReturnsAsync(null as Bookmark);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _bookmarkService.TogglePinAsync(bookmarkId));
    }

    [Fact]
    public async Task TogglePinAsync_WithArchivedBookmark_ThrowsForbiddenException()
    {
        // Arrange
        const long bookmarkId = 1L;
        var bookmark = new Bookmark { BookmarkId = bookmarkId, IsArchived = true };
        _bookmarkRepositoryMock.Setup(x => x.GetByIdAsync(bookmarkId)).ReturnsAsync(bookmark);

        // Act & Assert
        await Assert.ThrowsAsync<ForbiddenException>(() => _bookmarkService.TogglePinAsync(bookmarkId));
    }

    [Fact]
    public async Task ToggleArchiveAsync_WithExistingBookmark_TogglesArchive()
    {
        // Arrange
        const long bookmarkId = 1L;
        _bookmarkRepositoryMock.Setup(x => x.ExistsByBookmarkId(bookmarkId)).ReturnsAsync(true);

        // Act
        await _bookmarkService.ToggleArchiveAsync(bookmarkId);

        // Assert
        _bookmarkRepositoryMock.Verify(x => x.ToggleArchiveAsync(bookmarkId), Times.Once);
    }

    [Fact]
    public async Task ToggleArchiveAsync_WithNonExistentBookmark_ThrowsNotFoundException()
    {
        // Arrange
        const long bookmarkId = 1L;
        _bookmarkRepositoryMock.Setup(x => x.ExistsByBookmarkId(bookmarkId)).ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _bookmarkService.ToggleArchiveAsync(bookmarkId));
    }

    [Fact]
    public async Task GetAllByUserIdAndSearchTermAsync_ReturnsBookmarks()
    {
        // Arrange
        const string searchTerm = "test";
        var bookmarks = new List<Bookmark> { new() { BookmarkId = 1 } };
        _bookmarkRepositoryMock
            .Setup(x => x.GetAllByUserIdAndSearchTermAsync(UserId, searchTerm))
            .ReturnsAsync(bookmarks);

        // Act
        var result = await _bookmarkService.GetAllByUserIdAndSearchTermAsync(searchTerm);

        // Assert
        Assert.Equal(bookmarks, result);
        _bookmarkRepositoryMock.Verify(x => x.GetAllByUserIdAndSearchTermAsync(UserId, searchTerm), Times.Once);
    }

    [Fact]
    public async Task GetAllByUserIdAsync_ReturnsBookmarks()
    {
        // Arrange
        var bookmarks = new List<Bookmark> { new() { BookmarkId = 1 } };
        _bookmarkRepositoryMock.Setup(x => x.GetAllByUserIdAsync(UserId)).ReturnsAsync(bookmarks);

        // Act
        var result = await _bookmarkService.GetAllByUserIdAsync();

        // Assert
        Assert.Equal(bookmarks, result);
        _bookmarkRepositoryMock.Verify(x => x.GetAllByUserIdAsync(UserId), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingBookmark_ReturnsBookmark()
    {
        // Arrange
        const long bookmarkId = 1L;
        var bookmark = new Bookmark { BookmarkId = bookmarkId };
        _bookmarkRepositoryMock.Setup(x => x.GetByIdAsync(bookmarkId)).ReturnsAsync(bookmark);

        // Act
        var result = await _bookmarkService.GetByIdAsync(bookmarkId);

        // Assert
        Assert.Equal(bookmark, result);
    }

    [Fact]
    public async Task GetByIdAsync_WithNonExistentBookmark_ThrowsNotFoundException()
    {
        // Arrange
        const long bookmarkId = 1L;
        _bookmarkRepositoryMock.Setup(x => x.GetByIdAsync(bookmarkId)).ReturnsAsync(null as Bookmark);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _bookmarkService.GetByIdAsync(bookmarkId));
    }

    [Fact]
    public async Task CreateAsync_WithValidDataAndNoTags_CreatesBookmark()
    {
        // Arrange
        var command = new CreateOrUpdateBookmarkCommand("Title", "https://url.com", "Description", []);
        var createdBookmark = new Bookmark
            { BookmarkId = 1, Title = command.Title, Url = command.Url, Description = command.Description };

        _bookmarkRepositoryMock
            .Setup(x => x.ExistsByUserIdAndTitleOrUrl(UserId, command.Title, command.Url))
            .ReturnsAsync(false);
        _bookmarkRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<Bookmark>()))
            .ReturnsAsync(createdBookmark);

        // Act
        var result = await _bookmarkService.CreateAsync(command);

        // Assert
        Assert.Equal(createdBookmark, result);
        _bookmarkRepositoryMock.Verify(x => x.CreateAsync(It.Is<Bookmark>(b =>
            b.UserId == UserId &&
            b.Title == command.Title &&
            b.Url == command.Url &&
            b.Description == command.Description &&
            b.Tags.Count == 0
        )), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithValidDataAndExistingTags_CreatesBookmarkWithTags()
    {
        // Arrange
        var command = new CreateOrUpdateBookmarkCommand("Title", "https://url.com", "Description", ["tag1", "tag2"]);
        var existingTag = new Tag { Name = "tag1" };
        var createdBookmark = new Bookmark
        {
            BookmarkId = 1, Title = command.Title, Url = command.Url, Description = command.Description,
            Tags = new List<Tag> { new() { Name = "tag1" }, new() { Name = "tag2" } }
        };

        _bookmarkRepositoryMock
            .Setup(x => x.ExistsByUserIdAndTitleOrUrl(UserId, command.Title, command.Url))
            .ReturnsAsync(false);
        _tagRepositoryMock
            .Setup(x => x.GetByNamesForUpdate(command.TagNames))
            .ReturnsAsync([existingTag]);
        _bookmarkRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<Bookmark>()))
            .ReturnsAsync(createdBookmark);

        // Act
        var result = await _bookmarkService.CreateAsync(command);

        // Assert
        Assert.Equal(createdBookmark, result);
        _bookmarkRepositoryMock.Verify(x => x.CreateAsync(It.Is<Bookmark>(b => b.Tags.Count == 2)), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithValidDataAndNewTags_CreatesBookmarkWithNewTags()
    {
        // Arrange
        var command =
            new CreateOrUpdateBookmarkCommand("Title", "https://url.com", "Description", ["newTag1", "newTag2"]);
        var createdBookmark = new Bookmark
        {
            BookmarkId = 1, Title = command.Title, Url = command.Url, Description = command.Description,
            Tags = new List<Tag> { new() { Name = "newTag1" }, new() { Name = "newTag2" } }
        };

        _bookmarkRepositoryMock
            .Setup(x => x.ExistsByUserIdAndTitleOrUrl(UserId, command.Title, command.Url))
            .ReturnsAsync(false);
        _tagRepositoryMock
            .Setup(x => x.GetByNamesForUpdate(command.TagNames))
            .ReturnsAsync([]);
        _bookmarkRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<Bookmark>()))
            .ReturnsAsync(createdBookmark);

        // Act
        var result = await _bookmarkService.CreateAsync(command);

        // Assert
        Assert.Equal(createdBookmark, result);
        _bookmarkRepositoryMock.Verify(x => x.CreateAsync(It.Is<Bookmark>(b =>
            b.Tags.Count == 2 &&
            b.Tags.All(t => command.TagNames.AsEnumerable().Contains(t.Name))
        )), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithDuplicateTitleOrUrl_ThrowsConflictException()
    {
        // Arrange
        var command = new CreateOrUpdateBookmarkCommand("Title", "https://url.com", "Description", []);
        _bookmarkRepositoryMock
            .Setup(x => x.ExistsByUserIdAndTitleOrUrl(UserId, command.Title, command.Url))
            .ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(() => _bookmarkService.CreateAsync(command));
    }
}