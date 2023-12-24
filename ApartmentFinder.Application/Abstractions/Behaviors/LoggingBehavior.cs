using MediatR;
using Microsoft.Extensions.Logging;
using ApartmentFinder.Application.Abstractions.Messaging;

namespace ApartmentFinder.Application.Abstractions.Behaviors;

// Logging pipeline behavior: responsible for logging information before and after executing a command within the application
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IBaseCommand
{
	private readonly ILogger<TRequest> _logger;

	public LoggingBehavior(ILogger<TRequest> logger)
	{
		_logger = logger;
	}

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		var name = request.GetType().Name; // Get the name of the current request (command) using reflection
		try
		{
			_logger.LogInformation("Executing command {Command}", name);

			var result = await next(); // Execute the request handler delegate, which is the command handler

			_logger.LogInformation("Command {Command} processed successfully", name);

			return result;
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, "Command {Command} processing failed", name);
			throw;
		}
	}
}
