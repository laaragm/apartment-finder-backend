﻿using ApartmentFinder.Domain.Shared;
using ApartmentFinder.Domain.Apartments;
using ApartmentFinder.Domain.Abstractions;
using ApartmentFinder.Domain.Bookings.Events;

namespace ApartmentFinder.Domain.Bookings;

public sealed class Booking : Entity
{
	public Guid ApartmentId { get; private set; }
	public Guid UserId { get; private set; }
	public DateRange Duration { get; private set; }
	public Money PriceForPeriod { get; private set; }
	public Money CleaningFee { get; private set; }
	public Money AmenitiesUpCharge { get; private set; }
	public Money TotalPrice { get; private set; }
	public BookingStatus Status { get; private set; }
	public DateTime CreatedOnUtc { get; private set; }
	public DateTime? ConfirmedOnUtc { get; private set; }
	public DateTime? RejectedOnUtc { get; private set; }
	public DateTime? CompletedOnUtc { get; private set; }
	public DateTime? CancelledOnUtc { get; private set; }

	private Booking(
		Guid id,
		Guid apartmentId,
		Guid userId,
		DateRange duration,
		Money priceForPeriod,
		Money cleaningFee,
		Money amenitiesUpCharge,
		Money totalPrice,
		BookingStatus status,
		DateTime createdOnUtc)
		: base(id)
	{
		ApartmentId = apartmentId;
		UserId = userId;
		Duration = duration;
		PriceForPeriod = priceForPeriod;
		CleaningFee = cleaningFee;
		AmenitiesUpCharge = amenitiesUpCharge;
		TotalPrice = totalPrice;
		Status = status;
		CreatedOnUtc = createdOnUtc;
	}

	private Booking() { }

	public static Booking Reserve(Apartment apartment, Guid userId, DateRange duration, DateTime utcNow, PricingService pricingService)
	{
		var pricingDetails = pricingService.CalculatePrice(apartment, duration);
		var booking = new Booking(
			Guid.NewGuid(),
			apartment.Id,
			userId,
			duration,
			pricingDetails.PriceForPeriod,
			pricingDetails.CleaningFee,
			pricingDetails.AmenitiesUpCharge,
			pricingDetails.TotalPrice,
			BookingStatus.Reserved,
			utcNow);
		booking.RaiseDomainEvent(new BookingReservedDomainEvent(booking.Id));
		apartment.LastBookedOnUtc = utcNow;

		return booking;
	}

	public Result Confirm(DateTime utcNow)
	{
		var propertyHasBeenReserved = Status == BookingStatus.Reserved;
		if (!propertyHasBeenReserved)
		{
			return Result.Failure(BookingErrors.NotReserved);
		}

		Status = BookingStatus.Confirmed;
		ConfirmedOnUtc = utcNow;
		RaiseDomainEvent(new BookingConfirmedDomainEvent(Id));

		return Result.Success();
	}

	public Result Complete(DateTime utcNow)
	{
		var bookingHasBeenConfirmed = Status == BookingStatus.Confirmed;
		if (!bookingHasBeenConfirmed)
		{
			return Result.Failure(BookingErrors.NotConfirmed);
		}

		Status = BookingStatus.Completed;
		CompletedOnUtc = utcNow;
		RaiseDomainEvent(new BookingCompletedDomainEvent(Id));

		return Result.Success();
	}

	public Result Reject(DateTime utcNow)
	{
		var propertyHasBeenReserved = Status == BookingStatus.Reserved;
		if (!propertyHasBeenReserved)
		{
			return Result.Failure(BookingErrors.NotReserved);
		}

		Status = BookingStatus.Rejected;
		RejectedOnUtc = utcNow;
		RaiseDomainEvent(new BookingRejectedDomainEvent(Id));

		return Result.Success();
	}

	public Result Cancel(DateTime utcNow)
	{
		var bookingHasBeenConfirmed = Status == BookingStatus.Confirmed;
		if (!bookingHasBeenConfirmed)
		{
			return Result.Failure(BookingErrors.NotConfirmed);
		}

		var currentDate = DateOnly.FromDateTime(utcNow);
		var bookingPeriodAlreadyStarted = currentDate > Duration.Start;
		if (bookingPeriodAlreadyStarted)
		{
			return Result.Failure(BookingErrors.AlreadyStarted);
		}

		Status = BookingStatus.Cancelled;
		CancelledOnUtc = utcNow;
		RaiseDomainEvent(new BookingCancelledDomainEvent(Id));

		return Result.Success();
	}
}
