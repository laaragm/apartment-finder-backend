using ApartmentFinder.Domain.Abstractions;

namespace ApartmentFinder.Domain.Users.Events;

public sealed record class UserCreatedDomainEvent(UserId UserId) : IDomainEvent;
