using System.Linq;
using System.Text;
using NRedberry.Core.Contexts;
using NRedberry.Core.Tensors;
using NRedberry.Core.Utils;

namespace NRedberry.Core.Indices;

/// <summary>
/// Basic abstract <see cref="IIndices"/> implementation.
/// Indices are stored as final integer array.
/// </summary>
/// <remarks>https://github.com/redberry-cas/core/blob/master/src/main/java/cc/redberry/core/indices/AbstractIndices.java</remarks>
public abstract class AbstractIndices : IIndices
{
    public long[] Data { get; }
    private UpperLowerIndices? upperLower;

    protected AbstractIndices(long[] data)
    {
        Data = data;
    }

    protected abstract UpperLowerIndices CalculateUpperLower();

    public abstract long[] GetSortedData();

    protected UpperLowerIndices GetUpperLowerIndices()
    {
        UpperLowerIndices ul = upperLower;
        if (ul == null)
        {
            ul = CalculateUpperLower();
            upperLower = ul;
        }
        return ul;
    }

    public IntArray GetUpper()
    {
        UpperLowerIndices ul = upperLower;
        if (ul == null)
        {
            ul = CalculateUpperLower();
            upperLower = ul;
        }
        return new IntArray(ul.upper);
    }

    public IntArray GetLower()
    {
        UpperLowerIndices ul = upperLower;
        if (ul is null)
        {
            ul = CalculateUpperLower();
            upperLower = ul;
        }
        return new IntArray(ul.lower);
    }

    public IntArray GetAllIndices()
    {
        return new IntArray(Data);
    }

    public abstract IIndices GetOfType(IndexType type);

    public bool EqualsRegardlessOrder(IIndices indices)
    {
        if (ReferenceEquals(this, indices)) return true;
        if (indices is EmptyIndices) return Data.Length == 0;

        return GetSortedData().SequenceEqual(((AbstractIndices)indices).GetSortedData());
    }

    public abstract void TestConsistentWithException();
    public abstract IIndices ApplyIndexMapping(IIndexMapping mapping);

    public int Size() => Data.Length;

    public abstract int Size(IndexType type);

    public long this[long position] => Get(position);
    public abstract long this[IndexType type, long position] { get; }
    public abstract IIndices GetFree();
    public abstract IIndices GetInverted();

    public long Get(long position) => Data[position];

    public override int GetHashCode() => EnumerableEx.GetHashCode(Data);

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(obj, null)) return false;
        if (GetType() != obj.GetType()) return false;
        return Enumerable.SequenceEqual(Data, ((AbstractIndices)obj).Data);
    }

    public string ToString(OutputFormat mode)
    {
        if (Data.Length == 0)
            return "";
        bool latex = mode == OutputFormat.LaTeX;
        StringBuilder sb = new StringBuilder();
        long stateMode = (Data[0] >> 31);
        long currentState = stateMode;
        if (stateMode == 0)
            sb.Append(latex ? "_{" : "_{");
        else
            sb.Append(latex ? "^{" : "^{");
        for (int i = 0; i < Data.Length; i++)
        {
            stateMode = Data[i] >> 31;
            if (currentState != stateMode)
            {
                if (currentState == 0)
                    sb.Append(latex ? "}{}^{" : "}^{");
                if (currentState == 1)
                    sb.Append(latex ? "}{}_{" : "}_{");
                currentState = stateMode;
            }
            sb.Append(Context.Get().GetIndexConverterManager().GetSymbol(Data[i], mode));
        }
        sb.Append("}");
        return sb.ToString();
    }

    public abstract short[] GetDiffIds();

    public override string ToString()
    {
        return ToString(Context.Get().GetDefaultOutputFormat());
    }

    protected class UpperLowerIndices
    {
        public readonly long[] upper;
        public readonly long[] lower;

        public UpperLowerIndices(long[] upper, long[] lower)
        {
            this.upper = upper;
            this.lower = lower;
        }
    }
}