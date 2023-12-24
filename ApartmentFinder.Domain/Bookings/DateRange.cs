namespace ApartmentFinder.Domain.Bookings;

public record DateRange
{
	public DateOnly Start { get; init; }
	public DateOnly End { get; init; }
	public int LengthInDays => End.DayNumber - Start.DayNumber;

	private DateRange() { }

	public static DateRange Create(DateOnly start, DateOnly end)
	{
		if (start > end)
		{
			throw new ApplicationException("The start date (" + start + ") cannot be later than the end date (" + end + ")");
		}

		return new DateRange { Start = start, End = end };
	}
}
