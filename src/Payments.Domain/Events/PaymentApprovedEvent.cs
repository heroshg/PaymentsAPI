namespace Payments.Domain.Events;

public record PaymentApprovedEvent(Guid PaymentId, DateTime OccurredAt) : IDomainEvent;
