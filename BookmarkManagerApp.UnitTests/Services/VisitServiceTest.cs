using BookmarkManagerApp.Exceptions;
using BookmarkManagerApp.Models;
using BookmarkManagerApp.Repositories.Contracts;
using BookmarkManagerApp.Services;
using Moq;

namespace BookmarkManagerApp.UnitTests.Services;

public class VisitServiceTest
{
    [Fact]
    public async Task CreateAsync_ShouldCreateVisit_WhenNoConflictExists()
    {
        // Arrange
        var mockRepository = new Mock<IVisitRepository>();
        var visit = new Visit { BookmarkId = 1, VisitTime = DateTime.UtcNow };

        mockRepository
            .Setup(r => r.ExistsByBookmarkIdAndCreationTime(visit.BookmarkId, visit.VisitTime))
            .ReturnsAsync(false);
        mockRepository
            .Setup(r => r.CreateAsync(visit))
            .ReturnsAsync(visit);

        var service = new VisitService(mockRepository.Object);

        // Act
        var result = await service.CreateAsync(visit);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(visit, result);
        mockRepository.Verify(r => r.ExistsByBookmarkIdAndCreationTime(visit.BookmarkId, visit.VisitTime), Times.Once);
        mockRepository.Verify(r => r.CreateAsync(visit), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowConflictException_WhenVisitAlreadyExists()
    {
        // Arrange
        var mockRepository = new Mock<IVisitRepository>();
        var visit = new Visit { BookmarkId = 1, VisitTime = DateTime.UtcNow };

        mockRepository
            .Setup(r => r.ExistsByBookmarkIdAndCreationTime(visit.BookmarkId, visit.VisitTime))
            .ReturnsAsync(true);

        var service = new VisitService(mockRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(() => service.CreateAsync(visit));
        mockRepository.Verify(r => r.ExistsByBookmarkIdAndCreationTime(visit.BookmarkId, visit.VisitTime), Times.Once);
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<Visit>()), Times.Never);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnVisit_WhenVisitExists()
    {
        // Arrange
        var mockRepository = new Mock<IVisitRepository>();
        const long visitId = 1L;
        var visit = new Visit { VisitId = visitId, BookmarkId = 1, VisitTime = DateTime.UtcNow };

        mockRepository
            .Setup(r => r.GetByIdAsync(visitId))
            .ReturnsAsync(visit);

        var service = new VisitService(mockRepository.Object);

        // Act
        var result = await service.GetByIdAsync(visitId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(visit, result);
        mockRepository.Verify(r => r.GetByIdAsync(visitId), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowNotFoundException_WhenVisitDoesNotExist()
    {
        // Arrange
        var mockRepository = new Mock<IVisitRepository>();
        const long visitId = 1L;

        mockRepository
            .Setup(r => r.GetByIdAsync(visitId))
            .ReturnsAsync(null as Visit);

        var service = new VisitService(mockRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => service.GetByIdAsync(visitId));
        mockRepository.Verify(r => r.GetByIdAsync(visitId), Times.Once);
    }
}