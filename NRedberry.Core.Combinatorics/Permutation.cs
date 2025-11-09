using System;
using System.Collections.Generic;
using System.Linq;

namespace NRedberry.Core.Combinatorics;

public class Permutation : IComparable<Permutation>
{
    protected readonly int[] _permutation;
    private int[]? _inverse;

    public Permutation()
    {
    }

    public Permutation(int dimension)
    {
        _permutation = new int[dimension];
        for (var i = 0; i < dimension; ++i)
            _permutation[i] = i;
    }

    public Permutation(int[] permutation)
    {
        if (!Combinatorics.TestPermutationCorrectness(permutation))
            throw new ArgumentException("Wrong permutation input: input array is not consistent with one-line notation");

        _permutation = (int[])permutation.Clone();
    }

    public Permutation(int[] permutation, bool notClone)
    {
        _permutation = permutation;
    }

    public Permutation GetOne()
    {
        return new Permutation(_permutation.Length);
    }

    public virtual Permutation Identity => GetIdentity();

    public virtual Permutation GetIdentity()
    {
        return new Permutation(_permutation.Length);
    }

    protected virtual int[] CompositionArray(Permutation element)
    {
        if (_permutation.Length != element._permutation.Length)
            throw new ArgumentException("different dimensions of compositing combinatorics");

        var perm = new int[_permutation.Length];
        for (var i = 0; i < _permutation.Length; ++i)
            perm[i] = element._permutation[_permutation[i]];

        return perm;
    }

    public virtual Permutation Composition(Permutation element)
    {
        return new Permutation(CompositionArray(element), true);
    }

    public virtual Permutation Composition(Permutation a, Permutation b)
    {
        return a.Composition(b);
    }

    public virtual Permutation Composition(Permutation a, Permutation b, Permutation c)
    {
        return Composition(a.Composition(b), c);
    }

    public virtual Permutation CompositionWithInverse(Permutation other)
    {
        return Composition(other.Inverse());
    }

    public long[] Permute(long[] array)
    {
        if (array.Length != _permutation.Length)
            throw new ArgumentException("Wrong length");

        var copy = new long[_permutation.Length];
        for (long i = 0; i < _permutation.Length; ++i)
            copy[_permutation[i]] = array[i];

        return copy;
    }

    protected int[] CalculateInverse()
    {
        _inverse ??= new int[_permutation.Length];
        for (var i = 0; i < _permutation.Length; ++i)
            _inverse[_permutation[i]] = i;

        return _inverse;
    }

    public int NewIndexOf(long index)
    {
        return _permutation[index];
    }

    public virtual int ImageOf(int point)
    {
        return NewIndexOf(point);
    }

    public virtual int[] ImageOf(int[] set)
    {
        var result = new int[set.Length];
        for (int i = 0; i < set.Length; ++i)
            result[i] = NewIndexOf(set[i]);
        return result;
    }

    public virtual int NewIndexOfUnderInverse(int i)
    {
        return CalculateInverse()[i];
    }

    public int Dimension() => _permutation.Length;

    public int[] GetPermutation() => _permutation;

    public virtual Permutation Inverse() => new(CalculateInverse(), true);

    public virtual bool IsIdentity()
    {
        for (int i = 0; i < _permutation.Length; ++i)
        {
            if (_permutation[i] != i)
                return false;
        }

        return true;
    }

    public virtual bool Antisymmetry()
    {
        return false;
    }

    public virtual Permutation ToSymmetry()
    {
        return this;
    }

    public virtual Permutation Negate()
    {
        return this;
    }

    public virtual Permutation Pow(int exponent)
    {
        if (exponent == 0)
            return Identity;

        var exp = Math.Abs(exponent);
        Permutation result = Identity;
        Permutation basePerm = this;
        while (exp != 0)
        {
            if ((exp & 1) == 1)
                result = result.Composition(basePerm);
            basePerm = basePerm.Composition(basePerm);
            exp >>= 1;
        }

        return exponent < 0 ? result.Inverse() : result;
    }

    public int CompareTo(Permutation t)
    {
        if (t._permutation.Length != _permutation.Length)
            throw new ArgumentException("different dimensions of comparing combinatorics");

        for (int i = 0; i < _permutation.Length; ++i)
        {
            if (_permutation[i] < t._permutation[i])
                return -1;
            if (_permutation[i] > t._permutation[i])
                return 1;
        }

        return 0;
    }

    public bool Compare(int[] permutation) => _permutation.SequenceEqual(permutation);

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;
        if (GetType() != obj.GetType())
            return false;
        return _permutation.SequenceEqual(((Permutation)obj)._permutation);
    }

    public override int GetHashCode() => _permutation.GetHashCode() * 7 + 31;

    public override string ToString() => string.Join(", ", _permutation);
}
