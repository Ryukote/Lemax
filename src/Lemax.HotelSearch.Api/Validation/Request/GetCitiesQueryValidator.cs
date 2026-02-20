using FluentValidation;
using Lemax.HotelSearch.Api.Models;

namespace Lemax.HotelSearch.Api.Validation.Request;

/// <summary>
/// Represents get cities query validator type.
/// </summary>
public class GetCitiesQueryValidator : AbstractValidator<GetCitiesQuery>
{
    public GetCitiesQueryValidator()
    {
        RuleFor(x => x.CountryId)
            .Must(id => !id.HasValue || id.Value != Guid.Empty)
            .WithMessage("CountryId must be a valid GUID when provided.");
    }
}
