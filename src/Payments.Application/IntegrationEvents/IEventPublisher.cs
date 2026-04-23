using FiapCloudGames.Contracts.Events;

namespace Payments.Application.IntegrationEvents;

/// <summary>
/// Publica eventos de integração para consumidores externos (SQS em produção, no-op em dev).
/// </summary>
public interface IEventPublisher
{
    Task PublishPaymentProcessedAsync(PaymentProcessedEvent evt, CancellationToken ct = default);
}
