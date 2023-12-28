using ApartmentFinder.Application;
using ApartmentFinder.Infrastructure;
using ApartmentFinder.API.Extensions;
using ApartmentFinder.API.Endpoints.Users;
using ApartmentFinder.API.Endpoints.Reviews;
using ApartmentFinder.API.Endpoints.Bookings;
using ApartmentFinder.API.Endpoints.Apartments;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
	app.ApplyMigrations();
	// app.SeedData();
}

app.UseHttpsRedirection();

app.UseCustomExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.MapApartmentsEndpoints();
app.MapBookingsEndpoints();
app.MapReviewsEndpoints();
app.MapUsersEndpoints();

app.Run();
