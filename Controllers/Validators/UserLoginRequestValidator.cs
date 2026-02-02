using bookmark_manager_app.Controllers.Requests;
using FluentValidation;

namespace bookmark_manager_app.Controllers.Validators;

public class UserLoginRequestValidator : DefaultRequestValidator<UserLoginRequest>
{
    public UserLoginRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(320);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8).MaximumLength(255);
    }
}