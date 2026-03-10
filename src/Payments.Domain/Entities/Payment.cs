namespace Payments.Domain.Entities;

public class Payment
{
    public Guid OrderId { get; private set; }
    public Guid UserId { get; private set; }
    public string UserEmail { get; private set; }
    public Guid GameId { get; private set; }
    public string GameName { get; private set; }
    public decimal Price { get; private set; }
    public string Status { get; private set; }
    public DateTime ProcessedAt { get; private set; }

    private Payment() { UserEmail = string.Empty; GameName = string.Empty; Status = string.Empty; }

    public static Payment Process(Guid orderId, Guid userId, string userEmail, Guid gameId, string gameName, decimal price)
    {
        var approved = Random.Shared.NextDouble() > 0.1; // 90% approval rate
        return new Payment
        {
            OrderId = orderId,
            UserId = userId,
            UserEmail = userEmail,
            GameId = gameId,
            GameName = gameName,
            Price = price,
            Status = approved ? "Approved" : "Rejected",
            ProcessedAt = DateTime.UtcNow
        };
    }

    public bool IsApproved => Status == "Approved";
}
