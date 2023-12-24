using ApartmentFinder.Domain.Abstractions;
using ApartmentFinder.Domain.Shared;

namespace ApartmentFinder.Domain.Apartments;

public sealed class Apartment : Entity
{
	public Name Name { get; private set; } // The private setter is used because we don't want to allow the property to be changed outside the scope of the entity
	public Description Description { get; private set; }
	public Address Address { get; private set; }
	public Money Price { get; private set; }
	public Money CleaningFee { get; private set; }
	public DateTime? LastBookedOnUtc { get; internal set; } // We can only set the value of this property in the domain project
	public List<Amenity> Amenities { get; private set; } = new();

	public Apartment(Guid id, Name name, Description description, Address address, Money price, Money cleaningFee, List<Amenity> amenities) : base(id) 
	{
		Name = name;
		Description = description;
		Address = address;
		Price = price;
		CleaningFee = cleaningFee;
		Amenities = amenities;
	}
}
