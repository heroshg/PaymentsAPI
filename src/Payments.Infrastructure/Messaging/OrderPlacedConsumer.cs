using FiapCloudGames.Contracts.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Payments.Application.Commands.ProcessPayment;

namespace Payments.Infrastructure.Messaging;

public class OrderPlacedConsumer(
    IMediator mediator,
    ILogger<OrderPlacedConsumer> logger)
    : IConsumer<OrderPlacedEvent>
{
    public async Task Consume(ConsumeContext<OrderPlacedEvent> context)
    {
        var evt = context.Message;
        logger.LogInformation("OrderPlacedEvent received. OrderId={OrderId}", evt.OrderId);

        await mediator.Send(new ProcessPaymentCommand(
            evt.OrderId,
            evt.UserId,
            evt.UserEmail,
            evt.GameId,
            evt.GameName,
            evt.Price), context.CancellationToken);
    }
}
