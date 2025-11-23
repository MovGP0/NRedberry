using System.Collections.Immutable;
using System.Numerics;
using NRedberry.Core.Combinatorics;
using NRedberry.Core.Utils;
using System.Collections;

namespace NRedberry.Groups;

public class PermutationOneLineShort : Permutation, IEnumerable<short>
{
    private readonly short[] _permutation;
    private readonly short _internalDegree;

    public PermutationOneLineShort(bool antisymmetry, params short[] permutation)
    {
        if (!Permutations.TestPermutationCorrectness(permutation, antisymmetry))
            throw new ArgumentException("Inconsistent permutation.");
        _permutation = (short[])permutation.Clone();
        IsAntisymmetry = antisymmetry;
        IsIdentity = Permutations.IsIdentity(permutation);
        _internalDegree = Permutations.InternalDegree(permutation);
    }

    public PermutationOneLineShort(
        bool isIdentity,
        bool antisymmetry,
        short internalDegree,
        short[] permutation)
    {
        IsIdentity = isIdentity;
        _permutation = permutation;
        IsAntisymmetry = antisymmetry;
        _internalDegree = internalDegree;
        if (antisymmetry && Permutations.OrderOfPermutationIsOdd(permutation))
            throw new InconsistentGeneratorsException();
    }

    public PermutationOneLineShort(
        bool isIdentity,
        bool antisymmetry,
        short internalDegree,
        short[] permutation,
        bool identity)
    {
        IsIdentity = isIdentity;
        _permutation = permutation;
        IsAntisymmetry = antisymmetry;
        _internalDegree = internalDegree;
    }

    public PermutationOneLineInt ToIntRepresentation()
    {
        return new PermutationOneLineInt(
            IsIdentity,
            IsAntisymmetry,
            _internalDegree,
            ArraysUtils.ShortToInt(_permutation));
    }

    public int Length => _permutation.Length;

    public bool IsAntisymmetry { get; }

    public Permutation ToSymmetry()
    {
        return IsAntisymmetry ? new PermutationOneLineShort(IsIdentity, false, _internalDegree, _permutation, true) : this;
    }

    public Permutation Negate()
    {
        return new PermutationOneLineShort(false, IsAntisymmetry ^ true, _internalDegree, _permutation);
    }

    public int[] OneLine()
    {
        return ArraysUtils.ShortToInt(_permutation);
    }

    public ImmutableArray<int> OneLineImmutable()
    {
        return [..ArraysUtils.ShortToInt(_permutation)];
    }

    public int[][] Cycles()
    {
        return Permutations.ConvertOneLineToCycles(_permutation);
    }

    public int NewIndexOf(int i)
    {
        return i < _internalDegree ? _permutation[i] : i;
    }

    public int ImageOf(int i)
    {
        return i < _internalDegree ? _permutation[i] : i;
    }

    public int[] ImageOf(int[] set)
    {
        if (IsIdentity)
            return (int[])set.Clone();
        int[] result = new int[set.Length];
        for (int i = 0; i < set.Length; ++i)
            result[i] = NewIndexOf(set[i]);
        return result;
    }

    public int[] Permute(int[] array)
    {
        if (IsIdentity)
            return (int[])array.Clone();
        int[] result = new int[array.Length];
        for (int i = 0; i < array.Length; ++i)
            result[i] = array[NewIndexOf(i)];
        return result;
    }

    public char[] Permute(char[] array)
    {
        if (IsIdentity)
            return (char[])array.Clone();
        char[] result = new char[array.Length];
        for (int i = 0; i < array.Length; ++i)
            result[i] = array[NewIndexOf(i)];
        return result;
    }

    public T[] Permute<T>(T[] array)
    {
        if (IsIdentity)
            return (T[])array.Clone();
        T[] result = new T[array.Length];
        for (int i = 0; i < array.Length; ++i)
            result[i] = array[NewIndexOf(i)];
        return result;
    }

    public List<T> Permute<T>(List<T> set)
    {
        if (IsIdentity)
            return [..set];
        List<T> list = new List<T>(set.Count);
        for (int i = 0; i < set.Count; ++i)
            list.Add(set[NewIndexOf(i)]);
        return list;
    }

