using ApartmentFinder.Domain.Apartments;

namespace ApartmentFinder.Infrastructure.Repositories;

internal sealed class ApartmentRepository : Repository<Apartment, ApartmentId>, IApartmentRepository
{
	public ApartmentRepository(ApplicationDbContext dbContext) : base(dbContext)
	{
	}
}