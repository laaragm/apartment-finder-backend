using Microsoft.EntityFrameworkCore;
using ApartmentFinder.Infrastructure;

namespace ApartmentFinder.API.Extensions;

public static class ApplicationBuilderExtensions
{
	public static void ApplyMigrations(this IApplicationBuilder app)
	{
		using var scope = app.ApplicationServices.CreateScope();
		using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
		dbContext.Database.Migrate();
	}
}