    public int NewIndexOfUnderInverse(int i)
    {
        if (i >= _permutation.Length)
            return i;
        for (int j = _permutation.Length - 1; j >= 0; --j)
        {
            if (_permutation[j] == i)
                return j;
        }

        throw new IndexOutOfRangeException();
    }

    public Permutation Conjugate(Permutation p)
    {
        return Inverse().Composition(p).Composition(this);
    }

    public Permutation Commutator(Permutation p)
    {
        return Inverse().Composition(p.Inverse()).Composition(this).Composition(p);
    }

    public Permutation Composition(Permutation other)
    {
        if (IsIdentity)
            return other;
        if (other.IsIdentity)
            return this;

        int newLength = Math.Max(Degree, other.Degree);
        if (newLength > short.MaxValue)
            return ToIntRepresentation().Composition(other);

        short newInternalDegree = -1;
        short[] result = new short[newLength];
        bool resultIsIdentity = true;
        for (short i = 0; i < newLength; ++i)
        {
            result[i] = (short)other.NewIndexOf(NewIndexOf(i));
            resultIsIdentity &= result[i] == i;
            newInternalDegree = result[i] == i ? newInternalDegree : i;
        }

        try
        {
            return new PermutationOneLineShort(
                resultIsIdentity,
                IsAntisymmetry ^ other.IsAntisymmetry,
                (short)(newInternalDegree + 1),
                result);
        }
        catch (InconsistentGeneratorsException ex)
        {
            throw new InconsistentGeneratorsException(this + " and " + other);
        }
    }

    public Permutation Composition(Permutation a, Permutation b)
    {
        if (IsIdentity)
            return a.Composition(b);
        if (a.IsIdentity)
            return Composition(b);
        if (b.IsIdentity)
            return Composition(a);

        int newLength = Math.Max(Math.Max(Degree, a.Degree), b.Degree);
        if (newLength > short.MaxValue)
            return ToIntRepresentation().Composition(a, b);

        short newInternalDegree = -1;
        short[] result = new short[newLength];
        bool resultIsIdentity = true;
        for (short i = 0; i < newLength; ++i)
        {
            result[i] = (short)b.NewIndexOf(a.NewIndexOf(NewIndexOf(i)));
            resultIsIdentity &= result[i] == i;
            newInternalDegree = result[i] == i ? newInternalDegree : i;
        }

        try
        {
            return new PermutationOneLineShort(
                resultIsIdentity,
                IsAntisymmetry ^ a.IsAntisymmetry ^ b.IsAntisymmetry,
                (short)(newInternalDegree + 1),
                result);
        }
        catch (InconsistentGeneratorsException ex)
        {
            throw new InconsistentGeneratorsException(this + " and " + a + " and " + b);
        }
    }

    public Permutation Composition(Permutation a, Permutation b, Permutation c)
    {
        if (IsIdentity)
            return a.Composition(b, c);
        if (a.IsIdentity)
            return Composition(b, c);
        if (b.IsIdentity)
            return Composition(a, c);
        if (c.IsIdentity)
            return Composition(a, b);

        int newLength = Math.Max(
            c.Degree,
            Math.Max(
                Math.Max(
                    Degree,
                    a.Degree),
                b.Degree));

        if (newLength > short.MaxValue)
            return ToIntRepresentation().Composition(a, b, c);

        short[] result = new short[newLength];
        short newInternalDegree = -1;
        bool resultIsIdentity = true;
        for (short i = 0; i < newLength; ++i)
        {
            result[i] = (short)c.NewIndexOf(b.NewIndexOf(a.NewIndexOf(NewIndexOf(i))));
            resultIsIdentity &= result[i] == i;
            newInternalDegree = result[i] == i ? newInternalDegree : i;
        }

        try
        {
            return new PermutationOneLineShort(
                resultIsIdentity,
                IsAntisymmetry ^ a.IsAntisymmetry ^ b.IsAntisymmetry ^ c.IsAntisymmetry,
                (short)(newInternalDegree + 1),
                result);
        }
        catch (InconsistentGeneratorsException ex)
        {
            throw new InconsistentGeneratorsException(this + " and " + a + " and " + b + " and " + c);
        }
    }

