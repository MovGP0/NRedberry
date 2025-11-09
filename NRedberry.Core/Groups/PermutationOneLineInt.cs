using System.Collections.Immutable;
using System.Numerics;
using NRedberry.Core.Combinatorics;

namespace NRedberry.Core.Groups;

public class PermutationOneLineInt : Permutation
{
    private readonly int[] _permutation;
    private readonly bool _antisymmetry;
    private BigInteger _order;
    private bool _orderIsOdd;
    private int _parity;
    private int[] _lengthsOfCycles;

    public PermutationOneLineInt(bool antisymmetry, int[][] cycles)
        : this(antisymmetry, Permutations.ConvertCyclesToOneLine(cycles))
    {
    }

    public PermutationOneLineInt(int[][] cycles)
        : this(Permutations.ConvertCyclesToOneLine(cycles))
    {
    }

    public PermutationOneLineInt(params int[] permutation)
        : this(false, permutation)
    {
    }

    public PermutationOneLineInt(bool antisymmetry, params int[] permutation)
    {
        if (!Permutations.TestPermutationCorrectness(permutation, antisymmetry))
            throw new ArgumentException("Inconsistent permutation.");
        _permutation = (int[])permutation.Clone();
        _antisymmetry = antisymmetry;
        IsIdentity = Permutations.IsIdentity(permutation);
        Degree = Permutations.InternalDegree(permutation);
    }

    public PermutationOneLineInt(
        bool isIdentity,
        bool antisymmetry,
        int internalDegree,
        int[] permutation)
    {
        IsIdentity = isIdentity;
        _permutation = permutation;
        _antisymmetry = antisymmetry;
        Degree = internalDegree;
        if (antisymmetry && Permutations.OrderOfPermutationIsOdd(permutation))
            throw new InconsistentGeneratorsException();
    }

    public PermutationOneLineInt(
        bool isIdentity,
        bool antisymmetry,
        int internalDegree,
        int[] permutation,
        bool identity)
    {
        IsIdentity = isIdentity;
        _permutation = permutation;
        _antisymmetry = antisymmetry;
        Degree = internalDegree;
    }

    public int Length => _permutation.Length;

    public bool Antisymmetry()
    {
        return _antisymmetry;
    }

    public Permutation ToSymmetry()
    {
        return _antisymmetry ? new PermutationOneLineInt(IsIdentity, false, Degree, _permutation, true) : this;
    }

    public Permutation Negate()
    {
        return new PermutationOneLineInt(false, _antisymmetry ^ true, Degree, _permutation);
    }

    public int[] OneLine()
    {
        return (int[])_permutation.Clone();
    }

    public ImmutableArray<int> OneLineImmutable()
    {
        return [.._permutation];
    }

    public int[][] Cycles()
    {
        return Permutations.ConvertOneLineToCycles(_permutation);
    }

    public int NewIndexOf(int i)
    {
        return i < Degree ? _permutation[i] : i;
    }

    public int ImageOf(int i)
    {
        return i < Degree ? _permutation[i] : i;
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
        int newInternalDegree = -1;
        int[] result = new int[newLength];
        bool resultIsIdentity = true;
        for (int i = 0; i < newLength; ++i)
        {
            result[i] = other.NewIndexOf(NewIndexOf(i));
            resultIsIdentity &= result[i] == i;
            newInternalDegree = result[i] == i ? newInternalDegree : i;
        }

        try
        {
            return new PermutationOneLineInt(
                resultIsIdentity,
                _antisymmetry ^ other.Antisymmetry(),
                newInternalDegree + 1,
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
        int newInternalDegree = -1;
        int[] result = new int[newLength];
        bool resultIsIdentity = true;
        for (int i = 0; i < newLength; ++i)
        {
            result[i] = b.NewIndexOf(a.NewIndexOf(NewIndexOf(i)));
            resultIsIdentity &= result[i] == i;
            newInternalDegree = result[i] == i ? newInternalDegree : i;
        }

        try
        {
            return new PermutationOneLineInt(
                resultIsIdentity,
                _antisymmetry ^ a.Antisymmetry() ^ b.Antisymmetry(),
                newInternalDegree + 1,
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

        int newLength = Math.Max(c.Degree, Math.Max(Math.Max(Degree, a.Degree), b.Degree));
        int[] result = new int[newLength];
        int newInternalDegree = -1;
        bool resultIsIdentity = true;
        for (int i = 0; i < newLength; ++i)
        {
            result[i] = c.NewIndexOf(b.NewIndexOf(a.NewIndexOf(NewIndexOf(i))));
            resultIsIdentity &= result[i] == i;
            newInternalDegree = result[i] == i ? newInternalDegree : i;
        }

        try
        {
            return new PermutationOneLineInt(
                resultIsIdentity,
                _antisymmetry ^ a.Antisymmetry() ^ b.Antisymmetry() ^ c.Antisymmetry(),
                newInternalDegree + 1,
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
        int[] inv = new int[_permutation.Length];
        for (int i = _permutation.Length - 1; i >= 0; --i)
            inv[_permutation[i]] = i;

        return new PermutationOneLineInt(false, _antisymmetry, Degree, inv, true);
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

    BigInteger Permutation.Order => Permutations.OrderOfPermutation(_permutation);

    bool Permutation.OrderIsOdd => !IsIdentity && Permutations.OrderOfPermutationIsOdd(_permutation);

    public int Degree { get; }

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
        if (_antisymmetry != that.Antisymmetry())
            return false;
        if (Degree != that.Degree)
            return false;
        for (int i = 0; i < Degree; ++i)
        {
            if (NewIndexOf(i) != that.NewIndexOf(i))
                return false;
        }

        return true;
    }

    public override int GetHashCode()
    {
        int result = 1;
        for (int i = 0; i < Degree; ++i)
            result = 31 * result + _permutation[i];
        result = 31 * result + (_antisymmetry ? 1 : 0);
        return result;
    }

    public int Parity => Permutations.Parity(_permutation);

    public Permutation MoveRight(int size)
    {
        if (size == 0)
            return this;
        int[] p = new int[size + _permutation.Length];
        int i = 1;
        for (; i < size; ++i)
            p[i] = i;
        int k = i;
        for (; i < p.Length; ++i)
            p[i] = _permutation[i - k] + size;
        return new PermutationOneLineInt(IsIdentity, _antisymmetry, size + Degree, p, true);
    }

    int[] Permutation.LengthsOfCycles => _lengthsOfCycles;

    public int[] LengthsOfCycles()
    {
        return Permutations.LengthsOfCycles(_permutation);
    }

    public override string ToString()
    {
        return ToStringCycles();
    }

    public string ToStringOneLine()
    {
        return (_antisymmetry ? "-" : "+") + string.Join(", ", _permutation);
    }

    public string ToStringCycles()
    {
        string cycles = string.Join(", ", Cycles().Select(c => "{" + string.Join(", ", c) + "}"));
        return (_antisymmetry ? "-" : "+") + cycles;
    }

    public int CompareTo(Permutation t)
    {
        int max = Math.Max(Degree, t.Degree);
        if (_antisymmetry != t.Antisymmetry())
            return _antisymmetry ? -1 : 1;
        for (int i = 0; i < max; ++i)
        {
            if (NewIndexOf(i) < t.NewIndexOf(i))
                return -1;
            if (NewIndexOf(i) > t.NewIndexOf(i))
                return 1;
        }

        return 0;
    }
}
