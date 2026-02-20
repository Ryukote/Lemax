using FluentValidation;
using Lemax.HotelSearch.Api.Models;

namespace Lemax.HotelSearch.Api.Validation.Request;

/// <summary>
/// Represents get hotels query validator type.
/// </summary>
public class GetHotelsQueryValidator : AbstractValidator<GetHotelsQuery>
{
    public GetHotelsQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 200);

        RuleFor(x => x.CountryId)
            .Must(id => !id.HasValue || id.Value != Guid.Empty)
            .WithMessage("CountryId must be a valid GUID when provided.");

        RuleFor(x => x.CityId)
            .Must(id => !id.HasValue || id.Value != Guid.Empty)
            .WithMessage("CityId must be a valid GUID when provided.");
    }
}
