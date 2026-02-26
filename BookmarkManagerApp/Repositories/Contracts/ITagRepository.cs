using BookmarkManagerApp.Models;

namespace BookmarkManagerApp.Repositories.Contracts;

public interface ITagRepository
{
    Task<IEnumerable<Tag>> GetByNamesForUpdate(IEnumerable<string> names);
    Task<IEnumerable<Tag>> GetAllAsync();
}