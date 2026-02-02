using bookmark_manager_app.Controllers.Requests;
using FluentValidation;

namespace bookmark_manager_app.Controllers.Validators;

public class UserRegistrationRequestValidator : DefaultRequestValidator<UserRegistrationRequest>
{
    public UserRegistrationRequestValidator()
    {
        RuleFor(x => x.Fullname).NotEmpty().MinimumLength(3).MaximumLength(200);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(320);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8).MaximumLength(255);
    }
}