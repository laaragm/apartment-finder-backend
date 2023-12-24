using ApartmentFinder.Application.Abstractions.Clock;

namespace ApartmentFinder.Infrastructure.Clock;

internal sealed class DateTimeProvider : IDateTimeProvider
{
	public DateTime UtcNow => DateTime.UtcNow;
}