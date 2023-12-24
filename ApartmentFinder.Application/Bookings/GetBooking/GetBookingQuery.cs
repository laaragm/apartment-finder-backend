using ApartmentFinder.Application.Abstractions.Messaging;

namespace ApartmentFinder.Application.Bookings.GetBooking;

public sealed record GetBookingQuery(Guid BookingId) : IQuery<BookingResponse>;
