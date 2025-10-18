using System.Numerics;
using NRedberry.Core.Utils;

namespace NRedberry.Core.Groups;

/// <summary>
/// Skeleton port of cc.redberry.core.groups.permutations.PermutationOneLineAbstract.
/// </summary>
public abstract class PermutationOneLineAbstract : IPermutation
{
    protected PermutationOneLineAbstract(bool isIdentity, bool antisymmetry)
    {
        throw new NotImplementedException();
    }

    public virtual bool Antisymmetry()
    {
        throw new NotImplementedException();
    }

    public virtual bool IsIdentity()
    {
        throw new NotImplementedException();
    }

    public virtual IPermutation Pow(int exponent)
    {
        throw new NotImplementedException();
    }

    public virtual IPermutation GetIdentity()
    {
        throw new NotImplementedException();
    }

    public virtual IPermutation CompositionWithInverse(IPermutation other)
    {
        throw new NotImplementedException();
    }

    public virtual IPermutation Conjugate(IPermutation permutation)
    {
        throw new NotImplementedException();
    }

    public virtual IPermutation Commutator(IPermutation permutation)
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

    public virtual int CompareTo(IPermutation? other)
    {
        throw new NotImplementedException();
    }

    public virtual int[] OneLine()
    {
        throw new NotImplementedException();
    }

    public virtual IntArray OneLineImmutable()
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

    public virtual IPermutation ToSymmetry()
    {
        throw new NotImplementedException();
    }

    public virtual IPermutation Negate()
    {
        throw new NotImplementedException();
    }

    public virtual IPermutation Composition(IPermutation other)
    {
        throw new NotImplementedException();
    }

    public virtual IPermutation Composition(IPermutation a, IPermutation b)
    {
        throw new NotImplementedException();
    }

    public virtual IPermutation Composition(IPermutation a, IPermutation b, IPermutation c)
    {
        throw new NotImplementedException();
    }

    public virtual IPermutation Inverse()
    {
        throw new NotImplementedException();
    }

    public virtual BigInteger Order()
    {
        throw new NotImplementedException();
    }

    public virtual bool OrderIsOdd()
    {
        throw new NotImplementedException();
    }

    public virtual int Degree()
    {
        throw new NotImplementedException();
    }

    public virtual int Length()
    {
        throw new NotImplementedException();
    }

    public virtual int Parity()
    {
        throw new NotImplementedException();
    }

    public virtual IPermutation MoveRight(int size)
    {
        throw new NotImplementedException();
    }

    public virtual int[] LengthsOfCycles()
    {
        throw new NotImplementedException();
    }

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
