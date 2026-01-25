using BookmarkManagerApp.Exceptions;
using FluentValidation;
using FluentValidation.Results;

namespace BookmarkManagerApp.Controllers.Validators;

public class DefaultValidator<T> : AbstractValidator<T>
{
    protected override void RaiseValidationException(ValidationContext<T> context, ValidationResult result)
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