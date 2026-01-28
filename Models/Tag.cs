namespace BookmarkManagerApp.Models;

public class Tag : BaseModel
{
    public long? TagId { get; init; }
    
    public string Name { get; init; } = string.Empty;
    
    public ICollection<BookmarkTag> BookmarkTags { get; init; } = new List<BookmarkTag>();
}