    public Permutation CompositionWithInverse(Permutation other)
    {
        if (IsIdentity)
            return other.Inverse();
        if (other.IsIdentity)
            return this;

        return Composition(other.Inverse());
    }

    public Permutation Inverse()
    {
        if (IsIdentity)
            return this;
        short[] inv = new short[_permutation.Length];
        for (short i = (short)(_permutation.Length - 1); i >= 0; --i)
            inv[_permutation[i]] = i;

        return new PermutationOneLineShort(false, IsAntisymmetry, _internalDegree, inv, true);
    }

    public bool IsIdentity { get; }

    public Permutation Identity
    {
        get
        {
            if (IsIdentity)
                return this;
            return Permutations.CreateIdentityPermutation(_permutation.Length);
        }
    }

    public BigInteger Order => Permutations.OrderOfPermutation(_permutation);

    public bool OrderIsOdd => !IsIdentity && Permutations.OrderOfPermutationIsOdd(_permutation);

    public int Degree => _internalDegree;

    public Permutation Pow(int exponent)
    {
        if (IsIdentity)
            return this;
        if (exponent < 0)
            return Inverse().Pow(-exponent);
        Permutation basePerm = this;
        Permutation result = Identity;
        while (exponent != 0)
        {
            if ((exponent % 2) == 1)
                result = result.Composition(basePerm);
            basePerm = basePerm.Composition(basePerm);
            exponent >>= 1;
        }

        return result;
    }

    public override bool Equals(object? obj)
    {
        if (this == obj)
            return true;
        if (obj == null || !(obj is Permutation))
            return false;

        Permutation that = (Permutation)obj;
        if (IsAntisymmetry != that.IsAntisymmetry)
        {
            return false;
        }

        if (_internalDegree != that.Degree)
        {
            return false;
        }

        for (int i = 0; i < _internalDegree; ++i)
        {
            if (NewIndexOf(i) != that.NewIndexOf(i))
            {
                return false;
            }
        }

        return true;
    }

    public override int GetHashCode()
    {
        int result = 1;
        for (int i = 0; i < _internalDegree; ++i)
            result = 31 * result + _permutation[i];
        result = 31 * result + (IsAntisymmetry ? 1 : 0);
        return result;
    }

    public int Parity => Permutations.Parity(_permutation);

    public Permutation MoveRight(int size)
    {
        if (size == 0)
            return this;
        if (size + _permutation.Length > short.MaxValue)
            return ToIntRepresentation().MoveRight(size);

        short[] p = new short[size + _permutation.Length];
        short i = 1;
        for (; i < size; ++i)
            p[i] = i;
        int k = i;
        for (; i < p.Length; ++i)
            p[i] = (short)(_permutation[i - k] + size);
        return new PermutationOneLineShort(IsIdentity, IsAntisymmetry, (short)(size + _internalDegree), p, true);
    }

    public int[] LengthsOfCycles => Permutations.LengthsOfCycles(_permutation);

    public override string ToString()
    {
        return ToStringCycles();
    }

    public string ToStringOneLine()
    {
        return (IsAntisymmetry ? "-" : "+") + string.Join(", ", _permutation);
    }

    public string ToStringCycles()
    {
        string cycles = string.Join(", ", Cycles().Select(c => "{" + string.Join(", ", c) + "}"));
        return (IsAntisymmetry ? "-" : "+") + cycles;
    }

    public int CompareTo(Permutation t)
    {
        int max = Math.Max(Degree, t.Degree);
        if (IsAntisymmetry != t.IsAntisymmetry)
            return IsAntisymmetry ? -1 : 1;
        for (int i = 0; i < max; ++i)
        {
            if (NewIndexOf(i) < t.NewIndexOf(i))
                return -1;
            if (NewIndexOf(i) > t.NewIndexOf(i))
                return 1;
        }

        return 0;
    }

    public IEnumerator<short> GetEnumerator() => ((IEnumerable<short>)_permutation).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
