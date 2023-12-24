using FluentValidation;

namespace ApartmentFinder.Application.Bookings.ReserveBooking;

// The validator class needs to implement AbstractValidator<T> where T is the type that will be validated
public class ReserveBookingCommandValidator : AbstractValidator<ReserveBookingCommand>
{
	public ReserveBookingCommandValidator()
	{
		RuleFor(c => c.UserId).NotEmpty();
		RuleFor(c => c.ApartmentId).NotEmpty();
		RuleFor(c => c.StartDate).LessThan(c => c.EndDate);
	}
}
