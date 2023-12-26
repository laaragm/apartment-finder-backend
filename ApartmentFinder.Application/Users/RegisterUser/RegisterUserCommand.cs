using ApartmentFinder.Application.Abstractions.Messaging;

namespace ApartmentFinder.Application.Users.RegisterUser;

public sealed record RegisterUserCommand(string Email, string FirstName, string LastName, string Password) : ICommand<Guid>;
