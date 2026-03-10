using Payments.Domain.Entities;

namespace Payments.Domain.Interfaces;

public interface IPaymentRepository
{
    Task SaveAsync(Payment payment, CancellationToken ct);
    Task<Payment?> GetByOrderIdAsync(Guid orderId, CancellationToken ct);
}
