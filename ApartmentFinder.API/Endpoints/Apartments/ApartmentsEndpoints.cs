using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using ApartmentFinder.Domain.Abstractions;
using ApartmentFinder.Application.Apartments.SearchApartments;

namespace ApartmentFinder.API.Endpoints.Apartments;

public static class ApartmentsEndpoints
{
	public static IEndpointRouteBuilder MapApartmentsEndpoints(this IEndpointRouteBuilder builder)
	{
		var routeGroupBuilder = builder.MapGroup("api/apartments").RequireAuthorization();
		routeGroupBuilder.MapGet("", SearchApartments);

		return builder;
	}

	public static async Task<Results<Ok<IReadOnlyList<ApartmentResponse>>, BadRequest<Error>>> SearchApartments(
		[FromQuery(Name = "startDate")] DateOnly startDate,
		[FromQuery(Name = "endDate")] DateOnly endDate,
		ISender sender, 
		CancellationToken cancellationToken)
	{
		var query = new SearchApartmentsQuery(startDate, endDate);
		var result = await sender.Send(query, cancellationToken);

		if (result.IsFailure)
			return TypedResults.BadRequest(result.Error);

		return TypedResults.Ok(result.Value);
	}
}

