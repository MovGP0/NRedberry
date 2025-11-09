using System.Collections.Immutable;
using System.Numerics;
using NRedberry.Core.Combinatorics;

namespace NRedberry.Core.Groups;

/// <summary>
/// Skeleton port of cc.redberry.core.groups.permutations.PermutationOneLineAbstract.
/// </summary>
public abstract class PermutationOneLineAbstract : Permutation
{
    protected PermutationOneLineAbstract(bool isIdentity, bool antisymmetry)
    {
        throw new NotImplementedException();
    }

    public virtual bool Antisymmetry()
    {
        throw new NotImplementedException();
    }

    public virtual bool IsIdentity => throw new NotImplementedException();

    public virtual Permutation Pow(int exponent)
    {
        throw new NotImplementedException();
    }

    public virtual Permutation Identity => throw new NotImplementedException();

    public virtual Permutation CompositionWithInverse(Permutation other)
    {
        throw new NotImplementedException();
    }

    public virtual Permutation Conjugate(Permutation permutation)
    {
        throw new NotImplementedException();
    }

    public virtual Permutation Commutator(Permutation permutation)
    {
        throw new NotImplementedException();
    }

    public virtual int[] ImageOf(int[] set)
    {
        throw new NotImplementedException();
    }

    public virtual int[] Permute(int[] array)
    {
        throw new NotImplementedException();
    }

    public virtual char[] Permute(char[] array)
    {
        throw new NotImplementedException();
    }

    public virtual T[] Permute<T>(T[] array)
    {
        throw new NotImplementedException();
    }

    public virtual List<T> Permute<T>(List<T> array)
    {
        throw new NotImplementedException();
    }

    public virtual int CompareTo(Permutation? other)
    {
        throw new NotImplementedException();
    }

    public virtual int[] OneLine()
    {
        throw new NotImplementedException();
    }

    public virtual ImmutableArray<int> OneLineImmutable()
    {
        throw new NotImplementedException();
    }

    public virtual int[][] Cycles()
    {
        throw new NotImplementedException();
    }

    public virtual int NewIndexOf(int i)
    {
        throw new NotImplementedException();
    }

    public virtual int ImageOf(int i)
    {
        throw new NotImplementedException();
    }

    public virtual Permutation ToSymmetry()
    {
        throw new NotImplementedException();
    }

    public virtual Permutation Negate()
    {
        throw new NotImplementedException();
    }

    public virtual Permutation Composition(Permutation other)
    {
        throw new NotImplementedException();
    }

    public virtual Permutation Composition(Permutation a, Permutation b)
    {
        throw new NotImplementedException();
    }

    public virtual Permutation Composition(Permutation a, Permutation b, Permutation c)
    {
        throw new NotImplementedException();
    }

    public virtual Permutation Inverse()
    {
        throw new NotImplementedException();
    }

    public virtual BigInteger Order => throw new NotImplementedException();

    public virtual bool OrderIsOdd => throw new NotImplementedException();

    public virtual int Degree => throw new NotImplementedException();

    public virtual int Length => throw new NotImplementedException();

    public virtual int Parity => throw new NotImplementedException();

    public virtual Permutation MoveRight(int size)
    {
        throw new NotImplementedException();
    }

    public virtual int[] LengthsOfCycles => throw new NotImplementedException();

    public virtual string ToStringOneLine()
    {
        throw new NotImplementedException();
    }

    public virtual string ToStringCycles()
    {
        throw new NotImplementedException();
    }

    public virtual int NewIndexOfUnderInverse(int i)
    {
        throw new NotImplementedException();
    }
}
