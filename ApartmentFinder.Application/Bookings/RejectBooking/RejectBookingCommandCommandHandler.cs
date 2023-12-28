using ApartmentFinder.Application.Abstractions.Clock;
using ApartmentFinder.Application.Abstractions.Messaging;
using ApartmentFinder.Domain.Abstractions;
using ApartmentFinder.Domain.Bookings;

namespace ApartmentFinder.Application.Bookings.RejectBooking;

public sealed class RejectBookingCommandCommandHandler : ICommandHandler<RejectBookingCommand>
{
	private readonly IDateTimeProvider _dateTimeProvider;
	private readonly IBookingRepository _bookingRepository;
	private readonly IUnitOfWork _unitOfWork;

	public RejectBookingCommandCommandHandler(IDateTimeProvider dateTimeProvider, IBookingRepository bookingRepository, IUnitOfWork unitOfWork)
	{
		_bookingRepository = bookingRepository;
		_unitOfWork = unitOfWork;
		_dateTimeProvider = dateTimeProvider;
	}

	public async Task<Result> Handle(RejectBookingCommand request, CancellationToken cancellationToken)
	{
		var booking = await _bookingRepository.GetByIdAsync(new BookingId(request.BookingId), cancellationToken);
		if (booking is null)
		{
			return Result.Failure(BookingErrors.NotFound);
		}

		var result = booking.Reject(_dateTimeProvider.UtcNow);
		if (result.IsFailure)
		{
			return result;
		}

		await _unitOfWork.SaveChangesAsync(cancellationToken);

		return Result.Success();
	}
}
