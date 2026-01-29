namespace BookmarkManagerApp.Models;

public class Bookmark : BaseModel
{
    public long? BookmarkId { get; init; }
    
    public long UserId { get; init; }
    
    public string Title { get; init; } = string.Empty;
    
    public string Url { get; init; } = string.Empty;
    
    public string Description { get; init; } = string.Empty;
    
    public bool IsPinned { get; init; }
    
    public bool IsArchived { get; init; }
    
    public User? User { get; init; }
    
    public ICollection<Visit> Visits { get; init; } = new List<Visit>();
    public ICollection<BookmarkTag> BookmarkTags { get; init; } = new List<BookmarkTag>();
}