using ApartmentFinder.Domain.Abstractions;

namespace ApartmentFinder.Domain.Bookings.Events;

public sealed record BookingRejectedDomainEvent(Guid BookingId) : IDomainEvent;
