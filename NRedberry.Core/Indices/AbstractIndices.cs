using System.Collections;
using System.Collections.Immutable;
using System.Text;
using NRedberry.Contexts;

namespace NRedberry.Indices;

/// <summary>
/// Basic abstract <see cref="Indices"/> implementation.
/// Indices are stored as final integer array.
/// </summary>
/// <remarks>https://github.com/redberry-cas/core/blob/master/src/main/java/cc/redberry/core/indices/AbstractIndices.java</remarks>
public abstract class AbstractIndices(int[] data) : Indices
{
    public int[] Data { get; } = data ?? throw new ArgumentNullException(nameof(data));

    //TODO: private WeakReference<UpperLowerIndices?> _upperLower = new(null);
    private UpperLowerIndices? _upperLower;

    protected abstract UpperLowerIndices CalculateUpperLower();

    public abstract int[] GetSortedData();

    protected UpperLowerIndices UpperLowerIndices
    {
        get
        {
            UpperLowerIndices? ul = _upperLower;
            if (ul is null)
            {
                ul = CalculateUpperLower();
                _upperLower = ul;
            }

            return ul;
        }
    }

    public ImmutableArray<int> UpperIndices
    {
        get
        {
            UpperLowerIndices? ul = _upperLower;
            if (ul is null)
            {
                ul = CalculateUpperLower();
                _upperLower = ul;
            }

            return [..ul.Upper];
        }
    }

    public ImmutableArray<int> LowerIndices
    {
        get
        {
            UpperLowerIndices? ul = _upperLower;
            if (ul is null)
            {
                ul = CalculateUpperLower();
                _upperLower = ul;
            }

            return [..ul.Lower];
        }
    }

    public ImmutableArray<int> AllIndices => [.. Data];

    public abstract Indices GetOfType(IndexType type);

    public bool EqualsRegardlessOrder(Indices indices)
    {
        if (ReferenceEquals(this, indices))
        {
            return true;
        }

        if (indices is EmptyIndices)
        {
            return Data.Length == 0;
        }

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

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 1;
            for (int i = 0; i < Data.Length; ++i)
            {
                hash = (hash * 31) + Data[i];
            }

            return 291 + hash;
        }
    }

    public IEnumerator<int> GetEnumerator()
    {
        return ((IEnumerable<int>)Data).GetEnumerator();
    }

    public override bool Equals(object? obj)
    {
        if (obj is not AbstractIndices other)
        {
            return false;
        }

        return Data.SequenceEqual(other.Data);
    }

    public string ToString(OutputFormat mode)
    {
        if (Data.Length == 0)
        {
            return string.Empty;
        }

        StringBuilder sb = new();
        if (mode.Is(OutputFormat.WolframMathematica) || mode.Is(OutputFormat.Maple))
        {
            for (int i = 0; ; i++)
            {
                int currentState = (int)((uint)Data[i] >> 31);
                sb.Append(currentState == 1 ? mode.UpperIndexPrefix : mode.LowerIndexPrefix);
                sb.Append(Context.Get().ConverterManager.GetSymbol(Data[i], mode));
                if (i == Data.Length - 1)
                {
                    break;
                }

                sb.Append(',');
            }
        }
        else if (mode.Is(OutputFormat.Cadabra))
        {
            List<int> nonMetricIndices = [];
            List<int> metricIndices = new(Data.Length);
            for (int i = 0; i < Data.Length; ++i)
            {
                if (CC.IsMetric(IndicesUtils.GetType(Data[i])))
                {
                    metricIndices.Add(Data[i]);
                }
                else
                {
                    nonMetricIndices.Add(Data[i]);
                }
            }

            if (metricIndices.Count > 0)
            {
                sb.Append("_{");
                for (int i = 0, size = metricIndices.Count - 1; ; ++i)
                {
                    sb.Append(Context.Get().ConverterManager.GetSymbol(metricIndices[i], mode));
                    if (i == size)
                    {
                        break;
                    }

                    sb.Append(' ');
                }

                sb.Append('}');
            }

            if (nonMetricIndices.Count > 0)
            {
                int currentState = (int)((uint)nonMetricIndices[0] >> 31);
                sb.Append(mode.LowerIndexPrefix).Append('{');
                int lastState = currentState;
                for (int i = 0, size = nonMetricIndices.Count - 1; ; ++i)
                {
                    currentState = (int)((uint)nonMetricIndices[i] >> 31);
                    if (lastState != currentState)
                    {
                        sb.Append('}').Append(mode.GetPrefixFromIntState(currentState)).Append('{');
                        lastState = currentState;
                    }

                    sb.Append(Context.Get().ConverterManager.GetSymbol(nonMetricIndices[i], mode));
                    if (i == size)
                    {
                        break;
                    }

                    if (currentState == (int)((uint)nonMetricIndices[i + 1] >> 31))
                    {
                        sb.Append(' ');
                    }
                }

                sb.Append('}');
            }
        }
        else
        {
            string latexBrackets = mode.Is(OutputFormat.LaTeX) ? "{}" : string.Empty;

            int totalToPrint = 0;
            int lastState = -1;
            for (int i = 0; i < Data.Length; i++)
            {
                if (!CC.IsMetric(IndicesUtils.GetType(Data[i])) && !mode.PrintMatrixIndices)
                {
                    continue;
                }

                int currentState = (int)((uint)Data[i] >> 31);
                if (lastState != currentState)
                {
                    if (totalToPrint != 0)
                    {
                        sb.Append('}');
                    }

                    sb.Append(latexBrackets).Append(mode.GetPrefixFromIntState(currentState)).Append('{');
                    lastState = currentState;
                }

                sb.Append(Context.Get().ConverterManager.GetSymbol(Data[i], mode));
                ++totalToPrint;
            }

            sb.Append('}');
            if (totalToPrint == 0)
            {
                return string.Empty;
            }
        }

        return sb.ToString();
    }

    public abstract short[] GetDiffIds();

    public override string ToString()
    {
        Context tempQualifier = Context.Get();
        return ToString(tempQualifier.DefaultOutputFormat);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
