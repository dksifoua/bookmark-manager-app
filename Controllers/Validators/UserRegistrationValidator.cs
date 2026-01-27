using BookmarkManagerApp.Controllers.Requests;
using FluentValidation;

namespace BookmarkManagerApp.Controllers.Validators;

public class UserRegistrationValidator : DefaultValidator<UserRegistrationRequest>
{
    public UserRegistrationValidator()
    {
        RuleFor(x => x.Fullname).NotEmpty().MinimumLength(3).MaximumLength(200);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(320);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8).MaximumLength(255);
    }
}