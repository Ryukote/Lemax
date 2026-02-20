using FluentValidation;
using Lemax.HotelSearch.Api.Models;
using Lemax.HotelSearch.Service.Interfaces;

namespace Lemax.HotelSearch.Api.Validation.Request;

/// <summary>
/// Represents update hotel http request validator type.
/// </summary>
public class UpdateHotelHttpRequestValidator : AbstractValidator<UpdateHotelHttpRequest>
{
    public UpdateHotelHttpRequestValidator(ICityRepository cityRepository)
    {
        RuleFor(x => x.Name)
            .NotNull()
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .WithMessage("Name must not be empty.")
            .MaximumLength(200);

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .LessThanOrEqualTo(1_000_000);

        RuleFor(x => x.CityId)
            .NotEmpty()
            .Must(cityId => cityRepository.GetByIdAsync(cityId).GetAwaiter().GetResult() is not null)
            .WithMessage("City does not exist.");

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90);

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180);
    }
}
