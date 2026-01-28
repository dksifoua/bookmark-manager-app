namespace BookmarkManagerApp.Models;

public class BookmarkTag : BaseModel
{
    public long BookmarkId { get; init; }
    public long TagId { get; init; }
    
    public Bookmark? Bookmark { get; init; }
    public Tag? Tag { get; init; }
}