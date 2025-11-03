﻿﻿using System;
using System.Collections.Generic;
using System.Text;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// ExpVector implements exponent vectors for polynomials. Objects of this class are intended to be immutable.
/// The different storage unit implementations are <see cref="ExpVectorLong"/>, <see cref="ExpVectorInteger"/>,
/// <see cref="ExpVectorShort"/> and <see cref="ExpVectorByte"/>.
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.ExpVector
/// </remarks>
public abstract class ExpVector : AbelianGroupElem<ExpVector>
{
    protected int _hash;

    private const string FactoryNotImplementedMessage = "No factory implemented for ExpVector.";

    protected ExpVector()
    {
        _hash = 0;
    }

    public enum StorageUnit
    {
        Long,
        Int,
        Short,
        Byte
    }

    public const StorageUnit DefaultStorageUnit = StorageUnit.Long;

    private static readonly Random s_random = new ();

    public static ExpVector Create(int length)
    {
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Length must be non-negative.");
        }

        return DefaultStorageUnit switch
        {
            StorageUnit.Int => new ExpVectorInteger(length),
            StorageUnit.Long => new ExpVectorLong(length),
            StorageUnit.Short => new ExpVectorShort(length),
            StorageUnit.Byte => new ExpVectorByte(length),
            _ => new ExpVectorInteger(length)
        };
    }

    public static ExpVector Create(int length, int index, long exponent)
    {
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Length must be non-negative.");
        }

        if (index < 0 || index >= length)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Index must be within the new exponent vector range.");
        }

        return DefaultStorageUnit switch
        {
            StorageUnit.Int => new ExpVectorInteger(length, index, exponent),
            StorageUnit.Long => new ExpVectorLong(length, index, exponent),
            StorageUnit.Short => new ExpVectorShort(length, index, exponent),
            StorageUnit.Byte => new ExpVectorByte(length, index, exponent),
            _ => new ExpVectorInteger(length, index, exponent)
        };
    }

    public static ExpVector Create(long[] values)
    {
        ArgumentNullException.ThrowIfNull(values);

        return DefaultStorageUnit switch
        {
            StorageUnit.Int => new ExpVectorInteger(values),
            StorageUnit.Long => new ExpVectorLong(values),
            StorageUnit.Short => new ExpVectorShort(values),
            StorageUnit.Byte => new ExpVectorByte(values),
            _ => new ExpVectorInteger(values)
        };
    }

    public static ExpVector Create(ICollection<long> values)
    {
        ArgumentNullException.ThrowIfNull(values);
        long[] result = new long[values.Count];
        int position = 0;
        foreach (long value in values)
        {
            result[position] = value;
            position++;
        }

        return Create(result);
    }

    public virtual AbelianGroupFactory<ExpVector> Factory()
    {
        throw new NotSupportedException(FactoryNotImplementedMessage);
    }

    public virtual bool IsFinite()
    {
        return true;
    }

    public abstract ExpVector Clone();
    public abstract long[] GetVal();
    public abstract long GetVal(int index);
    public abstract int Length();

    public abstract ExpVector Extend(int count, int index, long exponent);
    public abstract ExpVector ExtendLower(int count, int index, long exponent);
    public abstract ExpVector Contract(int index, int length);
    public abstract ExpVector Reverse();
    public abstract ExpVector Reverse(int variables);
    public abstract ExpVector Combine(ExpVector vector);

    public abstract ExpVector Abs();
    public abstract ExpVector Negate();
    public virtual bool IsZero()
    {
        return Signum() == 0;
    }

    public abstract int Signum();
    public abstract ExpVector Sum(ExpVector vector);
    public abstract ExpVector Subtract(ExpVector vector);

    public virtual ExpVector Subst(int index, long exponent)
    {
        _ = index;
        _ = exponent;
        return Clone();
    }

    public long Degree()
    {
        return TotalDeg();
    }

    public abstract long TotalDeg();
    public abstract long MaxDeg();
    public abstract ExpVector Lcm(ExpVector vector);
    public abstract ExpVector Gcd(ExpVector vector);
    public abstract int[] DependencyOnVariables();
    public abstract bool MultipleOf(ExpVector vector);

    public virtual int CompareTo(ExpVector? other)
    {
        if (other is null)
        {
            return 1;
        }

        return InvLexCompareTo(other);
    }

    public static int EvIlcp(ExpVector left, ExpVector right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        return left.InvLexCompareTo(right);
    }

    public static int EvIlcp(ExpVector left, ExpVector right, int begin, int end)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        return left.InvLexCompareTo(right, begin, end);
    }

    public static int EvIglc(ExpVector left, ExpVector right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        return left.InvGradCompareTo(right);
    }

    public static int EvIglc(ExpVector left, ExpVector right, int begin, int end)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        return left.InvGradCompareTo(right, begin, end);
    }

    public static int EvRilcp(ExpVector left, ExpVector right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        return left.RevInvLexCompareTo(right);
    }

    public static int EvRilcp(ExpVector left, ExpVector right, int begin, int end)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        return left.RevInvLexCompareTo(right, begin, end);
    }

    public static int EvRiglc(ExpVector left, ExpVector right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        return left.RevInvGradCompareTo(right);
    }

    public static int EvRiglc(ExpVector left, ExpVector right, int begin, int end)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        return left.RevInvGradCompareTo(right, begin, end);
    }

    public static int EvIwlc(long[][] weights, ExpVector left, ExpVector right)
    {
        ArgumentNullException.ThrowIfNull(weights);
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        return left.InvWeightCompareTo(weights, right);
    }

    public abstract int InvLexCompareTo(ExpVector vector);
    public abstract int InvLexCompareTo(ExpVector vector, int begin, int end);
    public abstract int InvGradCompareTo(ExpVector vector);
    public abstract int InvGradCompareTo(ExpVector vector, int begin, int end);
    public abstract int RevInvLexCompareTo(ExpVector vector);
    public abstract int RevInvLexCompareTo(ExpVector vector, int begin, int end);
    public abstract int RevInvGradCompareTo(ExpVector vector);
    public abstract int RevInvGradCompareTo(ExpVector vector, int begin, int end);
    public abstract int InvWeightCompareTo(long[][] weights, ExpVector vector);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj is not ExpVector other)
        {
            return false;
        }

        if (Length() != other.Length())
        {
            return false;
        }

        return InvLexCompareTo(other) == 0;
    }

    public override int GetHashCode()
    {
        if (_hash == 0)
        {
            int computed = 0;
            int len = Length();
            for (int i = 0; i < len; i++)
            {
                computed = (computed << 4) + (int)GetVal(i);
            }

            if (computed == 0)
            {
                computed = 1;
            }

            _hash = computed;
        }

        return _hash;
    }

    public override string ToString()
    {
        int length = Length();
        if (length == 0)
        {
            return "()";
        }

        StringBuilder builder = new ();
        builder.Append('(');
        for (int i = 0; i < length; i++)
        {
            builder.Append(GetVal(i));
            if (i < length - 1)
            {
                builder.Append(',');
            }
        }

        builder.Append(')');
        return builder.ToString();
    }

    public string ToString(string[] variables)
    {
        ArgumentNullException.ThrowIfNull(variables);
        int length = Length();
        if (length != variables.Length)
        {
            return ToString();
        }

        if (length == 0)
        {
            return string.Empty;
        }

        StringBuilder builder = new ();
        for (int i = length - 1; i > 0; i--)
        {
            long exponent = GetVal(i);
            if (exponent != 0)
            {
                builder.Append(variables[length - 1 - i]);
                if (exponent != 1)
                {
                    builder.Append('^').Append(exponent);
                }

                bool pending = false;
                for (int j = i - 1; j >= 0; j--)
                {
                    if (GetVal(j) != 0)
                    {
                        pending = true;
                        break;
                    }
                }

                if (pending)
                {
                    builder.Append(" * ");
                }
            }
        }

        long lastExponent = GetVal(0);
        if (lastExponent != 0)
        {
            builder.Append(variables[length - 1]);
            if (lastExponent != 1)
            {
                builder.Append('^').Append(lastExponent);
            }
        }

        return builder.ToString();
    }

    public static string VarsToString(string[] variables)
    {
        if (variables == null)
        {
            return "null";
        }

        if (variables.Length == 0)
        {
            return string.Empty;
        }

        StringBuilder builder = new ();
        for (int i = 0; i < variables.Length; i++)
        {
            builder.Append(variables[i]);
            if (i < variables.Length - 1)
            {
                builder.Append(',');
            }
        }

        return builder.ToString();
    }

    public static ExpVector Random(int length, long maxDegree, float density)
    {
        return Random(length, maxDegree, density, s_random);
    }

    public static ExpVector Random(int length, long maxDegree, float density, Random random)
    {
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Length must be non-negative.");
        }

        if (density < 0 || density > 1)
        {
            throw new ArgumentOutOfRangeException(nameof(density), "Density must be within [0, 1].");
        }

        ArgumentNullException.ThrowIfNull(random);

        long[] values = new long[length];
        for (int i = 0; i < values.Length; i++)
        {
            float sample = (float)random.NextDouble();
            if (sample <= density && maxDegree > 0)
            {
                long exponent = random.NextInt64(maxDegree);
                values[i] = exponent;
            }
            else
            {
                values[i] = 0;
            }
        }

        return Create(values);
    }

    ElemFactory<ExpVector> Element<ExpVector>.Factory()
    {
        return Factory();
    }
}
