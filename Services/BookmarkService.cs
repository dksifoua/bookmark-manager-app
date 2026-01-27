using BookmarkManagerApp.Exceptions;
using BookmarkManagerApp.Models;
using BookmarkManagerApp.Repositories;
using BookmarkManagerApp.Services.Commands;
using BookmarkManagerApp.Services.Utils;

namespace BookmarkManagerApp.Services;

public class BookmarkService(BookmarkRepository bookmarkRepository, UserContext userContext)
{
    public async Task<Bookmark> CreateBookmarkAsync(CreateBookmarkCommand command)
    {
        if (await bookmarkRepository.ExistsByUserIdAndTitleAndUrl(userContext.UserId, command.Title, command.Url))
        {
            throw new ConflictException("A bookmark with the same title and/or URL already exists for the user");
        }

        return await bookmarkRepository.CreateAsync(new Bookmark
        {
            UserId = userContext.UserId, Title = command.Title, Url = command.Url, Description = command.Description
        });
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