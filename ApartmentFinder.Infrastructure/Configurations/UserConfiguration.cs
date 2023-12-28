using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ApartmentFinder.Domain.Users;

namespace ApartmentFinder.Infrastructure.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.ToTable("users");

		builder.HasKey(user => user.Id);

		builder.Property(user => user.Id)
			.HasConversion(userId => userId.Value, value => new UserId(value));

		builder.Property(user => user.FirstName)
			.HasMaxLength(200)
			.HasConversion(firstName => firstName.Value, value => new FirstName(value));

		builder.Property(user => user.LastName)
			.HasMaxLength(200)
			.HasConversion(firstName => firstName.Value, value => new LastName(value));

		builder.Property(user => user.Email)
			.HasMaxLength(400)
			.HasConversion(email => email.Value, value => new Domain.Users.Email(value));

		// Ensures a unique constraint in the database for the column in the current table
		builder.HasIndex(user => user.Email).IsUnique();

		builder.HasIndex(user => user.IdentityId).IsUnique();
	}
}
