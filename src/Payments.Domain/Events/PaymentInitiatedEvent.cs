namespace Payments.Domain.Events;

public record PaymentInitiatedEvent(
    Guid PaymentId,
    Guid UserId,
    string UserEmail,
    Guid GameId,
    string GameName,
    decimal Price,
    DateTime OccurredAt) : IDomainEvent;
