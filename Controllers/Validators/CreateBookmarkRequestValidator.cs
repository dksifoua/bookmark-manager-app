using BookmarkManagerApp.Controllers.Requests;
using FluentValidation;

namespace BookmarkManagerApp.Controllers.Validators;

public class CreateBookmarkRequestValidator : DefaultRequestValidator<CreateBookmarkRequest>
{
    public CreateBookmarkRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Url).NotEmpty().IsValidUrl().MaximumLength(2048);
        RuleFor(x => x.Description).MaximumLength(1024);
    }
}