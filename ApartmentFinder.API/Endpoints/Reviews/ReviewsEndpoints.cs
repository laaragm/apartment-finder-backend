﻿using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using ApartmentFinder.Domain.Abstractions;
using ApartmentFinder.Application.Reviews.AddReview;

namespace ApartmentFinder.API.Endpoints.Reviews;

public static class ReviewsEndpoints
{
	public static IEndpointRouteBuilder MapReviewsEndpoints(this IEndpointRouteBuilder builder)
	{
		var routeGroupBuilder = builder.MapGroup("api/reviews").RequireAuthorization();
		routeGroupBuilder.MapPost("", AddReview);

		return builder;
	}

	private static async Task<Results<Ok, BadRequest<Error>>> AddReview(AddReviewRequest request, ISender sender, CancellationToken cancellationToken)
	{
		var command = new AddReviewCommand(request.BookingId, request.Rating, request.Comment);
		var result = await sender.Send(command, cancellationToken);

		if (result.IsFailure)
			return TypedResults.BadRequest(result.Error);

		return TypedResults.Ok();
	}
}
