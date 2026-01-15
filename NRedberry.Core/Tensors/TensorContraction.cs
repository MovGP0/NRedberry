using System.Text;
using NRedberry.Core.Utils;

namespace NRedberry.Tensors;

public sealed class TensorContraction : IComparable<TensorContraction>, IEquatable<TensorContraction>
{
    private int _hash = -1;

    public TensorContraction(short tensorId, long[] indexContractions)
    {
        ArgumentNullException.ThrowIfNull(indexContractions);

        TensorId = tensorId;
        IndexContractions = indexContractions;
    }

    public short TensorId { get; }

    public long[] IndexContractions { get; }

    public void SortContractions()
    {
        Array.Sort(IndexContractions);
        _hash = -1;
    }

    public bool ContainsFreeIndex()
    {
        foreach (var contraction in IndexContractions)
        {
            if (GetToTensorId(contraction) == -1)
            {
                return true;
            }
        }

        return false;
    }

    public bool Equals(TensorContraction? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (GetHashCode() != other.GetHashCode())
        {
            return false;
        }

        if (TensorId != other.TensorId)
        {
            return false;
        }

        return IndexContractions.SequenceEqual(other.IndexContractions);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as TensorContraction);
    }

    public override int GetHashCode()
    {
        if (_hash == -1)
        {
            long hash = 1L;
            foreach (var contraction in IndexContractions)
            {
                hash ^= HashFunctions.JenkinWang64shift(contraction);
            }

            _hash = HashFunctions.Wang64to32shift(hash);
        }

        return _hash;
    }

    public int CompareTo(TensorContraction? other)
    {
        if (other is null)
        {
            return 1;
        }

        int value = TensorId.CompareTo(other.TensorId);
        if (value != 0)
        {
            return value;
        }

        value = IndexContractions.Length.CompareTo(other.IndexContractions.Length);
        if (value != 0)
        {
            return value;
        }

        for (int i = 0; i < IndexContractions.Length; ++i)
        {
            value = IndexContractions[i].CompareTo(other.IndexContractions[i]);
            if (value != 0)
            {
                return value;
            }
        }

        return 0;
    }

    public override string ToString()
    {
        var builder = new StringBuilder();
        if (IndexContractions.Length == 0)
        {
            return builder.Append(TensorId).Append("x").ToString();
        }

        builder.Append(TensorId).Append("x{");
        foreach (var contraction in IndexContractions)
        {
            builder.Append("^")
                .Append(GetFromIndexId(contraction))
                .Append("->")
                .Append(GetToTensorId(contraction))
                .Append("^")
                .Append(GetToIndexId(contraction));
            builder.Append(":");
        }

        builder.Remove(builder.Length - 1, 1);
        builder.Append("}");
        return builder.ToString();
    }

    public static short GetFromIndexId(long contraction)
    {
        return (short)((contraction >> 32) & 0xFFFF);
    }

    public static short GetToIndexId(long contraction)
    {
        return (short)(contraction & 0xFFFF);
    }

    public static short GetToTensorId(long contraction)
    {
        return (short)((contraction >> 16) & 0xFFFF);
    }

    public static bool operator ==(TensorContraction? left, TensorContraction? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(TensorContraction? left, TensorContraction? right)
    {
        return !Equals(left, right);
    }
}
