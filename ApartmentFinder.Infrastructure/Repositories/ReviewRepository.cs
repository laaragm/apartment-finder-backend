using ApartmentFinder.Domain.Reviews;

namespace ApartmentFinder.Infrastructure.Repositories;

internal sealed class ReviewRepository : Repository<Review, ReviewId>, IReviewRepository
{
	public ReviewRepository(ApplicationDbContext dbContext) : base(dbContext)
	{
	}
}
