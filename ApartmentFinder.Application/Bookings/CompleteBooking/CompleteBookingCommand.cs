using ApartmentFinder.Application.Abstractions.Messaging;

namespace ApartmentFinder.Application.Bookings.CompleteBooking;

public sealed record CompleteBookingCommand(Guid BookingId) : ICommand;
