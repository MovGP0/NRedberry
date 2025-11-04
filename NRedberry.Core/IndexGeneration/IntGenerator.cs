using System.Collections;
using NRedberry.Core.Utils;

namespace NRedberry.Core;

public sealed class IntGenerator : ICloneable<IntGenerator>, IEnumerator<int>
{
    private static int[] EMPTY_ARRAY = [-1];
    private int[] EngagedData;

    private int Counter { get; set; }
    private int Match { get; set; }

    public IntGenerator()
        : this(EMPTY_ARRAY)
    {
    }

    private IntGenerator(int[] engagedData, int counter, int match)
    {
        EngagedData = engagedData;
        Counter = counter;
        Match = match;
    }

    public IntGenerator(int[] engagedData)
    {
        EngagedData = engagedData;
        Counter = -1;
        Match = 0;
        Array.Sort(EngagedData);
        var shift = 0;
        var i = 0;
        while (i + shift + 1 < engagedData.Length)
        {
            if (engagedData[i + shift] == engagedData[i + shift + 1])
            {
                ++shift;
            }
            else
            {
                engagedData[i] = engagedData[i + shift];
                ++i;
            }
        }

        engagedData[i] = engagedData[i + shift];

        while (++i < engagedData.Length)
        {
            engagedData[i] = int.MaxValue;
        }
    }

    public void MergeFrom(IntGenerator intGenerator)
    {
        if (intGenerator.EngagedData != EngagedData)
        {
            throw new ArgumentException(nameof(intGenerator));
        }

        if (intGenerator.Counter > Counter)
        {
            Counter = intGenerator.Counter;
            Match = intGenerator.Match;
        }
    }

    [Obsolete("Use IEnumerator interface instead", true)]
    public int GetNext()
    {
        Counter++;
        while (Match < EngagedData.Length && EngagedData[Match] == Counter)
        {
            Match++;
            Counter++;
        }

        return Counter;
    }

    public bool Contains(int index)
    {
        if (Counter >= index)
        {
            return true;
        }

        return Arrays.BinarySearch(EngagedData, Match, EngagedData.Length, index) >= 0;
    }

    #region ICloneable<IntGenerator>

    public IntGenerator Clone() => new(EngagedData, Counter, Match);

    object ICloneable.Clone() => Clone();

    #endregion

    #region IEnumerator<int>

    public int Current
    {
        get
        {
            if (Counter < 0 || Counter >= EngagedData.Length)
                throw new InvalidOperationException();
            return Counter;
        }
    }

    object IEnumerator.Current => Current;

    public bool MoveNext() => ++Counter < EngagedData.Length;

    public void Reset() => Counter = -1;

    public void Dispose()
    {
        // nothing to dispose
    }

    #endregion
}
