using System.Data;

namespace ApartmentFinder.Application.Abstractions.Data;

public interface ISqlConnectionFactory
{
	IDbConnection CreateConnection();
}
