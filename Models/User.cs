namespace bookmark_manager_app.Models;

public sealed class User : BaseModel
{
    public long? UserId { get; init; }
    public string Fullname { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    
    public ICollection<Bookmark> Bookmarks { get; init; } = new List<Bookmark>();
}


