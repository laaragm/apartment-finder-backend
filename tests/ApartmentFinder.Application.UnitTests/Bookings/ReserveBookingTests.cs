using Moq;
using FluentAssertions;
using ApartmentFinder.Domain.Users;
using ApartmentFinder.Domain.Bookings;
using ApartmentFinder.Domain.Apartments;
using ApartmentFinder.Domain.Abstractions;
using ApartmentFinder.Application.Abstractions.Clock;
using ApartmentFinder.Application.Bookings.ReserveBooking;

namespace ApartmentFinder.Application.UnitTests.Bookings;

public class ReserveBookingTests
{
	private static readonly User User = User.Create(new FirstName("test"), new LastName("test"), new Email("test@test.com"));

	[Fact]
	public async Task Handle_Should_Return_Failure_When_User_Is_Null()
	{
		// Arrange
		var command = new ReserveBookingCommand(Guid.NewGuid(), Guid.NewGuid(), DateOnly.Parse("01-01-2023"), DateOnly.Parse("10-01-2023"));

		var userRepositoryMock = new Mock<IUserRepository>();
		userRepositoryMock
			.Setup(u => u.GetByIdAsync(It.IsAny<UserId>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync((User?)null);

		var handler = new ReserveBookingCommandHandler(
			userRepositoryMock.Object,
			new Mock<IApartmentRepository>().Object,
			new Mock<IBookingRepository>().Object,
			new Mock<IUnitOfWork>().Object,
			new Mock<PricingService>().Object,
			new Mock<IDateTimeProvider>().Object);

		// Act
		var result = await handler.Handle(command, default);

		// Assert
		result.Error.Should().Be(UserErrors.NotFound);
	}

	[Fact]
	public async Task Handle_Should_Return_Failure_When_Apartment_Is_Null()
	{
		// Arrange
		var command = new ReserveBookingCommand(Guid.NewGuid(), Guid.NewGuid(), DateOnly.Parse("01-01-2023"), DateOnly.Parse("10-01-2023"));

		var userRepositoryMock = new Mock<IUserRepository>();
		userRepositoryMock
			.Setup(u => u.GetByIdAsync(It.IsAny<UserId>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(User);

		var apartmentRepositoryMock = new Mock<IApartmentRepository>();
		apartmentRepositoryMock
			.Setup(u => u.GetByIdAsync(It.IsAny<ApartmentId>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync((Apartment?)null);


		var handler = new ReserveBookingCommandHandler(
			userRepositoryMock.Object,
			apartmentRepositoryMock.Object,
			new Mock<IBookingRepository>().Object,
			new Mock<IUnitOfWork>().Object,
			new Mock<PricingService>().Object,
			new Mock<IDateTimeProvider>().Object);

		// Act
		var result = await handler.Handle(command, default);

		// Assert
		result.Error.Should().Be(ApartmentErrors.NotFound);
	}
}
