using BookmarkManagerApp.Models;

namespace BookmarkManagerApp.Controllers.Responses;

public record TagResponse(long? TagId, string Name)
{
    public static TagResponse FromModel(Tag tag) => new(tag.TagId, tag.Name);
}