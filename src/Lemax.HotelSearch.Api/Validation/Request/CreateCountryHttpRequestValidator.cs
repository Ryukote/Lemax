using FluentValidation;
using Lemax.HotelSearch.Api.Models;

namespace Lemax.HotelSearch.Api.Validation.Request;

/// <summary>
/// Represents create country http request validator type.
/// </summary>
public class CreateCountryHttpRequestValidator : AbstractValidator<CreateCountryHttpRequest>
{
    public CreateCountryHttpRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .WithMessage("Name must not be empty.")
            .MaximumLength(120);

        RuleFor(x => x.IsoCode)
            .NotNull()
            .Must(code => !string.IsNullOrWhiteSpace(code))
            .WithMessage("IsoCode must not be empty.")
            .Length(2, 3)
            .Matches("^[A-Za-z]+$");
    }
}
