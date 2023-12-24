﻿namespace ApartmentFinder.Application.Abstractions.Email;

public interface IEmailService
{
	Task SendAsync(Domain.Users.Email recipient, string subject, string body);
}
