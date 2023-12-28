using ApartmentFinder.Domain.Abstractions;

namespace ApartmentFinder.Domain.Reviews.Events;

public sealed record ReviewCreatedDomainEvent(ReviewId ReviewId) : IDomainEvent;
