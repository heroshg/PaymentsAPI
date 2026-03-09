using FiapCloudGames.Contracts.Events;
using MassTransit;

namespace PaymentsAPI.Consumers;

public class OrderPlacedConsumer(
    IPublishEndpoint publishEndpoint,
    ILogger<OrderPlacedConsumer> logger)
    : IConsumer<OrderPlacedEvent>
{
    public async Task Consume(ConsumeContext<OrderPlacedEvent> context)
    {
        var evt = context.Message;

        logger.LogInformation(
            "Processing payment for OrderId={OrderId} UserId={UserId} GameId={GameId} Price={Price}",
            evt.OrderId, evt.UserId, evt.GameId, evt.Price);

        // Simulate payment processing (random approve/reject)
        var approved = Random.Shared.NextDouble() > 0.1; // 90% approval rate
        var status = approved ? "Approved" : "Rejected";

        logger.LogInformation(
            "Payment {Status} for OrderId={OrderId}",
            status, evt.OrderId);

        await publishEndpoint.Publish(new PaymentProcessedEvent(
            evt.OrderId,
            evt.UserId,
            evt.UserEmail,
            evt.GameId,
            evt.GameName,
            evt.Price,
            status), context.CancellationToken);
    }
}
