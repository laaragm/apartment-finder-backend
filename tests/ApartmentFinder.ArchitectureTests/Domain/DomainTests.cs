using FluentAssertions;
using NetArchTest.Rules;
using ApartmentFinder.Domain.Abstractions;

namespace ApartmentFinder.ArchitectureTests.Domain;

public class DomainTests : BaseTest
{
	[Fact]
	public void Domain_Event_Should_Have_DomainEvent_Postfix()
	{
		var result = Types.InAssembly(DomainAssembly)
			.That()
			.ImplementInterface(typeof(IDomainEvent))
			.Should().HaveNameEndingWith("DomainEvent")
			.GetResult();

		result.IsSuccessful.Should().BeTrue();
	}
}
