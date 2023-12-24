using ApartmentFinder.Domain.Abstractions;

namespace ApartmentFinder.Domain.Bookings.Events;

public sealed record BookingConfirmedDomainEvent(Guid BookingId) : IDomainEvent;
