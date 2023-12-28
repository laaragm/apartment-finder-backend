using ApartmentFinder.Domain.Users;
using ApartmentFinder.Domain.Abstractions;
using ApartmentFinder.Application.Abstractions.Messaging;
using ApartmentFinder.Application.Abstractions.Authentication;

namespace ApartmentFinder.Application.Users.LogInUser;

public sealed class LogInUserCommandHandler : ICommandHandler<LogInUserCommand, AccessTokenResponse>
{
	private readonly IJwtService _jwtService;

	public LogInUserCommandHandler(IJwtService jwtService)
	{
		_jwtService = jwtService;
	}

	public async Task<Result<AccessTokenResponse>> Handle(LogInUserCommand request, CancellationToken cancellationToken)
	{
		var result = await _jwtService.GetAccessTokenAsync(request.Email, request.Password, cancellationToken);

		if (result.IsFailure)
			return Result.Failure<AccessTokenResponse>(UserErrors.InvalidCredentials);

		return new AccessTokenResponse(result.Value);
	}
}
