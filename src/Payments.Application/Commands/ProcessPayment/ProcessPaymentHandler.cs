using FiapCloudGames.Contracts.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Payments.Domain.Entities;
using Payments.Domain.Interfaces;

namespace Payments.Application.Commands.ProcessPayment;

public class ProcessPaymentHandler(
    IPaymentRepository paymentRepository,
    IPublishEndpoint publishEndpoint,
    ILogger<ProcessPaymentHandler> logger)
    : IRequestHandler<ProcessPaymentCommand>
{
    public async Task Handle(ProcessPaymentCommand request, CancellationToken ct)
    {
        logger.LogInformation(
            "Processing payment for OrderId={OrderId} UserId={UserId} GameId={GameId} Price={Price}",
            request.OrderId, request.UserId, request.GameId, request.Price);

        var payment = Payment.Initiate(
            request.OrderId, request.UserId, request.UserEmail,
            request.GameId, request.GameName, request.Price);

        payment.Process();

        await paymentRepository.SaveAsync(payment, ct);

        logger.LogInformation(
            "Payment {Status} for OrderId={OrderId}. {EventCount} events persisted to event store.",
            payment.Status, payment.Id, payment.UncommittedEvents.Count);

        await publishEndpoint.Publish(new PaymentProcessedEvent(
            payment.Id,
            payment.UserId,
            payment.UserEmail,
            payment.GameId,
            payment.GameName,
            payment.Price,
            payment.Status), ct);
    }
}
