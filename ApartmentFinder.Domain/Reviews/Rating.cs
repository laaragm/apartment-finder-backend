using ApartmentFinder.Domain.Abstractions;

namespace ApartmentFinder.Domain.Reviews;

public sealed record Rating
{
	private static int MinValue = 1;
	private static int MaxValue = 5;
	public int Value { get; init; }
	public static readonly Error Invalid = new("Rating.Invalid", "The rating is invalid");

	private Rating(int value) => Value = value;

	public static Result<Rating> Create(int value)
	{
		if (value < MinValue || value > MaxValue)
		{
			return Result.Failure<Rating>(Invalid);
		}

		return new Rating(value);
	}
}
