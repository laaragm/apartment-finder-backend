using ApartmentFinder.Domain.Abstractions;

namespace ApartmentFinder.Domain.Bookings.Events;

public sealed record BookingReservedDomainEvent(Guid BookingId) : IDomainEvent;
