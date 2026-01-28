using BookmarkManagerApp.Exceptions;
using BookmarkManagerApp.Models;
using BookmarkManagerApp.Repositories;
using BookmarkManagerApp.Services.Commands;
using BookmarkManagerApp.Services.Utils;

namespace BookmarkManagerApp.Services;

public class BookmarkService(
    BookmarkRepository bookmarkRepository,
    TagRepository tagRepository,
    UserContext userContext)
{
    public async Task<Bookmark> CreateBookmarkAsync(CreateBookmarkCommand command)
    {
        if (await bookmarkRepository.ExistsByUserIdAndTitleAndUrl(userContext.UserId, command.Title, command.Url))
        {
            throw new ConflictException("A bookmark with the same title and/or URL already exists for the user");
        }

        var bookmark = new Bookmark
        {
            UserId = userContext.UserId,
            Title = command.Title,
            Url = command.Url,
            Description = command.Description
        };

        var tagnames = (command.Tagnames ?? []).Distinct()
            .Select(x => x.Trim().ToLowerInvariant())
            .Where(x => x.Length > 0)
            .Distinct()
            .ToArray();

        if (tagnames.Length == 0) return await bookmarkRepository.CreateAsync(bookmark);
        
        var existingTags = await tagRepository.RetrieveByNames(tagnames);
        var existingTagsByNames = existingTags.ToDictionary(tag => tag.Name);

        foreach (var tagName in tagnames)
        {
            if (!existingTagsByNames.TryGetValue(tagName, out var tag))
            {
                tag = await tagRepository.CreateWithoutCommitAsync(new Tag { Name = tagName });
                existingTagsByNames[tagName] = tag;
            }
                
            bookmark.BookmarkTags.Add(new BookmarkTag { Tag = tag });
        }

        return await bookmarkRepository.CreateAsync(bookmark);  
    }

    public async Task DeleteBookmarkAsync(long bookmarkId)
    {
        var bookmark = await RetrieveBookmarkByIdAsync(bookmarkId);
        if (bookmark == null)
        {
            throw new ResourceNotFoundException($"Bookmark with id {bookmarkId} not found");
        }

        await bookmarkRepository.DeleteAsync(bookmark);
    }

    public async Task<Bookmark> RetrieveBookmarkByIdAsync(long bookmarkId)
    {
        var bookmark = await bookmarkRepository.RetrieveByIdAsync(bookmarkId);
        return bookmark ?? throw new ResourceNotFoundException($"Bookmark with id {bookmarkId} not found.");
    }

    public async Task<IEnumerable<Bookmark>> RetrieveAllBookmarksByUserIdAsync() =>
        await bookmarkRepository.RetrieveAllByUserIdAsync(userContext.UserId);
}