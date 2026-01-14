namespace NRedberry.Contexts;

internal sealed class DerivativeDescriptor : IEquatable<DerivativeDescriptor>
{
    public DerivativeDescriptor(int[] orders)
    {
        ArgumentNullException.ThrowIfNull(orders);
        Orders = orders;
    }

    public int[] Orders { get; }

    public bool Equals(DerivativeDescriptor? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (other is null || Orders.Length != other.Orders.Length)
        {
            return false;
        }

        for (var i = 0; i < Orders.Length; ++i)
        {
            if (Orders[i] != other.Orders[i])
            {
                return false;
            }
        }

        return true;
    }

    public override bool Equals(object? obj) => obj is DerivativeDescriptor other && Equals(other);

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        foreach (var order in Orders)
        {
            hashCode.Add(order);
        }

        return hashCode.ToHashCode();
    }

    public static bool operator ==(DerivativeDescriptor? left, DerivativeDescriptor? right) => Equals(left, right);

    public static bool operator !=(DerivativeDescriptor? left, DerivativeDescriptor? right) => !Equals(left, right);
}
