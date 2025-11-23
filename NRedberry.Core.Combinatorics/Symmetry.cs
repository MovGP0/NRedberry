using System.Collections;
using System.Collections.Immutable;
using System.Numerics;

namespace NRedberry.Core.Combinatorics;

/// <summary>
/// C# port of cc.redberry.core.combinatorics.Symmetry.
/// Represents a permutation together with a sign (antisymmetry flag).
/// </summary>
public class Symmetry : Permutation
{
    private readonly int[] _permutation;

    private int[]? _inverse;
    private int[]? _lengthsOfCycles;
    private BigInteger? _order;
    private bool? _orderIsOdd;
    private int? _parity;

    public Symmetry(int[] permutation, bool sign)
    {
        if (!Combinatorics.TestPermutationCorrectness(permutation))
        {
            throw new ArgumentException("Wrong permutation input: input array is not consistent with one-line notation");
        }

        _permutation = (int[])permutation.Clone();
        IsAntisymmetry = sign;
        Degree = InternalDegree(_permutation);
        IsIdentity = !IsAntisymmetry && Combinatorics.IsIdentity(permutation);
        if (IsAntisymmetry && OrderOfPermutationIsOdd(_permutation))
        {
            throw new InconsistentGeneratorsException();
        }
    }

    public Symmetry(int dimension)
    {
        _permutation = new int[dimension];
        for (int i = 0; i < dimension; ++i)
        {
            _permutation[i] = i;
        }

        IsAntisymmetry = false;
        Degree = dimension;
        IsIdentity = true;
    }

    public Symmetry(int[] permutation, bool sign, bool notClone)
    {
        if (!Combinatorics.TestPermutationCorrectness(permutation))
        {
            throw new ArgumentException("Wrong permutation input: input array is not consistent with one-line notation");
        }

        _permutation = notClone ? permutation : (int[])permutation.Clone();
        IsAntisymmetry = sign;
        Degree = InternalDegree(_permutation);
        IsIdentity = !IsAntisymmetry && Combinatorics.IsIdentity(permutation);
        if (IsAntisymmetry && OrderOfPermutationIsOdd(_permutation))
        {
            throw new InconsistentGeneratorsException();
        }
    }

    public Symmetry One => new(_permutation.Length);

    /// <summary>
    /// Sign
    /// </summary>
    public bool IsAntisymmetry { get; }

    public int[] OneLine() => (int[])_permutation.Clone();

    public ImmutableArray<int> OneLineImmutable() => [.. _permutation];

    public int[][] Cycles()
    {
        var cycles = new List<int[]>();
        var seen = new bool[_permutation.Length];
        int visited = 0;

        while (visited < _permutation.Length)
        {
            int start = Array.IndexOf(seen, false);
            if (_permutation[start] == start)
            {
                seen[start] = true;
                ++visited;
                continue;
            }

            var cycle = new List<int>();
            int pointer = start;
            while (!seen[pointer])
            {
                seen[pointer] = true;
                ++visited;
                cycle.Add(pointer);
                pointer = _permutation[pointer];
            }

            cycles.Add(cycle.ToArray());
        }

        return cycles.ToArray();
    }

    public int NewIndexOf(int i) => i < Degree ? _permutation[i] : i;

    public int ImageOf(int i) => NewIndexOf(i);

    public int[] ImageOf(int[] set)
    {
        if (IsIdentity)
        {
            return (int[])set.Clone();
        }

        int[] result = new int[set.Length];
        for (int i = 0; i < set.Length; ++i)
        {
            result[i] = NewIndexOf(set[i]);
        }

        return result;
    }

    public int[] Permute(int[] array)
    {
        if (IsIdentity)
        {
            return (int[])array.Clone();
        }

        int[] result = new int[array.Length];
        for (int i = 0; i < array.Length; ++i)
        {
            result[i] = array[NewIndexOf(i)];
        }

        return result;
    }

    public char[] Permute(char[] array)
    {
        if (IsIdentity)
        {
            return (char[])array.Clone();
        }

        char[] result = new char[array.Length];
        for (int i = 0; i < array.Length; ++i)
        {
            result[i] = array[NewIndexOf(i)];
        }

        return result;
    }

    public T[] Permute<T>(T[] array)
    {
        if (IsIdentity)
        {
            return (T[])array.Clone();
        }

        T[] result = new T[array.Length];
        for (int i = 0; i < array.Length; ++i)
        {
            result[i] = array[NewIndexOf(i)];
        }

        return result;
    }

    public List<T> Permute<T>(List<T> list)
    {
        if (IsIdentity)
        {
            return [..list];
        }

        List<T> result = new List<T>(list.Count);
        for (int i = 0; i < list.Count; ++i)
        {
            result.Add(list[NewIndexOf(i)]);
        }

        return result;
    }

    public Permutation Conjugate(Permutation p)
    {
        return Inverse().Composition(p).Composition(this);
    }

