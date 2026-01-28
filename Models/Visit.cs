namespace BookmarkManagerApp.Models;

public class Visit : BaseModel
{
    public long? VisitId { get; init; }
    
    public long BookmarkId { get; init; }
    
    public DateTimeOffset VisitTime { get; init; } = DateTimeOffset.UtcNow;
    
    public Bookmark? Bookmark { get; init; }
}