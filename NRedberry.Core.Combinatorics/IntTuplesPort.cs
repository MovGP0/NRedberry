namespace NRedberry.Core.Combinatorics;

/// <summary>
/// Iterator over all N-tuples chosen from ranges [0..K_i).
/// </summary>
/// <remarks>
/// The calculation of the next tuple occurs only on the invocation of Take. Take returns the same reference on each
/// invocation, so clone the array if you need to keep it.
/// </remarks>
public sealed class IntTuplesPort : IIntCombinatorialPort
{
    private readonly int[] upperBounds;
    private readonly int[] current;
    private int lastUpdateDepth = -1;

    public IntTuplesPort(params int[] upperBounds)
    {
        CheckWithException(upperBounds);
        this.upperBounds = upperBounds;
        current = new int[upperBounds.Length];
        current[upperBounds.Length - 1] = -1;
    }

    private static void CheckWithException(int[] upperBounds)
    {
        foreach (var upperBound in upperBounds)
        {
            if (upperBound < 0)
            {
                throw new ArgumentException("Upper bound cannot be negative.");
            }
        }
    }

    public int[]? Take()
    {
        var pointer = upperBounds.Length - 1;
        var next = false;
        ++current[pointer];
        if (current[pointer] == upperBounds[pointer])
        {
            current[pointer] = 0;
            next = true;
        }

        while (next && --pointer >= 0)
        {
            next = false;
            ++current[pointer];
            if (current[pointer] == upperBounds[pointer])
            {
                current[pointer] = 0;
                next = true;
            }
        }

        if (lastUpdateDepth != -1)
        {
            lastUpdateDepth = pointer;
        }
        else
        {
            lastUpdateDepth = 0;
        }

        if (next)
        {
            return null;
        }

        return current;
    }

    public int GetLastUpdateDepth()
    {
        return lastUpdateDepth;
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
