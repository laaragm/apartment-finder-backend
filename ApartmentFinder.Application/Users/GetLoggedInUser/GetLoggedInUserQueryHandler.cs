using Dapper;
using ApartmentFinder.Domain.Abstractions;
using ApartmentFinder.Application.Abstractions.Data;
using ApartmentFinder.Application.Abstractions.Messaging;
using ApartmentFinder.Application.Abstractions.Authentication;

namespace ApartmentFinder.Application.Users.GetLoggedInUser;

internal sealed class GetLoggedInUserQueryHandler : IQueryHandler<GetLoggedInUserQuery, UserResponse>
{
	private readonly ISqlConnectionFactory _sqlConnectionFactory;
	private readonly IUserContext _userContext;

	public GetLoggedInUserQueryHandler(ISqlConnectionFactory sqlConnectionFactory, IUserContext userContext)
	{
		_sqlConnectionFactory = sqlConnectionFactory;
		_userContext = userContext;
	}

	public async Task<Result<UserResponse>> Handle(GetLoggedInUserQuery request, CancellationToken cancellationToken)
	{
		using var connection = _sqlConnectionFactory.CreateConnection();
		const string sql = """
            SELECT
                id AS Id,
                name AS Name,
                email AS Email
            FROM users
            WHERE identity_id = @IdentityId
            """;
		var user = await connection.QuerySingleAsync<UserResponse>(sql, new { _userContext.IdentityId });

		return user;
	}
}
