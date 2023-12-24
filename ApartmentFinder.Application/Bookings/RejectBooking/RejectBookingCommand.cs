using ApartmentFinder.Application.Abstractions.Messaging;

namespace ApartmentFinder.Application.Bookings.RejectBooking;

public sealed record RejectBookingCommand(Guid BookingId) : ICommand;
