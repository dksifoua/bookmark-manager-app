using BookmarkManagerApp.Exceptions;
using FluentValidation;
using FluentValidation.Results;

namespace BookmarkManagerApp.Controllers.Validators;

public class DefaultRequestValidator<T> : AbstractValidator<T>
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

public static class FluentValidationExtensions
{
    public static IRuleBuilderOptions<T, string> IsValidUrl<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(uri =>
            {
                if (string.IsNullOrWhiteSpace(uri))
                    return false;

                if (!Uri.TryCreate(uri, UriKind.Absolute, out var outUri))
                    return false;

                return outUri.Scheme == Uri.UriSchemeHttp || outUri.Scheme == Uri.UriSchemeHttps;
            })
            .WithMessage("Url must be a valid absolute http/https URL.");
    }
}