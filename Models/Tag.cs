namespace BookmarkManagerApp.Models;

public class Tag : BaseModel
{
    public long? TagId { get; init; }
    
    public string Name { get; init; } = string.Empty;
    
    public ICollection<Bookmark> Bookmarks { get; init; } = new List<Bookmark>();
}