    public Permutation Commutator(Permutation p)
    {
        return Inverse().Composition(p.Inverse()).Composition(this).Composition(p);
    }

    public int NewIndexOfUnderInverse(int i)
    {
        if (i >= _permutation.Length)
        {
            return i;
        }

        EnsureInverseCalculated();
        return _inverse![i];
    }

    public Permutation ToSymmetry() => IsAntisymmetry ? new Symmetry(_permutation, false, true) : this;

    public Permutation Negate() => new Symmetry(_permutation, !IsAntisymmetry, true);

    Permutation Permutation.Composition(Permutation other) => Composition(other);

    public Permutation Composition(Permutation a, Permutation b)
    {
        if (IsIdentity)
        {
            return a.Composition(b);
        }

        if (a.IsIdentity)
        {
            return Composition(b);
        }

        if (b.IsIdentity)
        {
            return Composition(a);
        }

        int newLength = Math.Max(Math.Max(Degree, a.Degree), b.Degree);
        int[] result = new int[newLength];
        for (int i = 0; i < newLength; ++i)
        {
            result[i] = b.NewIndexOf(a.NewIndexOf(NewIndexOf(i)));
        }

        try
        {
            return new Symmetry(result, IsAntisymmetry ^ a.IsAntisymmetry ^ b.IsAntisymmetry, true);
        }
        catch (InconsistentGeneratorsException)
        {
            throw new InconsistentGeneratorsException(this + " and " + a + " and " + b);
        }
    }

    public Permutation Composition(Permutation a, Permutation b, Permutation c)
    {
        if (IsIdentity)
        {
            return a.Composition(b, c);
        }

        if (a.IsIdentity)
        {
            return Composition(b, c);
        }

        if (b.IsIdentity)
        {
            return Composition(a, c);
        }

        if (c.IsIdentity)
        {
            return Composition(a, b);
        }

        int newLength = Math.Max(c.Degree, Math.Max(Math.Max(Degree, a.Degree), b.Degree));
        int[] result = new int[newLength];
        for (int i = 0; i < newLength; ++i)
        {
            result[i] = c.NewIndexOf(b.NewIndexOf(a.NewIndexOf(NewIndexOf(i))));
        }

        try
        {
            return new Symmetry(result, IsAntisymmetry ^ a.IsAntisymmetry ^ b.IsAntisymmetry ^ c.IsAntisymmetry, true);
        }
        catch (InconsistentGeneratorsException)
        {
            throw new InconsistentGeneratorsException(this + " and " + a + " and " + b + " and " + c);
        }
    }

    public Permutation CompositionWithInverse(Permutation other)
    {
        if (IsIdentity)
        {
            return other.Inverse();
        }

        if (other.IsIdentity)
        {
            return this;
        }

        return Composition(other.Inverse());
    }

    Permutation Permutation.Inverse() => Inverse();

    public bool IsIdentity { get; }

    public Permutation Identity => IsIdentity ? this : new Symmetry(_permutation.Length);

    public BigInteger Order
    {
        get
        {
            _order ??= CalculateOrder(_permutation);
            return _order.Value;
        }
    }

    public bool OrderIsOdd
    {
        get
        {
            _orderIsOdd ??= OrderOfPermutationIsOdd(_permutation);
            return _orderIsOdd.Value;
        }
    }

    public int Degree { get; }

    public int Length => _permutation.Length;

    public Permutation Pow(int exponent)
    {
        if (IsIdentity || exponent == 1)
        {
            return this;
        }

        if (exponent == 0)
        {
            return Identity;
        }

        if (exponent < 0)
        {
            return Inverse().Pow(-exponent);
        }

        Permutation result = Identity;
        Permutation basePerm = this;
        int exp = exponent;
        while (exp != 0)
        {
            if ((exp & 1) == 1)
            {
                result = result.Composition(basePerm);
            }

            basePerm = basePerm.Composition(basePerm);
            exp >>= 1;
        }

        return result;
    }

    public int Parity
    {
        get
        {
            _parity ??= CalculateParity(_permutation);
            return _parity.Value;
        }
    }

    public Permutation MoveRight(int size)
    {
        if (size == 0)
        {
            return this;
        }

        int[] moved = new int[size + _permutation.Length];
        int i = 1;
        for (; i < size; ++i)
        {
            moved[i] = i;
        }

        int offset = i;
        for (; i < moved.Length; ++i)
        {
            moved[i] = _permutation[i - offset] + size;
        }

        return new Symmetry(moved, IsAntisymmetry, true);
    }

