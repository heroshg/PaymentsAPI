using Payments.Domain.Events;

namespace Payments.Domain.Entities;

public class Payment
{
    private readonly List<IDomainEvent> _uncommittedEvents = new();

    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string UserEmail { get; private set; } = string.Empty;
    public Guid GameId { get; private set; }
    public string GameName { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public string Status { get; private set; } = string.Empty;
    public DateTime? ProcessedAt { get; private set; }
    public int Version { get; private set; }

    public IReadOnlyList<IDomainEvent> UncommittedEvents => _uncommittedEvents.AsReadOnly();

    private Payment() { }

    /// <summary>Reconstitui o agregado a partir do histórico de eventos (Event Sourcing).</summary>
    public static Payment LoadFromHistory(IEnumerable<IDomainEvent> events)
    {
        var payment = new Payment();
        foreach (var @event in events)
            payment.Apply(@event);
        return payment;
    }

    public static Payment Initiate(Guid orderId, Guid userId, string userEmail,
        Guid gameId, string gameName, decimal price)
    {
        var payment = new Payment();
        payment.RaiseEvent(new PaymentInitiatedEvent(
            orderId, userId, userEmail, gameId, gameName, price, DateTime.UtcNow));
        return payment;
    }

    public void Process()
    {
        if (Status != "Initiated")
            throw new InvalidOperationException($"Cannot process payment in status '{Status}'.");

        var approved = Random.Shared.NextDouble() > 0.1; // 90% approval rate
        if (approved)
            RaiseEvent(new PaymentApprovedEvent(Id, DateTime.UtcNow));
        else
            RaiseEvent(new PaymentRejectedEvent(Id, "Insufficient funds", DateTime.UtcNow));
    }

    public bool IsApproved => Status == "Approved";

    private void RaiseEvent(IDomainEvent @event)
    {
        Apply(@event);
        _uncommittedEvents.Add(@event);
    }

    private void Apply(IDomainEvent @event)
    {
        switch (@event)
        {
            case PaymentInitiatedEvent e:
                Id = e.PaymentId;
                UserId = e.UserId;
                UserEmail = e.UserEmail;
                GameId = e.GameId;
                GameName = e.GameName;
                Price = e.Price;
                Status = "Initiated";
                break;
            case PaymentApprovedEvent e:
                Status = "Approved";
                ProcessedAt = e.OccurredAt;
                break;
            case PaymentRejectedEvent e:
                Status = "Rejected";
                ProcessedAt = e.OccurredAt;
                break;
        }
        Version++;
    }
}
