using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ApartmentFinder.Infrastructure.Outbox;

namespace ApartmentFinder.Infrastructure.Configurations;

internal sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
	public void Configure(EntityTypeBuilder<OutboxMessage> builder)
	{
		builder.ToTable("outbox_messages");

		builder.HasKey(outboxMessage => outboxMessage.Id);

		builder.Property(outboxMessage => outboxMessage.Content).HasColumnType("json");
	}
}
