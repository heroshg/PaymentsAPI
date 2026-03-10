using MediatR;

namespace Payments.Application.Commands.ProcessPayment;

public record ProcessPaymentCommand(
    Guid OrderId,
    Guid UserId,
    string UserEmail,
    Guid GameId,
    string GameName,
    decimal Price) : IRequest;
