﻿using ApartmentFinder.Domain.Users;
using ApartmentFinder.Domain.Abstractions;
using ApartmentFinder.Application.Abstractions.Messaging;
using ApartmentFinder.Application.Abstractions.Authentication;

namespace ApartmentFinder.Application.Users.RegisterUser;

public sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, Guid>
{
	private readonly IAuthenticationService _authenticationService;
	private readonly IUserRepository _userRepository;
	private readonly IUnitOfWork _unitOfWork;

	public RegisterUserCommandHandler(IAuthenticationService authenticationService, IUserRepository userRepository, IUnitOfWork unitOfWork)
	{
		_authenticationService = authenticationService;
		_userRepository = userRepository;
		_unitOfWork = unitOfWork;
	}

	public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
	{
		var user = User.Create(new FirstName(request.FirstName), new LastName(request.LastName), new Email(request.Email));
		var identityId = await _authenticationService.RegisterAsync(user, request.Password, cancellationToken);
		user.SetIdentityId(identityId);
		_userRepository.Add(user);
		await _unitOfWork.SaveChangesAsync();

		return user.Id.Value;
	}
}