    public int[] LengthsOfCycles
    {
        get
        {
            _lengthsOfCycles ??= CalculateLengthsOfCycles(_permutation);
            return (int[])_lengthsOfCycles.Clone();
        }
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

    public Symmetry Composition(Permutation element)
    {
        if (IsIdentity)
        {
            return (Symmetry)element;
        }

        if (element.IsIdentity)
        {
            return this;
        }

        int newLength = Math.Max(Degree, element.Degree);
        int[] result = new int[newLength];
        for (int i = 0; i < newLength; ++i)
        {
            result[i] = element.NewIndexOf(NewIndexOf(i));
        }

        try
        {
            return new Symmetry(result, IsAntisymmetry ^ element.IsAntisymmetry, true);
        }
        catch (InconsistentGeneratorsException)
        {
            throw new InconsistentGeneratorsException(this + " and " + element);
        }
    }

    public Symmetry Inverse()
    {
        if (IsIdentity)
        {
            return this;
        }

        int[] inv = new int[_permutation.Length];
        for (int i = _permutation.Length - 1; i >= 0; --i)
        {
            inv[_permutation[i]] = i;
        }

        return new Symmetry(inv, IsAntisymmetry, true);
    }

    public int CompareTo(Permutation? other)
    {
        if (other == null)
        {
            return 1;
        }

        int max = Math.Max(Degree, other.Degree);
        if (IsAntisymmetry != other.IsAntisymmetry)
        {
            return IsAntisymmetry ? -1 : 1;
        }

        for (int i = 0; i < max; ++i)
        {
            if (NewIndexOf(i) < other.NewIndexOf(i))
            {
                return -1;
            }

            if (NewIndexOf(i) > other.NewIndexOf(i))
            {
                return 1;
            }
        }

        return 0;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj is not Symmetry other)
        {
            return false;
        }

        if (IsAntisymmetry != other.IsAntisymmetry)
        {
            return false;
        }

        if (Degree != other.Degree)
        {
            return false;
        }

        for (int i = 0; i < Degree; ++i)
        {
            if (_permutation[i] != other._permutation[i])
            {
                return false;
            }
        }

        return true;
    }

    public override int GetHashCode()
    {
        int hash = 1;
        for (int i = 0; i < Degree; ++i)
        {
            hash = 31 * hash + _permutation[i];
        }

        hash = 31 * hash + (IsAntisymmetry ? 1 : 0);
        return hash;
    }

    public override string ToString() => ToStringCycles();

    public IEnumerator<int> GetEnumerator() => ((IEnumerable<int>)_permutation).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private void EnsureInverseCalculated()
    {
        if (_inverse != null)
        {
            return;
        }

        _inverse = new int[_permutation.Length];
        for (int i = _permutation.Length - 1; i >= 0; --i)
        {
            _inverse[_permutation[i]] = i;
        }
    }

    private static int InternalDegree(int[] permutation)
    {
        int max = -1;
        for (int i = 0; i < permutation.Length; ++i)
        {
            max = Math.Max(max, Math.Max(i, permutation[i]));
        }

        return max + 1;
    }

    private static int CalculateParity(int[] permutation)
    {
        var used = new bool[permutation.Length];
        int counter = 0;
        int numOfTranspositions = 0;
        while (counter < permutation.Length)
        {
            int pointer = Array.FindIndex(used, v => !v);
            int start = pointer;
            int currentSize = 0;
            do
            {
                used[pointer] = true;
                pointer = permutation[pointer];
                ++currentSize;
            }
            while (pointer != start);

            counter += currentSize;
            numOfTranspositions += currentSize - 1;
        }

        return numOfTranspositions % 2;
    }

    private static int[] CalculateLengthsOfCycles(int[] permutation)
    {
        var used = new bool[permutation.Length];
        int counter = 0;
        var sizes = new List<int>();
        while (counter < permutation.Length)
        {
            int pointer = Array.FindIndex(used, v => !v);
            int start = pointer;
            int currentSize = 0;
            do
            {
                used[pointer] = true;
                pointer = permutation[pointer];
                ++currentSize;
            }
            while (pointer != start);

            counter += currentSize;
            sizes.Add(currentSize);
        }

        return sizes.ToArray();
    }

    private static BigInteger CalculateOrder(int[] permutation)
    {
        var used = new bool[permutation.Length];
        int counter = 0;
        BigInteger lcm = BigInteger.One;
        while (counter < permutation.Length)
        {
            int pointer = Array.FindIndex(used, v => !v);
            int start = pointer;
            int currentSize = 0;
            do
            {
                used[pointer] = true;
                pointer = permutation[pointer];
                ++currentSize;
            }
            while (pointer != start);

            counter += currentSize;
            BigInteger size = new BigInteger(currentSize);
            lcm = lcm / BigInteger.GreatestCommonDivisor(lcm, size) * size;
        }

        return lcm;
    }

    private static bool OrderOfPermutationIsOdd(int[] permutation)
    {
        var used = new bool[permutation.Length];
        int counter = 0;
        while (counter < permutation.Length)
        {
            int pointer = Array.FindIndex(used, v => !v);
            int start = pointer;
            int currentSize = 0;
            do
            {
                used[pointer] = true;
                pointer = permutation[pointer];
                ++currentSize;
            }
            while (pointer != start);

            if (currentSize % 2 == 0)
            {
                return false;
            }

            counter += currentSize;
        }

        return true;
    }
}
