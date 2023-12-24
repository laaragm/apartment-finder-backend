using ApartmentFinder.Application.Abstractions.Messaging;

namespace ApartmentFinder.Application.Bookings.ConfirmBooking;

public sealed record ConfirmBookingCommand(Guid BookingId) : ICommand;
