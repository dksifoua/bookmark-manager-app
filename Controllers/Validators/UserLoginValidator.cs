using BookmarkManagerApp.Controllers.Requests;
using FluentValidation;

namespace BookmarkManagerApp.Controllers.Validators;

public class UserLoginValidator : DefaultValidator<UserLoginRequest>
{
    public UserLoginValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(320);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8).MaximumLength(255);
    }
}