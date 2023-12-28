using FluentAssertions;
using ApartmentFinder.Domain.Users;
using ApartmentFinder.Domain.Users.Events;

namespace ApartmentFinder.Domain.UnitTests.Users;

public class RegisterUserTests : BaseTest
{
	[Fact]
	public void Create_Should_Raise_UserCreatedDomainEvent()
	{
		// Arrange
		var firstName = new FirstName("first");
		var lastName = new LastName("last");
		var email = new Email("test@test.com");

		// Act
		var user = User.Create(firstName, lastName, email);

		// Assert
		var userCreatedDomainEvent = AssertDomainEventWasPublished<UserCreatedDomainEvent>(user);

		userCreatedDomainEvent.UserId.Should().Be(user.Id);
	}
}

