using NpgsqlTypes;

namespace bookmark_manager_app.Models;

public class Bookmark : BaseModel
{
    public long? BookmarkId { get; init; }
    
    public long UserId { get; init; }
    
    public string Title { get; set; } = string.Empty;
    
    public string Url { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public bool IsPinned { get; init; }
    
    public bool IsArchived { get; init; }
    
    public User? User { get; init; }
    
    public NpgsqlTsVector SearchVector { get; init; } = NpgsqlTsVector.Empty;
    
    public ICollection<Tag> Tags { get; init; } = new List<Tag>();
    public ICollection<Visit> Visits { get; init; } = new List<Visit>();
}