using System.Text;

namespace NRedberry.Tensors;

public sealed class StructureOfContractionsHashed : IEquatable<StructureOfContractionsHashed>
{
    public static readonly StructureOfContractionsHashed EmptyInstance =
        new(new TensorContraction((short)-1, Array.Empty<long>()));

    public StructureOfContractionsHashed(TensorContraction freeContraction, params TensorContraction[] contractions)
    {
        ArgumentNullException.ThrowIfNull(freeContraction);
        ArgumentNullException.ThrowIfNull(contractions);

        FreeContraction = freeContraction;
        Contractions = contractions;
    }

    public TensorContraction FreeContraction { get; }

    private TensorContraction[] Contractions { get; }

    public TensorContraction this[int index] => Contractions[index];

    public TensorContraction Get(int index)
    {
        return Contractions[index];
    }

    public bool Equals(StructureOfContractionsHashed? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (!FreeContraction.Equals(other.FreeContraction))
        {
            return false;
        }

        return Contractions.SequenceEqual(other.Contractions);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as StructureOfContractionsHashed);
    }

    public override int GetHashCode()
    {
        HashCode hashCode = new();
        hashCode.Add(FreeContraction);
        foreach (var contraction in Contractions)
        {
            hashCode.Add(contraction);
        }

        return hashCode.ToHashCode();
    }

    public override string ToString()
    {
        StringBuilder builder = new();
        builder.Append("Free: ");
        builder.Append(FreeContraction);
        foreach (var contraction in Contractions)
        {
            builder.Append('\n').Append(contraction);
        }

        return builder.ToString();
    }

    public static bool operator ==(StructureOfContractionsHashed? left, StructureOfContractionsHashed? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(StructureOfContractionsHashed? left, StructureOfContractionsHashed? right)
    {
        return !Equals(left, right);
    }
}
