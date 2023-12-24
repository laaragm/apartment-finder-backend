using Dapper;
using System.Data;

namespace ApartmentFinder.Infrastructure.Data;

// Define a custom Dapper type handler for the DateOnly type since it's not natively supported
internal sealed class DateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
{
	public override DateOnly Parse(object value) => DateOnly.FromDateTime((DateTime)value);

	public override void SetValue(IDbDataParameter parameter, DateOnly value)
	{
		parameter.DbType = DbType.Date;
		parameter.Value = value;
	}
}
