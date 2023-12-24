﻿using ApartmentFinder.Application.Abstractions.Clock;
using ApartmentFinder.Application.Abstractions.Data;
using ApartmentFinder.Domain.Abstractions;
using ApartmentFinder.Domain.Apartments;
using ApartmentFinder.Domain.Bookings;
using ApartmentFinder.Domain.Reviews;
using ApartmentFinder.Domain.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ApartmentFinder.Infrastructure.Clock;
using ApartmentFinder.Infrastructure.Email;
using ApartmentFinder.Application.Abstractions.Email;
using Microsoft.EntityFrameworkCore;
using Dapper;

namespace ApartmentFinder.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddTransient<IDateTimeProvider, DateTimeProvider>();
		services.AddTransient<IEmailService, EmailService>();

		AddPersistence(services, configuration);

		return services;
	}

	private static void AddPersistence(IServiceCollection services, IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString("Database") ?? throw new ArgumentNullException(nameof(configuration));

		services.AddDbContext<ApplicationDbContext>(options =>
		{
			options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
		});

		//services.AddScoped<IUserRepository, UserRepository>();
		//services.AddScoped<IApartmentRepository, ApartmentRepository>();
		//services.AddScoped<IBookingRepository, BookingRepository>();
		//services.AddScoped<IReviewRepository, ReviewRepository>();

		services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

		//services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));

		//SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
	}
}
