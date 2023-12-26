using ApartmentFinder.Domain.Users;

namespace ApartmentFinder.Application.Abstractions.Authentication;

public interface IAuthenticationService
{
	Task<string> RegisterAsync(User user, string password, CancellationToken cancellationToken = default);
}
