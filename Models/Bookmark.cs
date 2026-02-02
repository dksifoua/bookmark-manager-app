namespace bookmark_manager_app.Models;

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
    
    public ICollection<Tag> Tags { get; init; } = new List<Tag>();
    public ICollection<Visit> Visits { get; init; } = new List<Visit>();
}