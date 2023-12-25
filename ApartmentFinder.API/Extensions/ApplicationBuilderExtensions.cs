using Microsoft.EntityFrameworkCore;
using ApartmentFinder.Infrastructure;
using ApartmentFinder.API.Middlewares;

namespace ApartmentFinder.API.Extensions;

public static class ApplicationBuilderExtensions
{
	public static void ApplyMigrations(this IApplicationBuilder app)
	{
		using var scope = app.ApplicationServices.CreateScope();
		using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
		dbContext.Database.Migrate();
	}

	public static void UseCustomExceptionHandler(this IApplicationBuilder app)
	{
		app.UseMiddleware<ExceptionHandlingMiddleware>();
	}
}
