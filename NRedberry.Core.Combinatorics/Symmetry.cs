using System.Collections.Immutable;
using System.Numerics;

namespace NRedberry.Core.Combinatorics;

public class Symmetry : Permutation
{
    protected readonly int[] _permutation;
    private int[]? _inverse;

    private bool Sign { get; }

    // Constructor with permutation array and sign
    public Symmetry(int[] permutation, bool sign)
    {
        if (!Combinatorics.TestPermutationCorrectness(permutation))
            throw new ArgumentException("Wrong permutation input: input array is not consistent with one-line notation");

        _permutation = (int[])permutation.Clone();
        this.Sign = sign;
    }

    // Constructor with dimension
    public Symmetry(int dimension)
    {
        _permutation = new int[dimension];
        for (var i = 0; i < dimension; ++i)
            _permutation[i] = i;
        Sign = false;
    }

    public Symmetry(int[] permutation, bool sign, bool notClone)
    {
        if (!Combinatorics.TestPermutationCorrectness(permutation))
            throw new ArgumentException("Wrong permutation input: input array is not consistent with one-line notation");

        _permutation = notClone ? permutation : (int[])permutation.Clone();
        this.Sign = sign;
    }

    public new Symmetry GetOne()
    {
        return new Symmetry(_permutation.Length);
    }

    public bool IsAntiSymmetry()
    {
        return Sign;
    }

    public int[] OneLine()
    {
        throw new NotImplementedException();
    }

    public ImmutableArray<int> OneLineImmutable()
    {
        throw new NotImplementedException();
    }

    public int[][] Cycles()
    {
        throw new NotImplementedException();
    }

    public int NewIndexOf(int i)
    {
        throw new NotImplementedException();
    }

    public int ImageOf(int i)
    {
        throw new NotImplementedException();
    }

    public int[] ImageOf(int[] set)
    {
        throw new NotImplementedException();
    }

    public int[] Permute(int[] array)
    {
        throw new NotImplementedException();
    }

    public char[] Permute(char[] array)
    {
        throw new NotImplementedException();
    }

    public T[] Permute<T>(T[] array)
    {
        throw new NotImplementedException();
    }

    public List<T> Permute<T>(List<T> list)
    {
        throw new NotImplementedException();
    }

    public Permutation Conjugate(Permutation p)
    {
        throw new NotImplementedException();
    }

    public Permutation Commutator(Permutation p)
    {
        throw new NotImplementedException();
    }

    public int NewIndexOfUnderInverse(int i)
    {
        throw new NotImplementedException();
    }

    public bool Antisymmetry()
    {
        throw new NotImplementedException();
    }

    public Permutation ToSymmetry()
    {
        throw new NotImplementedException();
    }

    public Permutation Negate()
    {
        throw new NotImplementedException();
    }

    Permutation Permutation.Composition(Permutation other)
    {
        return Composition(other);
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

    Permutation Permutation.Inverse()
    {
        return Inverse();
    }

    public bool IsIdentity { get; }
    public Permutation Identity { get; }
    public BigInteger Order { get; }
    public bool OrderIsOdd { get; }
    public int Degree { get; }

    /// <summary>
    /// Number of Dimensions
    /// </summary>
    public int Length { get; }

    public Permutation Pow(int exponent)
    {
        throw new NotImplementedException();
    }

    public int Parity { get; }

    public Permutation MoveRight(int size)
    {
        throw new NotImplementedException();
    }

    public int[] LengthsOfCycles { get; }

    public string ToStringOneLine()
    {
        throw new NotImplementedException();
    }

    public string ToStringCycles()
    {
        throw new NotImplementedException();
    }

    public new Symmetry Composition(Permutation element)
    {
        throw new NotImplementedException();
    }

    public new Symmetry Inverse()
    {
        throw new NotImplementedException();
    }

    public int CompareTo(Permutation? other)
    {
        throw new NotImplementedException();
    }

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;
        if (GetType() != obj.GetType())
            return false;
        var other = (Symmetry) obj;
        if (Sign != other.Sign)
            return false;
        return _permutation.SequenceEqual(other._permutation);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode() + (Sign ? 1 : 0);
    }

    public override string ToString()
    {
        return base.ToString() + "(" + (Sign ? "-" : "+") + ")";
    }
}
