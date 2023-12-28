﻿using ApartmentFinder.Domain.Users;
using ApartmentFinder.Domain.Bookings;
using ApartmentFinder.Domain.Abstractions;
using ApartmentFinder.Domain.Reviews.Events;
using ApartmentFinder.Domain.Apartments;

namespace ApartmentFinder.Domain.Reviews;

public sealed class Review : Entity<ReviewId>
{
	public ApartmentId ApartmentId { get; private set; }
	public BookingId BookingId { get; private set; }
	public UserId UserId { get; private set; }
	public Rating Rating { get; private set; }
	public Comment Comment { get; private set; }
	public DateTime CreatedOnUtc { get; private set; }

	private Review(ReviewId id, ApartmentId apartmentId, BookingId bookingId, UserId userId, Rating rating, Comment comment, DateTime createdOnUtc) : base(id)
	{
		ApartmentId = apartmentId;
		BookingId = bookingId;
		UserId = userId;
		Rating = rating;
		Comment = comment;
		CreatedOnUtc = createdOnUtc;
	}

	private Review() { }

	public static Result<Review> Create(Booking booking, Rating rating, Comment comment, DateTime createdOnUtc)
	{
		var bookingHasBeenCompleted = booking.Status == BookingStatus.Completed;
		if (!bookingHasBeenCompleted)
		{
			return Result.Failure<Review>(ReviewErrors.NotEligible);
		}

		var review = new Review(ReviewId.New(), booking.ApartmentId, booking.Id, booking.UserId, rating, comment, createdOnUtc);
		review.RaiseDomainEvent(new ReviewCreatedDomainEvent(review.Id));

		return review;
	}
}
