namespace NRedberry.Core.Combinatorics;

/// <summary>
/// Iterator over all permutations of specified dimension in one-line notation.
/// </summary>
/// <remarks>
/// The iterator is implemented such that each next permutation will be calculated only on the invocation of Current.
/// Note: Current returns the same reference on each invocation, so clone the array if you need to keep it.
/// </remarks>
public sealed class IntPermutationsGenerator : IntCombinatorialGenerator, IIntCombinatorialPort
{
    private readonly int[] permutation;
    private bool onFirst = true;
    private readonly int size;

    /// <summary>
    /// Construct iterator over all permutations with specified dimension starting with identity.
    /// </summary>
    /// <param name="dimension">Dimension of permutations.</param>
    public IntPermutationsGenerator(int dimension)
    {
        permutation = new int[dimension];
        for (var i = 0; i < dimension; ++i)
        {
            permutation[i] = i;
        }

        size = dimension;
    }

    /// <summary>
    /// Construct iterator over permutations with specified permutation at the start.
    /// </summary>
    /// <param name="permutation">Starting permutation.</param>
    public IntPermutationsGenerator(int[] permutation)
    {
        this.permutation = permutation;
        size = permutation.Length;
        for (var i = 0; i < size - 1; ++i)
        {
            if (permutation[i] >= size || permutation[i] < 0)
            {
                throw new ArgumentException($"Wrong permutation input: image of {i} element greater then degree");
            }

            for (var j = i + 1; j < size; ++j)
            {
                if (permutation[i] == permutation[j])
                {
                    throw new ArgumentException("Wrong permutation input: to elemets have the same image");
                }
            }
        }
    }

    public int[]? Take()
    {
        return MoveNext() ? Current : null;
    }

    public override bool MoveNext()
    {
        return !IsLast() || onFirst;
    }

    public bool HasPrevious()
    {
        return !IsFirst();
    }

    public override void Reset()
    {
        onFirst = true;
        for (var i = 0; i < size; ++i)
        {
            permutation[i] = i;
        }
    }

    private bool IsLast()
    {
        for (var i = 0; i < size; i++)
        {
            if (permutation[i] != size - 1 - i)
            {
                return false;
            }
        }

        return true;
    }

    private bool IsFirst()
    {
        for (var i = 0; i < size; i++)
        {
            if (permutation[i] != i)
            {
                return false;
            }
        }

        return true;
    }

    public override int[] Current => Next();

    public int[] Next()
    {
        if (onFirst)
        {
            onFirst = false;
            return permutation;
        }

        var end = size - 1;
        int p = end;
        int high;
        int med;
        while ((p > 0) && (permutation[p] < permutation[p - 1]))
        {
            p--;
        }

        if (p > 0)
        {
            var s = permutation[p - 1];
            int low;
            if (permutation[end] > s)
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
                    if (permutation[med] < s)
                    {
                        high = med;
                    }
                    else
                    {
                        low = med;
                    }
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

    public int[] Previous()
    {
        var nm1 = size - 1;
        int p = nm1;
        int high;
        int s;
        while ((p > 0) && (permutation[p] > permutation[p - 1]))
        {
            p--;
        }

        if (p > 0)
        {
            s = permutation[p - 1];
            int low;
            if (permutation[nm1] < s)
            {
                low = nm1;
            }
            else
            {
                high = nm1;
                low = p;
                while (high > low + 1)
                {
                    var m = (high + low) >> 1;
                    if (permutation[m] > s)
                    {
                        high = m;
                    }
                    else
                    {
                        low = m;
                    }
                }
            }

            permutation[p - 1] = permutation[low];
            permutation[low] = s;
        }

        high = nm1;
        while (high > p)
        {
            var m = permutation[high];
            permutation[high] = permutation[p];
            permutation[p] = m;
            p++;
            high--;
        }

        return permutation;
    }

    public int GetDimension()
    {
        return size;
    }

    public override int[] GetReference() => permutation;

    public override void Dispose()
    {
    }
}
