using System.Text;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// ExpVector implements exponent vectors for polynomials and exposes MAS-style helpers for different ordering,
/// arithmetic, and comparison strategies.
/// Instances behave as immutable once created, and the static factories choose a concrete storage implementation.
/// </summary>
/// <remarks>
/// The different storage unit implementations are <see cref="ExpVectorLong"/>, <see cref="ExpVectorInteger"/>,
/// <see cref="ExpVectorShort"/>, and <see cref="ExpVectorByte"/>. The <see cref="DefaultStorageUnit"/> should be
/// kept constant at runtime to mirror the Java <c>storunit</c> configuration.
/// </remarks>
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

    /// <summary>
    /// Supported storage representations for exponent arrays.
    /// </summary>
    public enum StorageUnit
    {
        Long,
        Int,
        Short,
        Byte
    }

    /// <summary>
    /// Selects the storage implementation used by the static factory helpers.
    /// </summary>
    public const StorageUnit DefaultStorageUnit = StorageUnit.Long;

    private static readonly Random s_random = new ();

    /// <summary>
    /// Creates an exponent vector of the requested <paramref name="length"/>,
    /// picking the concrete storage unit defined by <see cref="DefaultStorageUnit"/>.
    /// </summary>
    /// <param name="length">Number of variables in the exponent vector.</param>
    /// <returns>New exponent vector with all exponents set to zero.</returns>
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

    /// <summary>
    /// Creates an exponent vector of the requested <paramref name="length"/> with a single exponent set.
    /// </summary>
    /// <param name="length">Number of variables in the exponent vector.</param>
    /// <param name="index">Index to set to <paramref name="exponent"/>.</param>
    /// <param name="exponent">Exponent value to place at the specified index.</param>
    /// <returns>New exponent vector with the requested entry initialized.</returns>
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

    /// <summary>
    /// Creates an exponent vector from the given array representation.
    /// </summary>
    /// <param name="values">Array of exponents, one per variable.</param>
    /// <returns>New exponent vector with the provided entries.</returns>
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

    /// <summary>
    /// Creates an exponent vector from an enumerable collection of exponents.
    /// </summary>
    /// <param name="values">Collection whose values are copied into the exponent vector.</param>
    /// <returns>New exponent vector containing the supplied exponents.</returns>
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

    /// <summary>
    /// Placeholder for the MAS element factory; not implemented for ExpVector.
    /// </summary>
    /// <returns>Always throws because no factory exists for ExpVector.</returns>
    public virtual AbelianGroupFactory<ExpVector> Factory()
    {
        throw new NotSupportedException(FactoryNotImplementedMessage);
    }

    /// <summary>
    /// Always returns true because each exponent entry comes from a finite integer set.
    /// </summary>
    public virtual bool IsFinite()
    {
        return true;
    }

    /// <summary>
    /// Creates a deep copy of the exponent vector.
    /// </summary>
    public abstract ExpVector Clone();

    /// <summary>
    /// Returns the underlying array of exponent values.
    /// </summary>
    /// <returns>Copy of the stored exponents.</returns>
    public abstract long[] GetVal();

    /// <summary>
    /// Reads the exponent at the specified index.
    /// </summary>
    /// <param name="index">Variable position.</param>
    /// <returns>Exponent value.</returns>
    public abstract long GetVal(int index);

    /// <summary>
    /// Length of this exponent vector (number of variables).
    /// </summary>
    public abstract int Length();

    /// <summary>
    /// Extends this vector by <paramref name="count"/> new variables and sets the exponent at <paramref name="index"/>.
    /// </summary>
    public abstract ExpVector Extend(int count, int index, long exponent);

    /// <summary>
    /// Extends with additional lower variables while setting a specific exponent.
    /// </summary>
    public abstract ExpVector ExtendLower(int count, int index, long exponent);

    /// <summary>
    /// Contracts the vector down to a smaller number of variables starting at <paramref name="index"/>.
    /// </summary>
    /// <param name="index">First position to include in the result.</param>
    /// <param name="length">New length of the contracted vector.</param>
    public abstract ExpVector Contract(int index, int length);

    /// <summary>
    /// Reverses the order of the underlying variables, useful for opposite rings.
    /// </summary>
    public abstract ExpVector Reverse();

    /// <summary>
    /// Reverses the first <paramref name="variables"/> entries, leaving the rest unchanged.
    /// </summary>
    /// <param name="variables">Number of variables to reverse.</param>
    public abstract ExpVector Reverse(int variables);

    /// <summary>
    /// Combines this exponent vector with another by concatenating their entries.
    /// </summary>
    /// <param name="vector">Other vector to merge with.</param>
    public abstract ExpVector Combine(ExpVector vector);

    /// <summary>
    /// Returns the component-wise absolute value of this exponent vector.
    /// </summary>
    public abstract ExpVector Abs();

    /// <summary>
    /// Negates every exponent entry.
    /// </summary>
    public abstract ExpVector Negate();

    /// <summary>
    /// Tests whether all exponents are zero.
    /// </summary>
    public virtual bool IsZero()
    {
        return Signum() == 0;
    }

    /// <summary>
    /// Computes the signum: <c>0</c> for zero, <c>-1</c> if any entry is negative; otherwise <c>1</c>.
    /// </summary>
    public abstract int Signum();

    /// <summary>
    /// Adds component-wise to another exponent vector.
    /// </summary>
    public abstract ExpVector Sum(ExpVector vector);

    /// <summary>
    /// Subtracts the provided exponent vector entry-wise.
    /// </summary>
    public abstract ExpVector Subtract(ExpVector vector);

    /// <summary>
    /// Substitutes a new exponent at the given position while leaving the original vector unchanged.
    /// </summary>
    /// <param name="index">Position to update.</param>
    /// <param name="exponent">New exponent value.</param>
    /// <returns>Cloned vector with the updated entry.</returns>
    public virtual ExpVector Subst(int index, long exponent)
    {
        _ = index;
        _ = exponent;
        return Clone();
    }

    /// <summary>
    /// Returns the total degree (sum of exponents).
    /// </summary>
    public long Degree()
    {
        return TotalDeg();
    }

    /// <summary>
    /// Returns the sum of all exponents.
    /// </summary>
    public abstract long TotalDeg();

    /// <summary>
    /// Returns the maximum exponent entry.
    /// </summary>
    public abstract long MaxDeg();

    /// <summary>
    /// Computes the component-wise maximum (least common multiple) with another vector.
    /// </summary>
    public abstract ExpVector Lcm(ExpVector vector);

    /// <summary>
    /// Computes the component-wise minimum (greatest common divisor) with another vector.
    /// </summary>
    public abstract ExpVector Gcd(ExpVector vector);

    /// <summary>
    /// Identifies the variables that currently have positive exponents.
    /// </summary>
    public abstract int[] DependencyOnVariables();

    /// <summary>
    /// Tests whether this vector is component-wise greater than or equal to another.
    /// </summary>
    public abstract bool MultipleOf(ExpVector vector);

    /// <summary>
    /// Compares this vector to another using inverse lexicographical order.
    /// </summary>
    /// <param name="other">Vector to compare.</param>
    /// <returns>Sign of the comparison.</returns>
    public virtual int CompareTo(ExpVector? other)
    {
        if (other is null)
        {
            return 1;
        }

        return InvLexCompareTo(other);
    }

    /// <summary>
    /// MAS-style shorthand for inverse lexicographical comparison.
    /// </summary>
    public static int EvIlcp(ExpVector left, ExpVector right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        return left.InvLexCompareTo(right);
    }

    /// <summary>
    /// MAS-style inverse lexicographical comparison within a variable range.
    /// </summary>
    public static int EvIlcp(ExpVector left, ExpVector right, int begin, int end)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        return left.InvLexCompareTo(right, begin, end);
    }

    /// <summary>
    /// Inverse graded lexicographical comparison (MAS naming).
    /// </summary>
    public static int EvIglc(ExpVector left, ExpVector right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        return left.InvGradCompareTo(right);
    }

    /// <summary>
    /// Inverse graded lexicographical comparison over a subset of variables.
    /// </summary>
    public static int EvIglc(ExpVector left, ExpVector right, int begin, int end)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        return left.InvGradCompareTo(right, begin, end);
    }

    /// <summary>
    /// Reverse inverse lexicographical comparison (MAS helper).
    /// </summary>
    public static int EvRilcp(ExpVector left, ExpVector right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        return left.RevInvLexCompareTo(right);
    }

    /// <summary>
    /// Reverse inverse lexicographical comparison over a variable slice.
    /// </summary>
    public static int EvRilcp(ExpVector left, ExpVector right, int begin, int end)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        return left.RevInvLexCompareTo(right, begin, end);
    }

    /// <summary>
    /// Reverse inverse graded lexicographical comparison (MAS helper).
    /// </summary>
    public static int EvRiglc(ExpVector left, ExpVector right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        return left.RevInvGradCompareTo(right);
    }

    /// <summary>
    /// Reverse inverse graded lexicographical comparison over a range.
    /// </summary>
    public static int EvRiglc(ExpVector left, ExpVector right, int begin, int end)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        return left.RevInvGradCompareTo(right, begin, end);
    }

    /// <summary>
    /// Weighted inverse comparison following the MAS naming.
    /// </summary>
    public static int EvIwlc(long[][] weights, ExpVector left, ExpVector right)
    {
        ArgumentNullException.ThrowIfNull(weights);
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        return left.InvWeightCompareTo(weights, right);
    }

    /// <summary>
    /// Compares this vector with another using inverse lex order.
    /// </summary>
    public abstract int InvLexCompareTo(ExpVector vector);

    /// <summary>
    /// Inverse lex comparison restricted to a variable range.
    /// </summary>
    public abstract int InvLexCompareTo(ExpVector vector, int begin, int end);

    /// <summary>
    /// Compares this vector with another using inverse graded lex order.
    /// </summary>
    public abstract int InvGradCompareTo(ExpVector vector);

    /// <summary>
    /// Inverse graded lex comparison restricted to a variable range.
    /// </summary>
    public abstract int InvGradCompareTo(ExpVector vector, int begin, int end);

    /// <summary>
    /// Reverse inverse lexicographical comparison.
    /// </summary>
    public abstract int RevInvLexCompareTo(ExpVector vector);

    /// <summary>
    /// Reverse inverse lex comparison restricted to a range.
    /// </summary>
    public abstract int RevInvLexCompareTo(ExpVector vector, int begin, int end);

    /// <summary>
    /// Reverse inverse graded lexicographical comparison.
    /// </summary>
    public abstract int RevInvGradCompareTo(ExpVector vector);

    /// <summary>
    /// Reverse inverse graded lex comparison restricted to a range.
    /// </summary>
    public abstract int RevInvGradCompareTo(ExpVector vector, int begin, int end);

    /// <summary>
    /// Weighted inverse lex comparison using a provided weight matrix.
    /// </summary>
    public abstract int InvWeightCompareTo(long[][] weights, ExpVector vector);

    /// <summary>
    /// Equality via inverse lexicographical comparison.
    /// </summary>
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

    /// <summary>
    /// Lazily cached hash code optimized for small exponent vectors (mirrors Java logic).
    /// </summary>
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

    /// <summary>
    /// Formats the exponent vector as a comma-separated list in parentheses.
    /// </summary>
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

    /// <summary>
    /// Formats the exponent vector using the provided variable names and exponents.
    /// </summary>
    /// <param name="variables">Variable names aligned with exponent positions.</param>
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

    /// <summary>
    /// Joins a variable name array into a comma-delimited string (null returns "null").
    /// </summary>
    /// <param name="variables">Array of variable names.</param>
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

    /// <summary>
    /// Generates a random exponent vector with the requested degree bounds and density.
    /// </summary>
    /// <param name="length">Number of variables.</param>
    /// <param name="maxDegree">Maximum exponent value (exclusive).</param>
    /// <param name="density">Probability that an entry is non-zero.</param>
    public static ExpVector Random(int length, long maxDegree, float density)
    {
        return Random(length, maxDegree, density, s_random);
    }

    /// <summary>
    /// Generates a random exponent vector with the supplied random source for reproducibility.
    /// </summary>
    /// <param name="length">Number of variables.</param>
    /// <param name="maxDegree">Maximum exponent value (exclusive).</param>
    /// <param name="density">Probability that an entry is non-zero.</param>
    /// <param name="random">Random source.</param>
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
                values[i] = random.NextInt64(maxDegree);
            }
            else
            {
                values[i] = 0;
            }
        }

        return Create(values);
    }

    /// <summary>
    /// Explicit interface forwarding to <see cref="Factory"/>.
    /// </summary>
    ElemFactory<ExpVector> Element<ExpVector>.Factory()
    {
        return Factory();
    }
}
