using BookmarkManagerApp.Controllers.Requests;
using BookmarkManagerApp.Exceptions;
using FluentValidation;
using FluentValidation.Results;

namespace BookmarkManagerApp.Controllers.Validators;

public class UserRegistrationValidator : AbstractValidator<UserRegistrationRequest>
{
    public UserRegistrationValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().MinimumLength(3).MaximumLength(200);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(320);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8).MaximumLength(255);
        RuleFor(x => x.ConfirmedPassword).NotEmpty().MaximumLength(255)
            .Equal(x => x.Password)
            .WithMessage("Passwords do not match.");
    }

    protected override void RaiseValidationException(ValidationContext<UserRegistrationRequest> context, ValidationResult result)
    {
        var errors = result.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );

        throw new CustomValidationException(errors);
    }
}