﻿using FluentAssertions;
using ApartmentFinder.Domain.Users;
using ApartmentFinder.Domain.Shared;
using ApartmentFinder.Domain.Bookings;
using ApartmentFinder.Domain.Apartments;
using ApartmentFinder.Domain.Bookings.Events;
using ApartmentFinder.Domain.UnitTests.Infrastructure;

namespace ApartmentFinder.Domain.UnitTests.Bookings;

public class CancelBookingTests : BaseTest
{
	private Apartment CreateApartment()
	{
		return new Apartment(
			ApartmentId.New(),
			new Name("Apartment 1"),
			new Description("Description here"),
			new Address("England", "London", "EC1A 1AE", "London", "King Edward St"),
			new Money(1000, Currency.Eur),
			new Money(50, Currency.Eur),
			new List<Amenity>());
	}

	private Booking BookApartment(DateTime startDate, DateTime endDate)
	{
		var apartment = CreateApartment();
		return Booking.Reserve(
			apartment,
			UserId.New(),
			DateRange.Create(DateOnly.FromDateTime(startDate), DateOnly.FromDateTime(endDate)),
			DateTime.UtcNow,
			new PricingService());
	}

	[Fact]
	public void Cancel_Should_Raise_BookingCancelledDomainEvent()
	{
		// Arrange
		var booking = BookApartment(new DateTime(2024, 10, 12), new DateTime(2024, 10, 18));
		booking.Confirm(DateTime.UtcNow);

		// Act
		booking.Cancel(DateTime.UtcNow);

		// Assert
		var bookingCancelledDomainEvent = AssertDomainEventWasPublished<BookingCancelledDomainEvent>(booking);

		bookingCancelledDomainEvent.BookingId.Should().Be(booking.Id);
	}

	[Fact]
	public void Cancel_Before_Confirmation_Should_Not_Be_Allowed()
	{
		// Arrange
		var booking = BookApartment(new DateTime(2024, 10, 12), new DateTime(2024, 10, 18));

		// Act
		var result = booking.Cancel(DateTime.UtcNow);

		// Assert
		result.IsFailure.Should().BeTrue();
	}

	[Fact]
	public void Cancel_After_Start_Date_Should_Not_Be_Allowed()
	{
		// Arrange
		var booking = BookApartment(new DateTime(2023, 05, 10), new DateTime(2024, 06, 10));

		// Act
		var result = booking.Cancel(DateTime.UtcNow);

		// Assert
		result.IsFailure.Should().BeTrue();
	}
}
