using MediatR;
using Microsoft.AspNetCore.Mvc;
using ApartmentFinder.Application.Reviews.AddReview;

namespace ApartmentFinder.API.Controllers.Reviews;

[Route("api/reviews")]
[ApiController]
public class ReviewsController : ControllerBase
{
	private readonly ISender _sender;

	public ReviewsController(ISender sender)
	{
		_sender = sender;
	}

	[HttpPost]
	public async Task<IActionResult> AddReview(AddReviewRequest request, CancellationToken cancellationToken)
	{
		var command = new AddReviewCommand(request.BookingId, request.Rating, request.Comment);
		var result = await _sender.Send(command, cancellationToken);

		if (result.IsFailure)
			return BadRequest(result.Error);

		return Ok();
	}
}
