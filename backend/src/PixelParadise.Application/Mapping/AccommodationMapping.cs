using Microsoft.Extensions.Options;
using PixelParadise.Application.Contracts.Accommodation.Requests;
using PixelParadise.Application.Contracts.Accommodation.Responses;
using PixelParadise.Application.Options;
using PixelParadise.Domain.Entities;
using PixelParadise.Domain.Options;
using PixelParadise.Infrastructure.Repositories.Results;

namespace PixelParadise.Application.Mapping;

public static class RentalMapping
{
    public static Accommodation MapToAccommodation(this CreateAccommodationRequest request,
        IOptions<StorageOptions> options)
    {
        return new Accommodation
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            OwnerId = request.OwnerId,
            CoverImage = options.Value.RelDefaultAccommodationCoverImagePath
        };
    }

    public static AccommodationResponse MapToResponse(this Accommodation accommodation)
    {
        return new AccommodationResponse
        {
            Id = accommodation.Id,
            Name = accommodation.Name,
            Description = accommodation.Description,
            Price = accommodation.Price,
            OwnerId = accommodation.OwnerId,
            CoverImageUrl = accommodation.CoverImage,
            Images = accommodation.Images
        };
    }

    public static Accommodation MapToAccommodation(this UpdateAccommodationRequest request, Guid accommodationId)
    {
        return new Accommodation
        {
            Id = accommodationId,
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            OwnerId = request.OwnerId
        };
    }

    public static GetAllAccommodationOptions MapToOptions(this GetAllAccommodationsRequest request)
    {
        return new GetAllAccommodationOptions
        {
            Name = request.Name,
            Description = request.Description,
            PriceLowerLimit = request.PriceLowerLimit,
            PriceUpperLimit = request.PriceUpperLimit,
            OwnerUsername = request.OwnerUsername,
            SortField = request.SortBy?.Trim('+', '-'),
            SortOrder = request.SortBy is null ? SortOrder.Unsorted :
                request.SortBy.StartsWith('-') ? SortOrder.Descending : SortOrder.Ascending,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }

    public static AccommodationsResponse MapToResponse(this PaginatedResult<Accommodation> accommodations)
    {
        return new AccommodationsResponse
        {
            Items = accommodations.Items.Select(MapToResponse).ToList(),
            Page = accommodations.Page,
            PageSize = accommodations.PageSize,
            Total = accommodations.TotalCount
        };
    }
}