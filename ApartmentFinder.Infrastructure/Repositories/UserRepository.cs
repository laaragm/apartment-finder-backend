﻿using ApartmentFinder.Domain.Users;

namespace ApartmentFinder.Infrastructure.Repositories;

internal sealed class UserRepository : Repository<User, UserId>, IUserRepository
{
	public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
	{
	}
}
