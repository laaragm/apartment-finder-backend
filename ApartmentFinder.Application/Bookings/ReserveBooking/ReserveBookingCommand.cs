using ApartmentFinder.Application.Abstractions.Messaging;

namespace ApartmentFinder.Application.Bookings.ReserveBooking;

public record ReserveBookingCommand(Guid ApartmentId, Guid UserId, DateOnly StartDate, DateOnly EndDate) : ICommand<Guid>;
