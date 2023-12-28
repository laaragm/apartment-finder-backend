using ApartmentFinder.Domain.Abstractions;

namespace ApartmentFinder.Domain.Bookings.Events;

public sealed record BookingRejectedDomainEvent(BookingId BookingId) : IDomainEvent;
