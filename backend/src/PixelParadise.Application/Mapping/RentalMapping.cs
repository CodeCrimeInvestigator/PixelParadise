﻿using PixelParadise.Application.Contracts.Rental.Requests;
using PixelParadise.Application.Contracts.Rental.Responses;
using PixelParadise.Domain.Entities;
using PixelParadise.Domain.Options;

namespace PixelParadise.Application.Mapping;

public static class RentalMapping
{
    public static Rental MapToRental(this CreateRentalRequest request)
    {
        return new Rental
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            OwnerId = request.OwnerId
        };
    }

    public static RentalResponse MapToResponse(this Rental rental)
    {
        return new RentalResponse
        {
            Id = rental.Id,
            Name = rental.Name,
            Description = rental.Description,
            Price = rental.Price,
            OwnerId = rental.OwnerId
        };
    }

    public static Rental MapToRental(this UpdateRentalRequest request, Guid rentalId)
    {
        return new Rental
        {
            Id = rentalId,
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            OwnerId = request.OwnerId
        };
    }

    public static GetAllRentalOptions MapToOptions(this GetAllRentalsRequest request)
    {
        return new GetAllRentalOptions
        {
            Name = request.Name,
            Description = request.Description,
            PriceLowerLimit = request.PriceLowerLimit,
            PriceUpperLimit = request.PriceUpperLimit,
            OwnerUsername = request.OwnerUsername,
            SortField = request.SortBy?.Trim('+', '-'),
            SortOrder = request.SortBy is null ? SortOrder.Unsorted :
                request.SortBy.StartsWith('-') ? SortOrder.Descending : SortOrder.Ascending
        };
    }

    public static RentalsResponse MapToResponse(this IEnumerable<Rental> rentals)
    {
        return new RentalsResponse
        {
            Rentals = rentals.Select(MapToResponse).ToList()
        };
    }
}