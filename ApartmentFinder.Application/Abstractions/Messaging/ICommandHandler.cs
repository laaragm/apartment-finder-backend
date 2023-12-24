using MediatR;
using ApartmentFinder.Domain.Abstractions;

namespace ApartmentFinder.Application.Abstractions.Messaging;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result> where TCommand : ICommand
{
}

public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>> where TCommand : ICommand<TResponse>
{
}