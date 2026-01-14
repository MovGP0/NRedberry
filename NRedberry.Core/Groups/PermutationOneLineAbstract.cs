using System.Collections;
using System.Collections.Immutable;
using System.Numerics;
using System.Text;
using NRedberry.Core.Combinatorics;

namespace NRedberry.Groups;

/// <summary>
/// Port of cc.redberry.core.groups.permutations.PermutationOneLineAbstract.
/// </summary>
public abstract class PermutationOneLineAbstract : Permutation, IEnumerable<int>, IEquatable<Permutation>
{
    protected PermutationOneLineAbstract(bool isIdentity, bool antisymmetry)
    {
        IsIdentity = isIdentity;
        IsAntisymmetry = antisymmetry;
    }

    public bool IsAntisymmetry { get; }

    public bool IsIdentity { get; }

    public virtual Permutation Pow(int exponent)
    {
        if (IsIdentity)
        {
            return this;
        }

        if (exponent < 0)
        {
            return Inverse().Pow(-exponent);
        }

        Permutation basePerm = this;
        Permutation result = Identity;
        while (exponent != 0)
        {
            if ((exponent % 2) == 1)
            {
                result = result.Composition(basePerm);
            }

            basePerm = basePerm.Composition(basePerm);
            exponent >>= 1;
        }

        return result;
    }

    public virtual Permutation Identity => IsIdentity ? this : Permutations.CreateIdentityPermutation(Length);

    public virtual Permutation CompositionWithInverse(Permutation other)
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

    public virtual Permutation Conjugate(Permutation permutation)
    {
        return Inverse().Composition(permutation, this);
    }

    public virtual Permutation Commutator(Permutation permutation)
    {
        return Inverse().Composition(permutation.Inverse(), this, permutation);
    }

    public virtual int[] ImageOf(int[] set)
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

    public virtual int[] Permute(int[] array)
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

    public virtual char[] Permute(char[] array)
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

    public virtual T[] Permute<T>(T[] array)
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

    public virtual List<T> Permute<T>(List<T> array)
    {
        if (IsIdentity)
        {
            return [..array];
        }

        List<T> list = new List<T>(array.Count);
        for (int i = 0; i < array.Count; ++i)
        {
            list.Add(array[NewIndexOf(i)]);
        }

        return list;
    }

    public virtual int CompareTo(Permutation? other)
    {
        if (other is null)
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

    public abstract int[] OneLine();

    public abstract ImmutableArray<int> OneLineImmutable();

    public virtual int[][] Cycles()
    {
        List<int[]> cycles = [];
        BitArray seen = new BitArray(Degree);
        int counter = 0;
        while (counter < Degree)
        {
            int start = NextZeroBit(seen);
            if (NewIndexOf(start) == start)
            {
                ++counter;
                seen.Set(start, true);
                continue;
            }

            List<int> cycle = [];
            while (!seen.Get(start))
            {
                seen.Set(start, true);
                ++counter;
                cycle.Add(start);
                start = NewIndexOf(start);
            }

            cycles.Add(cycle.ToArray());
        }

        return cycles.ToArray();
    }

    public abstract int NewIndexOf(int i);

    public abstract int ImageOf(int i);

    public abstract Permutation ToSymmetry();

    public abstract Permutation Negate();

    public abstract Permutation Composition(Permutation other);

    public abstract Permutation Composition(Permutation a, Permutation b);

    public abstract Permutation Composition(Permutation a, Permutation b, Permutation c);

    public abstract Permutation Inverse();

    public abstract BigInteger Order { get; }

    public abstract bool OrderIsOdd { get; }

    public abstract int Degree { get; }

    public abstract int Length { get; }

    public abstract int Parity { get; }

    public abstract Permutation MoveRight(int size);

    public abstract int[] LengthsOfCycles { get; }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return obj is Permutation permutation && Equals(permutation);
    }

    public bool Equals(Permutation? other)
    {
        if (other is null)
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
            if (NewIndexOf(i) != other.NewIndexOf(i))
            {
                return false;
            }
        }

        return true;
    }

    public static bool operator ==(PermutationOneLineAbstract? left, PermutationOneLineAbstract? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(PermutationOneLineAbstract? left, PermutationOneLineAbstract? right)
    {
        return !Equals(left, right);
    }

    public override int GetHashCode()
    {
        int result = 1;
        for (int i = 0; i < Degree; ++i)
        {
            result = 31 * result + NewIndexOf(i);
        }

        result = 31 * result + (IsAntisymmetry ? 1 : 0);
        return result;
    }

    public override string ToString()
    {
        return ToStringCycles();
    }

    public virtual string ToStringOneLine()
    {
        var builder = new StringBuilder();
        builder.Append(IsAntisymmetry ? "-" : "+");
        for (int i = 0; i < Degree; ++i)
        {
            if (i > 0)
            {
                builder.Append(", ");
            }

            builder.Append(NewIndexOf(i));
        }

        return builder.ToString();
    }

    public virtual string ToStringCycles()
    {
        int[][] cycles = Cycles();
        var builder = new StringBuilder();
        builder.Append(IsAntisymmetry ? "-" : "+");
        for (int i = 0; i < cycles.Length; ++i)
        {
            if (i > 0)
            {
                builder.Append(", ");
            }

            builder.Append('{');
            int[] cycle = cycles[i];
            for (int j = 0; j < cycle.Length; ++j)
            {
                if (j > 0)
                {
                    builder.Append(", ");
                }

                builder.Append(cycle[j]);
            }

            builder.Append('}');
        }

        return builder.ToString();
    }

    public abstract int NewIndexOfUnderInverse(int i);

    public virtual IEnumerator<int> GetEnumerator() => ((IEnumerable<int>)OneLine()).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private static int NextZeroBit(BitArray bitArray)
    {
        for (int i = 0; i < bitArray.Length; ++i)
        {
            if (!bitArray.Get(i))
            {
                return i;
            }
        }

        return -1;
    }
}
