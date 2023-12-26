using ApartmentFinder.Application.Abstractions.Messaging;

namespace ApartmentFinder.Application.Users.GetLoggedInUser;

public sealed record GetLoggedInUserQuery : IQuery<UserResponse>;
