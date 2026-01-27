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
            Uri.TryCreate(uri, UriKind.RelativeOrAbsolute, out var outUri) 
            && (outUri.Scheme == Uri.UriSchemeHttp || outUri.Scheme == Uri.UriSchemeHttps));
    }
}