using ApartmentFinder.Domain.Abstractions;

namespace ApartmentFinder.Domain.Bookings.Events;

public sealed record BookingCompletedDomainEvent(Guid BookingId) : IDomainEvent;
