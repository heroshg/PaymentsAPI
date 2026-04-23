using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Logging;

namespace Payments.Infrastructure.Messaging.Sqs;

public class SqsPublisher(IAmazonSQS sqsClient, ILogger<SqsPublisher> logger) : ISqsPublisher
{
    public async Task PublishAsync<T>(T message, string queueUrl, CancellationToken ct = default)
        where T : class
    {
        var body = JsonSerializer.Serialize(message, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await sqsClient.SendMessageAsync(new SendMessageRequest
        {
            QueueUrl    = queueUrl,
            MessageBody = body
        }, ct);

        logger.LogInformation(
            "Message published to SQS — queue={Queue} type={Type}",
            queueUrl, typeof(T).Name);
    }
}
