using FiapCloudGames.Contracts.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Payments.Domain.Entities;

namespace Payments.Application.Commands.ProcessPayment;

public class ProcessPaymentHandler(
    IPublishEndpoint publishEndpoint,
    ILogger<ProcessPaymentHandler> logger)
    : IRequestHandler<ProcessPaymentCommand>
{
    public async Task Handle(ProcessPaymentCommand request, CancellationToken ct)
    {
        logger.LogInformation(
            "Processing payment for OrderId={OrderId} UserId={UserId} GameId={GameId} Price={Price}",
            request.OrderId, request.UserId, request.GameId, request.Price);

        var payment = Payment.Process(
            request.OrderId, request.UserId, request.UserEmail,
            request.GameId, request.GameName, request.Price);

        logger.LogInformation("Payment {Status} for OrderId={OrderId}", payment.Status, payment.OrderId);

        await publishEndpoint.Publish(new PaymentProcessedEvent(
            payment.OrderId,
            payment.UserId,
            payment.UserEmail,
            payment.GameId,
            payment.GameName,
            payment.Price,
            payment.Status), ct);
    }
}
