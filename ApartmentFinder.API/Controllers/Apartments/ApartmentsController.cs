using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ApartmentFinder.Application.Apartments.SearchApartments;

namespace ApartmentFinder.API.Controllers.Apartments;

[Authorize]
[Route("api/apartments")]
[ApiController]
public class ApartmentsController : ControllerBase
{
	private readonly ISender _sender;

	public ApartmentsController(ISender sender)
	{
		_sender = sender;
	}

	[HttpGet]
	public async Task<IActionResult> SearchApartments(DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken)
	{
		var query = new SearchApartmentsQuery(startDate, endDate);
		var result = await _sender.Send(query, cancellationToken);

		return Ok(result);
	}
}
