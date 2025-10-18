using System.Numerics;
using NRedberry.Core.Utils;

namespace NRedberry.Core.Groups;

public interface IPermutation : IComparable<IPermutation>
{
    int[] OneLine();
    IntArray OneLineImmutable();
    int[][] Cycles();
    int NewIndexOf(int i);
    int ImageOf(int i);
    int[] ImageOf(int[] set);
    int[] Permute(int[] array);
    char[] Permute(char[] array);
    T[] Permute<T>(T[] array);
    List<T> Permute<T>(List<T> array);
    IPermutation Conjugate(IPermutation p);
    IPermutation Commutator(IPermutation p);
    int NewIndexOfUnderInverse(int i);
    bool Antisymmetry();
    IPermutation ToSymmetry();
    IPermutation Negate();
    IPermutation Composition(IPermutation other);
    IPermutation Composition(IPermutation a, IPermutation b);
    IPermutation Composition(IPermutation a, IPermutation b, IPermutation c);
    IPermutation CompositionWithInverse(IPermutation other);
    IPermutation Inverse();
    bool IsIdentity();
    IPermutation GetIdentity();
    BigInteger Order();
    bool OrderIsOdd();
    int Degree();
    int Length();
    IPermutation Pow(int exponent);
    int Parity();
    IPermutation MoveRight(int size);
    int[] LengthsOfCycles();
    string ToStringOneLine();
    string ToStringCycles();
}