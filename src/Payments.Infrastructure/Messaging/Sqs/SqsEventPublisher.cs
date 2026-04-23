using FiapCloudGames.Contracts.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Payments.Application.IntegrationEvents;

namespace Payments.Infrastructure.Messaging.Sqs;

public class SqsEventPublisher(
    ISqsPublisher sqsPublisher,
    IConfiguration configuration,
    ILogger<SqsEventPublisher> logger) : IEventPublisher
{
    public async Task PublishPaymentProcessedAsync(PaymentProcessedEvent evt, CancellationToken ct = default)
    {
        var queueUrl = configuration["AWS:SQS:PaymentProcessedQueueUrl"];
        if (string.IsNullOrWhiteSpace(queueUrl))
        {
            logger.LogDebug("AWS:SQS:PaymentProcessedQueueUrl not configured — skipping SQS publish");
            return;
        }

        await sqsPublisher.PublishAsync(evt, queueUrl, ct);
    }
}
