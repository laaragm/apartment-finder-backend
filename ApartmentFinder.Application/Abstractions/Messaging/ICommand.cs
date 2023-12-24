using MediatR;
using ApartmentFinder.Domain.Abstractions;

namespace ApartmentFinder.Application.Abstractions.Messaging;

// We want to enforce that all of our commands return a result, either as a generic or a specific type
public interface ICommand : IRequest<Result>, IBaseCommand
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand
{
}

// The value behind having a base command interface is that we can apply generic constraints in our pipeline behaviors
public interface IBaseCommand
{
}