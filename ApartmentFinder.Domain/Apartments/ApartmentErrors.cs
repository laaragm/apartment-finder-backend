using ApartmentFinder.Domain.Abstractions;

namespace ApartmentFinder.Domain.Apartments;

public static class ApartmentErrors
{
	public static Error NotFound = new("Property.NotFound", "The property with the specified id was not found");
}
