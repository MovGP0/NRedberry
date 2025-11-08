using System.Text;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// Term order class for ordered polynomials. Implements the most used term orders:
/// graded, lexicographical, weight array and block orders.
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.TermOrder
/// </remarks>
public sealed class TermOrder
{
    public const int LEX = 1;
    public const int INVLEX = 2;
    public const int GRLEX = 3;
    public const int IGRLEX = 4;
    public const int REVLEX = 5;
    public const int REVILEX = 6;
    public const int REVTDEG = 7;
    public const int REVITDG = 8;
    public const int DEFAULT_EVORD = IGRLEX;

    private int evord;
    private int evord2;
    private int evbeg1;
    private int evend1;
    private int evbeg2;
    private int evend2;
    private long[][] weight;
    private IComparer<ExpVector> horder;
    private IComparer<ExpVector> lorder;

    /// <summary>
    /// Initializes the default inverse graded lex term order.
    /// </summary>
    public TermOrder()
    {
        InitializeSingle(DEFAULT_EVORD);
    }

    /// <summary>
    /// Initializes the specified term order.
    /// </summary>
    /// <param name="evord">One of the <c>TermOrder</c> constants.</param>
    public TermOrder(int evord)
    {
        InitializeSingle(evord);
    }

    /// <summary>
    /// Creates a split term order with the default order applied to both blocks.
    /// </summary>
    /// <param name="length">Maximum number of exponents to compare.</param>
    /// <param name="split">Index where the second block starts.</param>
    /// <summary>
    /// Creates a split term order where the same order applies to both blocks.
    /// </summary>
    /// <param name="length">Maximum exponent length.</param>
    /// <param name="split">Index where the second block begins.</param>
    public TermOrder(int length, int split)
    {
        InitializeSplit(DEFAULT_EVORD, DEFAULT_EVORD, length, split);
    }

    /// <summary>
    /// Constructs a weighted term order from a single weight vector.
    /// </summary>
    /// <param name="weightVector">Weight vector for a weighted lex order.</param>
    public TermOrder(long[] weightVector)
        : this(weightVector == null ? throw new ArgumentNullException(nameof(weightVector)) : [weightVector.ToArray()])
    {
    }

    /// <summary>
    /// Constructs a weighted term order using the provided weight matrix.
    /// </summary>
    /// <param name="w">Matrix of weight vectors.</param>
    public TermOrder(long[][] w)
    {
        if (w == null || w.Length == 0)
        {
            throw new ArgumentException("Invalid term order weight", nameof(w));
        }

        weight = CloneWeight(w);
        evord = 0;
        evord2 = 0;
        evbeg1 = 0;
        evend1 = weight[0].Length;
        evbeg2 = evend1;
        evend2 = evend1;

        horder = new ExpVectorComparer((left, right) => -ExpVector.EvIwlc(weight, left, right));
        lorder = new ExpVectorComparer((left, right) => ExpVector.EvIwlc(weight, left, right));
    }

    /// <summary>
    /// Creates a split term order with explicit block orders.
    /// </summary>
    /// <param name="firstOrder">Order for the first block.</param>
    /// <param name="secondOrder">Order for the second block.</param>
    /// <param name="length">Maximum number of exponents considered.</param>
    /// <param name="split">Index where the second block starts.</param>
    /// <summary>
    /// Creates a split term order with independent orders for each block.
    /// </summary>
    public TermOrder(int firstOrder, int secondOrder, int length, int split)
    {
        InitializeSplit(firstOrder, secondOrder, length, split);
    }

    public int GetEvord() => evord;
    public int GetEvord2() => evord2;
    public long[][]? GetWeight() => weight is null ? null : CloneWeight(weight);
    public int GetSplit() => evend1;
    public IComparer<ExpVector> GetDescendComparator() => horder;
    public IComparer<ExpVector> GetAscendComparator() => lorder;

    /// <summary>
    /// Extends the term order when adding new variables.
    /// </summary>
    public TermOrder Extend(int length, int extendBy)
    {
        if (weight != null)
        {
            long[][] cloned = CloneWeight(weight);
            for (int rowIndex = 0; rowIndex < cloned.Length; rowIndex++)
            {
                long[] row = cloned[rowIndex];
                long max = row.Length == 0 ? 0 : row.Max();
                long[] extended = new long[row.Length + extendBy];
                long maxValue = max + 1;
                for (int j = 0; j < rowIndex && j < extended.Length; j++)
                {
                    extended[j] = maxValue;
                }

                Array.Copy(row, 0, extended, rowIndex, row.Length);
                cloned[rowIndex] = extended;
            }

            return new TermOrder(cloned);
        }

        if (evord2 != 0)
        {
            return new TermOrder(evord, evord2, length + extendBy, evend1 + extendBy);
        }

        return new TermOrder(DEFAULT_EVORD, evord, length + extendBy, extendBy);
    }

