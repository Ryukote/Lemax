using FluentValidation;
using Lemax.HotelSearch.Api.Models;

namespace Lemax.HotelSearch.Api.Validation.Request;

/// <summary>
/// Represents search hotels query validator type.
/// </summary>
public class SearchHotelsQueryValidator : AbstractValidator<SearchHotelsQuery>
{
    private static readonly string[] AllowedSortBy = ["relevance", "price", "distance"];
    private static readonly string[] AllowedOrder = ["asc", "desc"];

    public SearchHotelsQueryValidator()
    {
        RuleFor(x => x.SearchLatitude)
            .InclusiveBetween(-90, 90);

        RuleFor(x => x.SearchLongitude)
            .InclusiveBetween(-180, 180);

        RuleFor(x => x.RadiusKm)
            .GreaterThan(0)
            .LessThanOrEqualTo(1000);

        RuleFor(x => x.SortBy)
            .NotEmpty()
            .Must(sortBy => AllowedSortBy.Contains(sortBy.Trim().ToLowerInvariant()))
            .WithMessage("SortBy must be one of: relevance, price, distance.");

        RuleFor(x => x.Order)
            .NotEmpty()
            .Must(order => AllowedOrder.Contains(order.Trim().ToLowerInvariant()))
            .WithMessage("Order must be one of: asc, desc.");

        RuleFor(x => x.Page)
            .GreaterThan(0);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 200);

        RuleFor(x => x.MinPrice)
            .GreaterThan(0)
            .When(x => x.MinPrice.HasValue);

        RuleFor(x => x.MaxPrice)
            .GreaterThan(0)
            .When(x => x.MaxPrice.HasValue);

        RuleFor(x => x)
            .Must(x => !x.MinPrice.HasValue || !x.MaxPrice.HasValue || x.MaxPrice.Value >= x.MinPrice.Value)
            .WithMessage("MaxPrice must be greater than or equal to MinPrice.");

        RuleFor(x => x.CountryId)
            .Must(id => !id.HasValue || id.Value != Guid.Empty)
            .WithMessage("CountryId must be a valid GUID when provided.");

        RuleFor(x => x.CityId)
            .Must(id => !id.HasValue || id.Value != Guid.Empty)
            .WithMessage("CityId must be a valid GUID when provided.");
    }
}
