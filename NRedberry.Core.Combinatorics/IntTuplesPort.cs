namespace NRedberry.Core.Combinatorics;

public sealed class IntTuplesPort : IIntCombinatorialPort
{
    private readonly int[] upperBounds;
    private int[] current;

    public IntTuplesPort(params int[] upperBounds)
    {
        CheckWithException(upperBounds);
        this.upperBounds = upperBounds;
        current = new int[upperBounds.Length];
        current[upperBounds.Length - 1] = -1;
    }

    private static void CheckWithException(int[] upperBounds)
    {
        foreach (var i in upperBounds)
        {
            if (i < 0)
            {
                throw new ArgumentException("Upper bound cannot be negative.");
            }
        }
    }

    public int[] Take()
    {
        var pointer = upperBounds.Length - 1;
        var next = false;
        ++current[pointer];
        if (current[pointer] == upperBounds[pointer])
        {
            current[pointer] = 0;
            next = true;
        }
        while (--pointer >= 0 && next)
        {
            next = false;
            ++current[pointer];
            if (current[pointer] == upperBounds[pointer])
            {
                current[pointer] = 0;
                next = true;
            }
        }
        if (next)
        {
            return null;
        }
        return current;
    }

    public void Reset()
    {
        Array.Fill(current, 0);
        current[upperBounds.Length - 1] = -1;
    }

    public int[] GetReference()
    {
        return current;
    }
}