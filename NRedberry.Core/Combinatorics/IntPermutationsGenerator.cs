using System;
using System.Collections.Generic;

namespace NRedberry.Core.Combinatorics;

public sealed class IntPermutationsGenerator : IIntCombinatorialGenerator
{
    internal readonly int[] permutation;
    private bool onFirst = true;
    private readonly int size;

    public IntPermutationsGenerator(int dimension)
    {
        permutation = new int[dimension];
        for (int i = 0; i < dimension; ++i)
            permutation[i] = i;
        size = dimension;
    }

    public IntPermutationsGenerator(int[] permutation)
    {
        this.permutation = permutation;
        size = permutation.Length;
        for (int i = 0; i < size - 1; ++i)
        {
            if (permutation[i] >= size || permutation[i] < 0)
                throw new ArgumentException("Wrong permutation input: image of " + i + " element"
                                            + " greater than degree");
            for (int j = i + 1; j < size; ++j)
                if (permutation[i] == permutation[j])
                    throw new ArgumentException("Wrong permutation input: two elements have the same image");
        }
    }

    public bool MoveNext()
    {
        return !IsLast() || onFirst;
    }

    public void Reset()
    {
        onFirst = true;
        for (int i = 0; i < size; ++i)
            permutation[i] = i;
    }

    public int[] Current => Next();

    object System.Collections.IEnumerator.Current => Current;

    public void Dispose() { }

    public IEnumerator<int[]> GetEnumerator()
    {
        return this;
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private bool IsLast()
    {
        for (int i = 0; i < size; i++)
            if (permutation[i] != size - 1 - i)
                return false;
        return true;
    }

    private int[] Next()
    {
        if (onFirst)
        {
            onFirst = false;
            return permutation;
        }
        int end = size - 1;
        int p = end, low, high, med, s;
        while ((p > 0) && (permutation[p] < permutation[p - 1]))
            p--;
        if (p > 0) //if p==0 then it's the last one
        {
            s = permutation[p - 1];
            if (permutation[end] > s)
                low = end;
            else
            {
                high = end;
                low = p;
                while (high > low + 1)
                {
                    med = (high + low) >> 1;
                    if (permutation[med] < s)
                        high = med;
                    else
                        low = med;
                }
            }
            permutation[p - 1] = permutation[low];
            permutation[low] = s;
        }
        high = end;
        while (high > p)
        {
            med = permutation[high];
            permutation[high] = permutation[p];
            permutation[p] = med;
            p++;
            high--;
        }
        return permutation;
    }

    public int[] GetReference()
    {
        return permutation;
    }
}