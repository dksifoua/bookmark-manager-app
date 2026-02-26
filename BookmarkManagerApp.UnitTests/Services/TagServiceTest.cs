using BookmarkManagerApp.Models;
using BookmarkManagerApp.Repositories.Contracts;
using BookmarkManagerApp.Services;
using Moq;

namespace BookmarkManagerApp.UnitTests.Services;

public class TagServiceTest
{
    [Fact]
    public async Task GetTagsAsync_ShouldReturnAllTags()
    {
        // Arrange
        var expectedTags = new List<Tag>
        {
            new() { TagId = 1, Name = "Technology" },
            new() { TagId = 2, Name = "Programming" },
            new() { TagId = 3, Name = "CSharp" }
        };

        var mockRepository = new Mock<ITagRepository>();
        mockRepository.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(expectedTags);

        var tagService = new TagService(mockRepository.Object);

        // Act
        var result = (await tagService.GetTagsAsync()).ToList();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedTags.Count, result.Count);
        Assert.Equal(expectedTags, result);
        
        mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
    }
}