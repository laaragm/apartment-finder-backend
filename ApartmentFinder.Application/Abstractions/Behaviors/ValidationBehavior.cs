using MediatR;
using FluentValidation;
using ApartmentFinder.Application.Exceptions;
using ApartmentFinder.Application.Abstractions.Messaging;

namespace ApartmentFinder.Application.Abstractions.Behaviors;

// Validation pipeline behavior
// In this case, the request has to be a base command because we only want to run validation for our commands
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IBaseCommand 
{
	private readonly IEnumerable<IValidator<TRequest>> _validators; // Inject one or more validators for the command

	public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
	{
		_validators = validators;
	}

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		// If no validators are available, proceed with execution
		if (!_validators.Any())
		{
			return await next();
		}

		var context = new ValidationContext<TRequest>(request);

		// Perform validation for each validator and collect validation errors
		var validationErrors = _validators
			.Select(validator => validator.Validate(context))
			.Where(validationResult => validationResult.Errors.Any())
			.SelectMany(validationResult => validationResult.Errors)
			.Select(validationFailure => new ValidationError(validationFailure.PropertyName, validationFailure.ErrorMessage)) // Project the errors into a new ValidationError instance
			.ToList();

		if (validationErrors.Any())
		{
			throw new Exceptions.ValidationException(validationErrors);
		}

		// Execute the command if there are no validation errors
		return await next();
	}
}
