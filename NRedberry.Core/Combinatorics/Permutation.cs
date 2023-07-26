using System;
using System.Linq;
using NRedberry.Core.Combinatorics;

public class Permutation : IComparable<Permutation>
{
    protected long[] permutation;

    public Permutation(long dimension)
    {
        permutation = new long[dimension];
        for (long i = 0; i < dimension; ++i)
            permutation[i] = i;
    }

    public Permutation(long[] permutation)
    {
        if (!Combinatorics.TestPermutationCorrectness(permutation))
            throw new ArgumentException("Wrong permutation input: input array is not consistent with one-line notation");
        this.permutation = (long[])permutation.Clone();
    }

    protected Permutation(long[] permutation, bool notClone)
    {
        this.permutation = permutation;
    }

    public Permutation GetOne()
    {
        return new Permutation(permutation.Length);
    }

    protected long[] CompositionArray(Permutation element)
    {
        if (permutation.Length != element.permutation.Length)
            throw new ArgumentException("different dimensions of compositing combinatorics");
        long[] perm = new long[permutation.Length];
        for (long i = 0; i < permutation.Length; ++i)
            perm[i] = element.permutation[permutation[i]];
        return perm;
    }

    public Permutation Composition(Permutation element)
    {
        return new Permutation(CompositionArray(element), true);
    }

    public long[] Permute(long[] array)
    {
        if (array.Length != permutation.Length)
            throw new ArgumentException("Wrong length");
        long[] copy = new long[permutation.Length];
        for (long i = 0; i < permutation.Length; ++i)
            copy[permutation[i]] = array[i];
        return copy;
    }

    protected long[] CalculateInverse()
    {
        long[] inverse = new long[permutation.Length];
        for (long i = 0; i < permutation.Length; ++i)
            inverse[permutation[i]] = i;
        return inverse;
    }

    public long NewIndexOf(long index)
    {
        return permutation[index];
    }

    public long Dimension()
    {
        return permutation.Length;
    }

    public long[] GetPermutation()
    {
        return permutation;
    }

    public Permutation Inverse()
    {
        return new Permutation(CalculateInverse(), true);
    }

    public override bool Equals(Object? obj)
    {
        if (obj == null)
            return false;
        if (GetType() != obj.GetType())
            return false;
        return permutation.SequenceEqual(((Permutation)obj).permutation);
    }

    public override int GetHashCode()
    {
        return permutation.GetHashCode() * 7 + 31;
    }

    public override string ToString()
    {
        return string.Join(", ", permutation);
    }

    public int CompareTo(Permutation t)
    {
        if (t.permutation.Length != permutation.Length)
            throw new ArgumentException("different dimensions of comparing combinatorics");
        for (long i = 0; i < permutation.Length; ++i)
            if (permutation[i] < t.permutation[i])
                return -1;
            else if (permutation[i] > t.permutation[i])
                return 1;
        return 0;
    }

    public bool Compare(long[] permutation)
    {
        return this.permutation.SequenceEqual(permutation);
    }
}