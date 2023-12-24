using ApartmentFinder.Application.Abstractions.Messaging;

namespace ApartmentFinder.Application.Reviews.AddReview;

public sealed record AddReviewCommand(Guid BookingId, int Rating, string Comment) : ICommand;
