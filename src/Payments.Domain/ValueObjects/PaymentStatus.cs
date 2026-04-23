namespace Payments.Domain.ValueObjects;

public sealed class PaymentStatus
{
    public static readonly PaymentStatus Initiated = new("Initiated");
    public static readonly PaymentStatus Approved  = new("Approved");
    public static readonly PaymentStatus Rejected  = new("Rejected");

    public string Value { get; }

    private PaymentStatus(string value) => Value = value;

    public override bool Equals(object? obj) => obj is PaymentStatus s && Value == s.Value;
    public override int GetHashCode() => Value.GetHashCode();
    public static bool operator ==(PaymentStatus? a, PaymentStatus? b) => a?.Value == b?.Value;
    public static bool operator !=(PaymentStatus? a, PaymentStatus? b) => !(a == b);
    public override string ToString() => Value;
}