    /// <summary>
    /// Contracts the term order by removing variables from the front.
    /// </summary>
    public TermOrder Contract(int start, int newLength)
    {
        if (weight != null)
        {
            long[][] contracted = CloneWeight(weight);
            for (int rowIndex = 0; rowIndex < contracted.Length; rowIndex++)
            {
                long[] row = contracted[rowIndex];
                long[] slice = new long[newLength];
                Array.Copy(row, start, slice, 0, newLength);
                contracted[rowIndex] = slice;
            }

            return new TermOrder(contracted);
        }

        if (evord2 == 0)
        {
            return new TermOrder(evord);
        }

        if (evend1 > start)
        {
            int length = evend1 - start;
            while (length > newLength)
            {
                length -= newLength;
            }

            if (length <= 0 || length == newLength)
            {
                return new TermOrder(evord);
            }

            return new TermOrder(evord, evord2, newLength, length);
        }

        return new TermOrder(evord2);
    }

    /// <summary>
    /// Reverses the variable order completely.
    /// </summary>
    public TermOrder Reverse()
    {
        return Reverse(false);
    }

    /// <summary>
    /// Reverses the variable order optionally in partial blocks.
    /// </summary>
    public TermOrder Reverse(bool partial)
    {
        if (weight != null)
        {
            long[][] reversedWeight = CloneWeight(weight);
            for (int i = 0; i < reversedWeight.Length; i++)
            {
                Array.Reverse(reversedWeight[i]);
            }

            return new TermOrder(reversedWeight);
        }

        if (evord2 == 0)
        {
            return new TermOrder(Revert(evord));
        }

        if (partial)
        {
            return new TermOrder(Revert(evord), Revert(evord2), evend2, evend1);
        }

        return new TermOrder(Revert(evord2), Revert(evord), evend2, evend2 - evbeg2);
    }

    /// <summary>
    /// Returns the reverse counterpart of the given order constant.
    /// </summary>
    public static int Revert(int order)
    {
        return order switch
        {
            LEX => REVLEX,
            INVLEX => REVILEX,
            GRLEX => REVTDEG,
            IGRLEX => REVITDG,
            REVLEX => LEX,
            REVILEX => INVLEX,
            REVTDEG => GRLEX,
            REVITDG => IGRLEX,
            _ => order
        };
    }

    /// <summary>
    /// Formats the term order by describing weight rows and splits.
    /// </summary>
    public override string ToString()
    {
        StringBuilder builder = new ();
        if (weight != null)
        {
            builder.Append("W(");
            for (int j = 0; j < weight.Length; j++)
            {
                long[] row = weight[j];
                builder.Append('(');
                for (int i = 0; i < row.Length; i++)
                {
                    builder.Append(row[row.Length - 1 - i]);
                    if (i < row.Length - 1)
                    {
                        builder.Append(',');
                    }
                }

                builder.Append(')');
                if (j < weight.Length - 1)
                {
                    builder.Append(',');
                }
            }

            builder.Append(')');
            if (evend1 == evend2)
            {
                return builder.ToString();
            }

            builder.Append('[')
                .Append(evbeg1)
                .Append(',')
                .Append(evend1)
                .Append(']')
                .Append('[')
                .Append(evbeg2)
                .Append(',')
                .Append(evend2)
                .Append(']');
            return builder.ToString();
        }

        builder.Append(OrderName(evord));
        if (evord2 == 0)
        {
            return builder.ToString();
        }

        builder.Append('[')
            .Append(evbeg1)
            .Append(',')
            .Append(evend1)
            .Append(']')
            .Append(OrderName(evord2))
            .Append('[')
            .Append(evbeg2)
            .Append(',')
            .Append(evend2)
            .Append(']');
        return builder.ToString();
    }

