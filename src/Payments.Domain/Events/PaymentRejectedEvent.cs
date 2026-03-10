namespace Payments.Domain.Events;

public record PaymentRejectedEvent(Guid PaymentId, string Reason, DateTime OccurredAt) : IDomainEvent;
