using FluentValidation;
using Lemax.HotelSearch.Api.Models;
using Lemax.HotelSearch.Service.Interfaces;

namespace Lemax.HotelSearch.Api.Validation.Request;

/// <summary>
/// Represents create city http request validator type.
/// </summary>
public class CreateCityHttpRequestValidator : AbstractValidator<CreateCityHttpRequest>
{
    public CreateCityHttpRequestValidator(ICountryRepository countryRepository)
    {
        RuleFor(x => x.Name)
            .NotNull()
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .WithMessage("Name must not be empty.")
            .MaximumLength(120);

        RuleFor(x => x.CountryId)
            .NotEmpty()
            .Must(countryId => countryRepository.GetByIdAsync(countryId).GetAwaiter().GetResult() is not null)
            .WithMessage("Country does not exist.");

        RuleFor(x => x.CenterLatitude)
            .InclusiveBetween(-90, 90);

        RuleFor(x => x.CenterLongitude)
            .InclusiveBetween(-180, 180);
    }
}
