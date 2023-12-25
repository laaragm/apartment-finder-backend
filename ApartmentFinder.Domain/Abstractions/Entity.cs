﻿namespace ApartmentFinder.Domain.Abstractions;

public abstract class Entity
{
	public Guid Id { get; init; } // the init accessor will ensure that once the entity has been defined, its ID will be set and remain unchanged for its lifetime
	private readonly List<IDomainEvent> _domainEvents = new();

	protected Entity(Guid id)
	{
		Id = id;
	}

	protected Entity() { }

	public IReadOnlyList<IDomainEvent> GetDomainEvents()
	{
		return _domainEvents.ToList();
	}

	public void ClearDomainEvents()
	{
		_domainEvents.Clear();
	}

	protected void RaiseDomainEvent(IDomainEvent domainEvent)
	{
		_domainEvents.Add(domainEvent);
	}
}
