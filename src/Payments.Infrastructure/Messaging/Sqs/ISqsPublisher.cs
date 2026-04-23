namespace Payments.Infrastructure.Messaging.Sqs;

public interface ISqsPublisher
{
    Task PublishAsync<T>(T message, string queueUrl, CancellationToken ct = default) where T : class;
}
