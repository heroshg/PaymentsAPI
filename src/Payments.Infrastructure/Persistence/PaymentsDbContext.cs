using Microsoft.EntityFrameworkCore;

namespace Payments.Infrastructure.Persistence;

public class PaymentsDbContext(DbContextOptions<PaymentsDbContext> options) : DbContext(options)
{
    public DbSet<PaymentEventRecord> PaymentEvents => Set<PaymentEventRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PaymentEventRecord>(entity =>
        {
            entity.ToTable("payment_events");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.AggregateId).IsRequired();
            entity.Property(e => e.EventType).HasMaxLength(256).IsRequired();
            entity.Property(e => e.EventData).IsRequired();
            entity.HasIndex(e => new { e.AggregateId, e.Version }).IsUnique();
        });
    }
}
