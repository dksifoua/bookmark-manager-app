namespace BookmarkManagerApp.Models;

public class Bookmark : BaseModel
{
    public long? BookmarkId { get; init; }
    
    public long? UserId { get; init; }
    
    public string Title { get; init; } = string.Empty;
    
    public string Url { get; init; } = string.Empty;
    
    public string Description { get; init; } = string.Empty;
    
    public bool IsPinned { get; init; } = false;
    
    public bool IsArchived { get; init; } = false;
    
    public User? User { get; init; }
}