using System.Collections;
using NRedberry.Core.Combinatorics.Extensions;

namespace NRedberry.Core.Combinatorics;

public sealed class IntDistinctTuplesPort : IIntCombinatorialPort
{
    private readonly BitArray previousMask;
    private readonly BitArray[] setMasks;
    private readonly int[] combination;
    private readonly BitArray temp;
    private byte state = unchecked((byte)-1);

    public IntDistinctTuplesPort(params int[][] sets)
    {
        var maxIndex = 0;
        foreach (var set in sets)
        {
            if (set.Length == 0)
            {
                continue;
            }

            Array.Sort(set);
            if (maxIndex < set[^1])
            {
                maxIndex = set[^1];
            }
        }

        ++maxIndex;
        previousMask = new BitArray(maxIndex);
        temp = new BitArray(maxIndex);

        setMasks = new BitArray[sets.Length];
        for (var i = 0; i < sets.Length; ++i)
        {
            setMasks[i] = new BitArray(maxIndex);
            foreach (var j in sets[i])
            {
                setMasks[i].Set(j, true);
            }
        }

        combination = new int[sets.Length];
        previousMask.SetAll(true);
        Init();
    }

    private void Init()
    {
        var i = 0;
        while (i < setMasks.Length)
        {
            temp.SetAll(false);
            temp.Or(setMasks[i]);
            temp.And(previousMask);

            var nextBit = temp.NextTrailingBit(combination[i]);
            if (nextBit != -1)
            {
                combination[i] = nextBit;
                previousMask.Set(nextBit, false);
            }
            else
            {
                if (i == 0)
                {
                    state = 1;
                    return;
                }
                combination[i] = 0;
                previousMask.Set(combination[--i], true);
                ++combination[i];
                continue;
            }
            ++i;
        }
    }

    public int[] Take()
    {
        if (state == 1)
        {
            return [];
        }

        if (state == unchecked((byte)-1))
        {
            state = 0;
            return combination;
        }

        previousMask.Set(combination[setMasks.Length - 1]++, true);

        var i = setMasks.Length - 1;
        while (i < setMasks.Length)
        {
            temp.SetAll(false);
            temp.Or(setMasks[i]);
            temp.And(previousMask);

            var nextBit = temp.NextTrailingBit(combination[i]);
            if (nextBit != -1)
            {
                combination[i] = nextBit;
                previousMask.Set(nextBit, false);
            }
            else
            {
                if (i == 0)
                {
                    state = 1;
                    return [];
                }

                combination[i] = 0;
                previousMask.Set(combination[--i], true);
                ++combination[i];
                continue;
            }

            ++i;
        }

        return combination;
    }

    public void Reset()
    {
        state = unchecked((byte)-1);
        Array.Fill(combination, 0);
        previousMask.SetAll(true);
        Init();
    }

    public int[] GetReference() => combination;
}