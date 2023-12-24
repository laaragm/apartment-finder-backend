using ApartmentFinder.Domain.Shared;

namespace ApartmentFinder.Domain.Bookings;

public record PricingDetails(
	Money PriceForPeriod,
	Money CleaningFee,
	Money AmenitiesUpCharge,
	Money TotalPrice);
