using MediatR;
using Microsoft.AspNetCore.Mvc;
using ApartmentFinder.Application.Bookings.GetBooking;
using ApartmentFinder.Application.Bookings.ReserveBooking;

namespace ApartmentFinder.API.Controllers.Bookings;

[Route("api/bookings")]
[ApiController]
public class BookingsController : ControllerBase
{
	private readonly ISender _sender;

	public BookingsController(ISender sender)
	{
		_sender = sender;
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetBooking(Guid id, CancellationToken cancellationToken)
	{
		var query = new GetBookingQuery(id);
		var result = await _sender.Send(query, cancellationToken);

		return result.IsSuccess ? Ok(result.Value) : NotFound();
	}

	[HttpPost]
	public async Task<IActionResult> ReserveBooking(ReserveBookingRequest request, CancellationToken cancellationToken)
	{
		// We don't want to expose the command, because we'd be coupling it to the endpoint. We'd be leaking that information, which shouldn't happen.
		var command = new ReserveBookingCommand(request.ApartmentId, request.UserId, request.StartDate, request.EndDate);
		var result = await _sender.Send(command, cancellationToken);
		if (result.IsFailure)
			return BadRequest(result.Error);

		// RESTful API convention and the response is going to contain a location header with the route to the get booking endpoint and the id of the newly created booking
		return CreatedAtAction(nameof(GetBooking), new { id = result.Value }, result.Value);
	}
}
