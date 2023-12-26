using ApartmentFinder.Application.Abstractions.Messaging;

namespace ApartmentFinder.Application.Users.LogInUser;

public sealed record LogInUserCommand(string Email, string Password) : ICommand<AccessTokenResponse>;
