using System.Reflection;
using ApartmentFinder.Infrastructure;
using ApartmentFinder.Domain.Abstractions;
using ApartmentFinder.Application.Abstractions.Messaging;

namespace ApartmentFinder.ArchitectureTests;


public class BaseTest
{
	protected static Assembly ApplicationAssembly => typeof(IBaseCommand).Assembly;
	protected static Assembly DomainAssembly => typeof(IEntity).Assembly;
	protected static Assembly InfrastructureAssembly => typeof(ApplicationDbContext).Assembly;
}
