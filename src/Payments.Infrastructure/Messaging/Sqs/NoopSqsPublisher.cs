using Microsoft.Extensions.Logging;

namespace Payments.Infrastructure.Messaging.Sqs;

public class NoopSqsPublisher(ILogger<NoopSqsPublisher> logger) : ISqsPublisher
{
    public Task PublishAsync<T>(T message, string queueUrl, CancellationToken ct = default)
        where T : class
    {
        logger.LogDebug("SQS not configured — skipping publish of {Type}", typeof(T).Name);
        return Task.CompletedTask;
    }
}