    /// <summary>
    /// Equality compares all internal order parameters and weights.
    /// </summary>
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj is not TermOrder other)
        {
            return false;
        }

        if (evord != other.evord
            || evord2 != other.evord2
            || evbeg1 != other.evbeg1
            || evend1 != other.evend1
            || evbeg2 != other.evbeg2
            || evend2 != other.evend2)
        {
            return false;
        }

        if (weight is null && other.weight is null)
        {
            return true;
        }

        if (weight is null || other.weight is null)
        {
            return false;
        }

        if (weight.Length != other.weight.Length)
        {
            return false;
        }

        for (int i = 0; i < weight.Length; i++)
        {
            if (!weight[i].SequenceEqual(other.weight[i]))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Computes a hash code from the order parameters and weight matrix.
    /// </summary>
    public override int GetHashCode()
    {
        HashCode hash = new ();
        hash.Add(evord);
        hash.Add(evord2);
        hash.Add(evbeg1);
        hash.Add(evend1);
        hash.Add(evbeg2);
        hash.Add(evend2);
        if (weight != null)
        {
            foreach (long[] row in weight)
            {
                foreach (long value in row)
                {
                    hash.Add(value);
                }
            }
        }

        return hash.ToHashCode();
    }

    private void InitializeSingle(int order)
    {
        ValidateOrder(order, nameof(order));
        evord = order;
        evord2 = 0;
        weight = null;
        evbeg1 = 0;
        evend1 = int.MaxValue;
        evbeg2 = int.MaxValue;
        evend2 = int.MaxValue;
        horder = CreateSingleOrderComparer(order);
        lorder = new ExpVectorComparer((left, right) => -horder.Compare(left, right));
    }

    private void InitializeSplit(int firstOrder, int secondOrder, int length, int split)
    {
        ValidateOrder(firstOrder, nameof(firstOrder));
        ValidateOrder(secondOrder, nameof(secondOrder));
        if (split < 0 || split > length)
        {
            throw new ArgumentOutOfRangeException(nameof(split), "Invalid term order split.");
        }

        evord = firstOrder;
        evord2 = secondOrder;
        weight = null;
        evbeg1 = 0;
        evend1 = split;
        evbeg2 = split;
        evend2 = length;

        horder = CreateSplitOrderComparer(firstOrder, secondOrder, evbeg1, evend1, evbeg2, evend2);
        lorder = new ExpVectorComparer((left, right) => -horder.Compare(left, right));
    }

    private static void ValidateOrder(int order, string paramName)
    {
        if (order < LEX || order > REVITDG)
        {
            throw new ArgumentOutOfRangeException(paramName, $"Invalid term order: {order}");
        }
    }

    private static IComparer<ExpVector> CreateSingleOrderComparer(int order)
    {
        return new ExpVectorComparer((left, right) => CompareBlock(left, right, order, 0, int.MaxValue));
    }

    private static IComparer<ExpVector> CreateSplitOrderComparer(int firstOrder, int secondOrder, int begin1, int end1, int begin2, int end2)
    {
        return new ExpVectorComparer((left, right) =>
        {
            int primary = CompareBlock(left, right, firstOrder, begin1, end1);
            if (primary != 0 || secondOrder == 0 || end2 <= begin2)
            {
                return primary;
            }

            return CompareBlock(left, right, secondOrder, begin2, end2);
        });
    }

    private static int CompareBlock(ExpVector left, ExpVector right, int order, int begin, int end)
    {
        if (left is null)
        {
            throw new ArgumentNullException(nameof(left));
        }

        if (right is null)
        {
            throw new ArgumentNullException(nameof(right));
        }

        int leftLength = left.Length();
        int rightLength = right.Length();
        int maxLength = Math.Min(leftLength, rightLength);
        int effectiveEnd = end == int.MaxValue ? maxLength : Math.Min(end, maxLength);
        int effectiveBegin = Math.Clamp(begin, 0, effectiveEnd);
        if (effectiveEnd <= effectiveBegin)
        {
            effectiveBegin = 0;
            effectiveEnd = maxLength;
        }

        return order switch
        {
            LEX => ExpVector.EvIlcp(left, right, effectiveBegin, effectiveEnd),
            INVLEX => -ExpVector.EvIlcp(left, right, effectiveBegin, effectiveEnd),
            GRLEX => ExpVector.EvIglc(left, right, effectiveBegin, effectiveEnd),
            IGRLEX => -ExpVector.EvIglc(left, right, effectiveBegin, effectiveEnd),
            REVLEX => ExpVector.EvRilcp(left, right, effectiveBegin, effectiveEnd),
            REVILEX => -ExpVector.EvRilcp(left, right, effectiveBegin, effectiveEnd),
            REVTDEG => ExpVector.EvRiglc(left, right, effectiveBegin, effectiveEnd),
            REVITDG => -ExpVector.EvRiglc(left, right, effectiveBegin, effectiveEnd),
            _ => throw new ArgumentOutOfRangeException(nameof(order), $"Unsupported term order: {order}")
        };
    }

    private static string OrderName(int order)
    {
        return order switch
        {
            LEX => "LEX",
            INVLEX => "INVLEX",
            GRLEX => "GRLEX",
            IGRLEX => "IGRLEX",
            REVLEX => "REVLEX",
            REVILEX => "REVILEX",
            REVTDEG => "REVTDEG",
            REVITDG => "REVITDG",
            _ => $"ORDER({order})"
        };
    }

    private static long[][] CloneWeight(long[][] source)
    {
        long[][] copy = new long[source.Length][];
        for (int i = 0; i < source.Length; i++)
        {
            copy[i] = source[i].ToArray();
        }

        return copy;
    }

    private sealed class ExpVectorComparer : IComparer<ExpVector>
    {
        private readonly Func<ExpVector, ExpVector, int> comparer;

        public ExpVectorComparer(Func<ExpVector, ExpVector, int> comparer)
        {
            this.comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
        }

        public int Compare(ExpVector? x, ExpVector? y)
        {
            if (ReferenceEquals(x, y))
            {
                return 0;
            }

            if (x is null)
            {
                return -1;
            }

            if (y is null)
            {
                return 1;
            }

            return comparer(x, y);
        }
    }
}
