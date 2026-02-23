using bookmark_manager_app.Models;

namespace bookmark_manager_app.Controllers.Responses;

public record TagResponse(long? TagId, string Name)
{
    public static TagResponse FromModel(Tag tag) => new(tag.TagId, tag.Name);
}