namespace NRedberry.Core.Combinatorics;

public sealed class IntPermutationsGenerator : IIntCombinatorialGenerator
{
    internal readonly int[] Permutation;
    private bool onFirst = true;
    private readonly int size;

    public IntPermutationsGenerator(int dimension)
    {
        Permutation = new int[dimension];
        for (var i = 0; i < dimension; ++i)
        {
            Permutation[i] = i;
        }

        size = dimension;
    }

    public IntPermutationsGenerator(int[] permutation)
    {
        this.Permutation = permutation;
        size = permutation.Length;
        for (var i = 0; i < size - 1; ++i)
        {
            if (permutation[i] >= size || permutation[i] < 0)
            {
                throw new ArgumentException($"Wrong permutation input: image of {i} element greater than degree");
            }

            for (var j = i + 1; j < size; ++j)
            {
                if (permutation[i] == permutation[j])
                {
                    throw new ArgumentException("Wrong permutation input: two elements have the same image");
                }
            }
        }
    }

    public bool MoveNext() => !IsLast() || onFirst;

    public void Reset()
    {
        onFirst = true;
        for (var i = 0; i < size; ++i)
        {
            Permutation[i] = i;
        }
    }

    public int[] Current => Next();

    object System.Collections.IEnumerator.Current => Current;

    public void Dispose() { }

    public IEnumerator<int[]> GetEnumerator() => this;

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

    private bool IsLast()
    {
        for (var i = 0; i < size; i++)
        {
            if (Permutation[i] != size - 1 - i)
            {
                return false;
            }
        }

        return true;
    }

    public int[] Next()
    {
        if (onFirst)
        {
            onFirst = false;
            return Permutation;
        }

        var end = size - 1;
        int p = end;
        int high, med;
        while ((p > 0) && (Permutation[p] < Permutation[p - 1]))
        {
            p--;
        }

        if (p > 0) //if p==0 then it's the last one
        {
            var s = Permutation[p - 1];
            int low;
            if (Permutation[end] > s)
            {
                low = end;
            }
            else
            {
                high = end;
                low = p;
                while (high > low + 1)
                {
                    med = (high + low) >> 1;
                    if (Permutation[med] < s)
                    {
                        high = med;
                    }
                    else
                    {
                        low = med;
                    }
                }
            }

            Permutation[p - 1] = Permutation[low];
            Permutation[low] = s;
        }

        high = end;
        while (high > p)
        {
            med = Permutation[high];
            Permutation[high] = Permutation[p];
            Permutation[p] = med;
            p++;
            high--;
        }

        return Permutation;
    }

    public int[] GetReference() => Permutation;
}