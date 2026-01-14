using NRedberry.Core.Utils;

namespace NRedberry.IndexGeneration;

public sealed class IntGenerator : ICloneable<IntGenerator>
{
    private static readonly int[] s_emptyArray = [-1];
    private readonly int[] _engagedData;
    private int _counter;
    private int _match;

    public IntGenerator()
        : this(s_emptyArray)
    {
    }

    private IntGenerator(int[] engagedData, int counter, int match)
    {
        _engagedData = engagedData;
        _counter = counter;
        _match = match;
    }

    public IntGenerator(int[] engagedData)
    {
        _engagedData = engagedData;
        _counter = -1;
        _match = 0;
        Array.Sort(_engagedData);
        int shift = 0;
        int i = 0;
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
        if (intGenerator._engagedData != _engagedData)
        {
            throw new ArgumentException(nameof(intGenerator));
        }

        if (intGenerator._counter > _counter)
        {
            _counter = intGenerator._counter;
            _match = intGenerator._match;
        }
    }

    public int GetNext()
    {
        _counter++;
        while (_match < _engagedData.Length && _engagedData[_match] == _counter)
        {
            _match++;
            _counter++;
        }

        return _counter;
    }

    public bool Contains(int index)
    {
        if (_counter >= index)
        {
            return true;
        }

        return Arrays.BinarySearch(_engagedData, _match, _engagedData.Length, index) >= 0;
    }

    public IntGenerator Clone() => new(_engagedData, _counter, _match);

    object ICloneable.Clone() => Clone();
}
