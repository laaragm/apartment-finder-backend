namespace ApartmentFinder.Domain.Apartments;

// A record is a good choice for implementing a Value object because of immutability and value-based equality
public record Address(
	string Country,
	string State,
	string ZipCode,
	string City,
	string Street);
