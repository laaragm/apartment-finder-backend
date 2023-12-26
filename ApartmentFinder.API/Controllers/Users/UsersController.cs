using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ApartmentFinder.Application.Users.LogInUser;
using ApartmentFinder.Application.Users.RegisterUser;
using ApartmentFinder.Application.Users.GetLoggedInUser;

namespace ApartmentFinder.API.Controllers.Users;

[Authorize]
[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
	private readonly ISender _sender;

	public UsersController(ISender sender)
	{
		_sender = sender;
	}

	[HttpGet("me")]
	public async Task<IActionResult> GetLoggedInUser(CancellationToken cancellationToken)
	{
		var query = new GetLoggedInUserQuery();
		var result = await _sender.Send(query, cancellationToken);

		return Ok(result.Value);
	}

	[AllowAnonymous]
	[HttpPost("register")]
	public async Task<IActionResult> Register(RegisterUserRequest request, CancellationToken cancellationToken)
	{
		var command = new RegisterUserCommand(request.Email, request.FirstName, request.LastName, request.Password);
		var result = await _sender.Send(command, cancellationToken);

		if (result.IsFailure)
			return BadRequest(result.Error);

		return Ok(result.Value);
	}

	[AllowAnonymous]
	[HttpPost("login")]
	public async Task<IActionResult> LogIn(LogInUserRequest request, CancellationToken cancellationToken)
	{
		var command = new LogInUserCommand(request.Email, request.Password);
		var result = await _sender.Send(command, cancellationToken);

		if (result.IsFailure)
			return Unauthorized(result.Error);

		return Ok(result.Value);
	}
}

