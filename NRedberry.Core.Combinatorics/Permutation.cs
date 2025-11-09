namespace NRedberry.Core.Combinatorics;

public class Permutation : IComparable<Permutation>
{
    protected readonly int[] _permutation;

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

    public Permutation One => new(_permutation.Length);

    [Obsolete("Use One property instead")]
    public Permutation GetOne() => One;

    public virtual Permutation Identity => new(_permutation.Length);

    [Obsolete("Use Identity property instead")]
    public Permutation GetIdentity() => Identity;

    protected int[] CompositionArray(Permutation element)
    {
        if (_permutation.Length != element._permutation.Length)
        {
            throw new ArgumentException("different dimensions of compositing combinatorics");
        }

        var perm = new int[_permutation.Length];
        for (var i = 0; i < _permutation.Length; ++i)
        {
            perm[i] = element._permutation[_permutation[i]];
        }

        return perm;
    }

    public Permutation Composition(Permutation element)
    {
        return new Permutation(CompositionArray(element), true);
    }

    public Permutation Composition(Permutation a, Permutation b)
    {
        throw new NotImplementedException();
    }

    public Permutation Composition(Permutation a, Permutation b, Permutation c)
    {
        throw new NotImplementedException();
    }

    public Permutation CompositionWithInverse(Permutation other)
    {
        throw new NotImplementedException();
    }

    public long[] Permute(long[] array)
    {
        if (array.Length != _permutation.Length)
        {
            throw new ArgumentException("Wrong length");
        }

        var copy = new long[_permutation.Length];
        for (long i = 0; i < _permutation.Length; ++i)
        {
            copy[_permutation[i]] = array[i];
        }

        return copy;
    }

    protected int[] CalculateInverse()
    {
        var inverse = new int[_permutation.Length];
        for (var i = 0; i < _permutation.Length; ++i)
        {
            inverse[_permutation[i]] = i;
        }

        return inverse;
    }

    public int NewIndexOf(long index)
    {
        return _permutation[index];
    }

    public int Dimension() => _permutation.Length;

    public int[] GetPermutation() => _permutation;

    public virtual Permutation Inverse() => new(CalculateInverse(), true);

    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (GetType() != obj.GetType())
        {
            return false;
        }

        return _permutation.SequenceEqual(((Permutation)obj)._permutation);
    }

    public override int GetHashCode() => _permutation.GetHashCode() * 7 + 31;

    public override string ToString() => string.Join(", ", _permutation);

    public int CompareTo(Permutation t)
    {
        if (t._permutation.Length != _permutation.Length)
        {
            throw new ArgumentException("different dimensions of comparing combinatorics");
        }

        for (long i = 0; i < _permutation.Length; ++i)
        {
            if (_permutation[i] < t._permutation[i])
                return -1;

            if (_permutation[i] > t._permutation[i])
                return 1;
        }

        return 0;
    }

    public bool Compare(int[] permutation) => _permutation.SequenceEqual(permutation);

    public virtual int Degree() => throw new NotImplementedException();
    public virtual  bool Antisymmetry() => throw new NotImplementedException();
    public virtual Permutation Pow(int exponent) => throw new NotImplementedException();
    public virtual bool IsIdentity() => throw new NotImplementedException();
}
