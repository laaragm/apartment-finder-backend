using ApartmentFinder.Domain.Apartments;

namespace ApartmentFinder.Domain.Bookings;

public interface IBookingRepository
{
	Task<Booking?> GetByIdAsync(BookingId id, CancellationToken cancellationToken = default);
	Task<bool> IsOverlappingAsync(Apartment apartment, DateRange duration, CancellationToken cancellationToken = default);
	void Add(Booking booking);
}
