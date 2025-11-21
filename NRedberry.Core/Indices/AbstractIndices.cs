using System.Collections;
using System.Collections.Immutable;
using System.Text;
using NRedberry.Contexts;
using NRedberry.Core.Utils;

namespace NRedberry.Indices;

/// <summary>
/// Basic abstract <see cref="Indices"/> implementation.
/// Indices are stored as final integer array.
/// </summary>
/// <remarks>https://github.com/redberry-cas/core/blob/master/src/main/java/cc/redberry/core/indices/AbstractIndices.java</remarks>
public abstract class AbstractIndices(int[] data) : Indices
{
    public int[] Data { get; } = data;

    private UpperLowerIndices? upperLower;

    protected abstract UpperLowerIndices CalculateUpperLower();

    public abstract int[] GetSortedData();

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

    public ImmutableArray<int> GetUpper()
    {
        UpperLowerIndices ul = upperLower;
        if (ul == null)
        {
            ul = CalculateUpperLower();
            upperLower = ul;
        }

        return [..ul.Upper];
    }

    public ImmutableArray<int> GetLower()
    {
        UpperLowerIndices ul = upperLower;
        if (ul is null)
        {
            ul = CalculateUpperLower();
            upperLower = ul;
        }

        return [..ul.Lower];
    }

    public ImmutableArray<int> GetAllIndices()
    {
        return [..Data];
    }

    public abstract Indices GetOfType(IndexType type);

    public bool EqualsRegardlessOrder(Indices indices)
    {
        if (ReferenceEquals(this, indices))
            return true;
        if (indices is EmptyIndices)
            return Data.Length == 0;

        return GetSortedData().SequenceEqual(((AbstractIndices)indices).GetSortedData());
    }

    public abstract void TestConsistentWithException();
    public abstract Indices ApplyIndexMapping(IIndexMapping mapping);

    public int Size() => Data.Length;

    public abstract int Size(IndexType type);

    public int this[int position] => Get(position);
    public abstract int this[IndexType type, int position] { get; }

    public abstract Indices GetFree();
    public abstract Indices GetInverted();

    public int Get(int position) => Data[position];

    public override int GetHashCode() => EnumerableEx.GetHashCode(Data);

    public IEnumerator<int> GetEnumerator()
    {
        throw new System.NotImplementedException();
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(obj, null))
            return false;

        if (GetType() != obj.GetType())
            return false;

        return Data.SequenceEqual(((AbstractIndices)obj).Data);
    }

    public string ToString(OutputFormat mode)
    {
        if (Data.Length == 0)
            return string.Empty;
        bool latex = mode == OutputFormat.LaTeX;
        StringBuilder sb = new StringBuilder();
        int stateMode = (Data[0] >> 31);
        int currentState = stateMode;
        if (stateMode == 0)
        {
            sb.Append(latex ? "_{" : "_{");
        }
        else
        {
            sb.Append(latex ? "^{" : "^{");
        }

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

            sb.Append(Context.Get().ConverterManager.GetSymbol(Data[i], mode));
        }

        sb.Append("}");
        return sb.ToString();
    }

    public abstract short[] GetDiffIds();

    public override string ToString()
    {
        Context tempQualifier = Context.Get();
        return ToString(tempQualifier.DefaultOutputFormat);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
