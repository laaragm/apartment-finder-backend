using ApartmentFinder.Domain.Abstractions;
using ApartmentFinder.Domain.Users;
using ApartmentFinder.Domain.Users.Events;

namespace ApartmentFinder.Domain.User;

public sealed class User : Entity
{
	public FirstName FirstName { get; private set; }
	public LastName LastName { get; private set; }
	public Email Email { get; private set; }

	private User(Guid id, FirstName firstName, LastName lastName, Email email) : base(id) 
	{
		FirstName = firstName;
		LastName = lastName;
		Email = email;
	}

	// The purpose of wrapping the constructor in a factory method is to hide the constructor for better encapsulation
	// Additionally, this allows for the introduction of side effects in the factory method that wouldn't be appropriate in a constructor (e.g: domain events)
	public static User Create(FirstName firstName, LastName lastName, Email email)
	{
		var user = new User(Guid.NewGuid(), firstName, lastName, email);
		user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));

		return user;
	}
}
