using Dapper;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ApartmentFinder.Domain.Users;
using ApartmentFinder.Domain.Reviews;
using ApartmentFinder.Domain.Bookings;
using ApartmentFinder.Domain.Apartments;
using ApartmentFinder.Domain.Abstractions;
using ApartmentFinder.Infrastructure.Data;
using ApartmentFinder.Infrastructure.Clock;
using ApartmentFinder.Infrastructure.Email;
using ApartmentFinder.Infrastructure.Repositories;
using ApartmentFinder.Application.Abstractions.Data;
using ApartmentFinder.Infrastructure.Authentication;
using ApartmentFinder.Application.Abstractions.Email;
using ApartmentFinder.Application.Abstractions.Clock;
using ApartmentFinder.Application.Abstractions.Authentication;
using ApartmentFinder.Infrastructure.Outbox;
using Quartz;

namespace ApartmentFinder.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddTransient<IDateTimeProvider, DateTimeProvider>();
		services.AddTransient<IEmailService, EmailService>();

		AddPersistence(services, configuration);
		AddAuthentication(services, configuration);
		AddBackgroundJobs(services, configuration);

		return services;
	}

	private static void AddPersistence(IServiceCollection services, IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString("Database") ?? throw new ArgumentNullException(nameof(configuration));

		services.AddDbContext<ApplicationDbContext>(options =>
		{
			options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
		});

		services.AddScoped<IUserRepository, UserRepository>();
		services.AddScoped<IApartmentRepository, ApartmentRepository>();
		services.AddScoped<IBookingRepository, BookingRepository>();
		services.AddScoped<IReviewRepository, ReviewRepository>();

		services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

		services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));

		SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
	}

	private static void AddAuthentication(IServiceCollection services, IConfiguration configuration)
	{
		services
			.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer();

		services.Configure<AuthenticationOptions>(configuration.GetSection("Authentication"));

		services.ConfigureOptions<JwtBearerOptionsSetup>();

		services.Configure<KeycloakOptions>(configuration.GetSection("Keycloak"));

		services.AddTransient<AdminAuthorizationDelegatingHandler>();

		services.AddHttpClient<IAuthenticationService, AuthenticationService>((serviceProvider, httpClient) =>
		{
			var keycloakOptions = serviceProvider.GetRequiredService<IOptions<KeycloakOptions>>().Value;
			httpClient.BaseAddress = new Uri(keycloakOptions.AdminUrl);
		})
		.AddHttpMessageHandler<AdminAuthorizationDelegatingHandler>();

		services.AddHttpClient<IJwtService, JwtService>((serviceProvider, httpClient) =>
		{
			var keycloakOptions = serviceProvider.GetRequiredService<IOptions<KeycloakOptions>>().Value;
			httpClient.BaseAddress = new Uri(keycloakOptions.TokenUrl);
		});

		services.AddHttpContextAccessor();

		services.AddScoped<IUserContext, UserContext>();
	}

	private static void AddBackgroundJobs(IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<OutboxOptions>(configuration.GetSection("Outbox"));

		// Utilize built-in DI support for resolving job instances, enabling the use of scoped services within jobs as needed
		services.AddQuartz(options => { options.UseMicrosoftDependencyInjectionJobFactory(); });

		// Add a hosted service to start Quartz in the background, initiating the triggering of background jobs
		services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

		services.ConfigureOptions<ProcessOutboxMessagesJobSetup>();
	}
}
