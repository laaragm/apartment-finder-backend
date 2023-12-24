using ApartmentFinder.Application.Abstractions.Messaging;

namespace ApartmentFinder.Application.Bookings.CancelBooking;

public sealed record CancelBookingCommand(Guid BookingId) : ICommand;
