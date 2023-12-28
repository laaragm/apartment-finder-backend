using Microsoft.EntityFrameworkCore;
using ApartmentFinder.Domain.Abstractions;
using MediatR;
using ApartmentFinder.Application.Exceptions;

namespace ApartmentFinder.Infrastructure;

public sealed class ApplicationDbContext : DbContext, IUnitOfWork
{
	private readonly IPublisher _publisher;

	public ApplicationDbContext(DbContextOptions options, IPublisher publisher) : base(options)
	{
		_publisher = publisher;
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		// Configure the EF data model by scanning the assembly for entity configurations
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
		base.OnModelCreating(modelBuilder);
	}

	public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		try
		{
			var result = await base.SaveChangesAsync(cancellationToken);
			await PublishDomainEventsAsync();
			return result;
		} 
		catch (DbUpdateConcurrencyException exception) // This exception is thrown by the database when there's a concurrency violation at the database level
		{
			// Wrap the EF Core specific exception in a custom exception for abstraction
			throw new ConcurrencyException("Concurrency exception occurred", exception);
		}
	}

	private async Task PublishDomainEventsAsync()
	{
		// Get all domain events from entities that have raised them
		var domainEvents = ChangeTracker
			.Entries<IEntity>()
			.Select(entry => entry.Entity)
			.SelectMany(entity =>
			{
				// Get and clear domain events from the entity
				var domainEvents = entity.GetDomainEvents();
				entity.ClearDomainEvents();
				return domainEvents;
			})
			.ToList();

		// Publish each domain event in order to trigger the respective domain event handlers defined in the application layer
		foreach (var domainEvent in domainEvents)
		{
			await _publisher.Publish(domainEvent);
		}
	}
}
