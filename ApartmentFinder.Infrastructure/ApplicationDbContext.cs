using Microsoft.EntityFrameworkCore;
using ApartmentFinder.Domain.Abstractions;

namespace ApartmentFinder.Infrastructure;

public sealed class ApplicationDbContext : DbContext, IUnitOfWork
{
	public ApplicationDbContext(DbContextOptions options) : base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		// Configure the EF data model by scanning the assembly for entity configurations
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
		base.OnModelCreating(modelBuilder);
	}
}
