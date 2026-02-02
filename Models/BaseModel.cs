namespace bookmark_manager_app.Models;

public abstract class BaseModel
{
    public DateTimeOffset CreationTime { get; private set; }
    public DateTimeOffset? LastModifiedTime { get; private set; }
    
    public void SetCreationTimeToNow()
    {
        CreationTime = DateTimeOffset.UtcNow;
    }
    
    public void UpdateLastModifiedTimeToNow()
    {
        LastModifiedTime = DateTimeOffset.UtcNow;
    }
}