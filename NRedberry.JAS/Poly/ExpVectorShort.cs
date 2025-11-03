namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// ExpVectorShort implements exponent vectors for polynomials using arrays of short as storage unit.
/// This class is used by ExpVector internally, there is no need to use this class directly.
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.ExpVectorShort
/// </remarks>
public sealed class ExpVectorShort : ExpVector
{
    public static readonly long MaxShort = short.MaxValue / 2L;
    public static readonly long MinShort = short.MinValue / 2L;

    internal readonly short[] _values;

    public ExpVectorShort(int length)
        : this(new short[length])
    {
    }

    public ExpVectorShort(int length, int index, long exponent)
        : this(new short[length])
    {
        if (index < 0 || index >= length)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Index must be within the exponent vector range.");
        }

        _values[index] = ConvertToShort(exponent);
    }

    public ExpVectorShort(long[] values)
    {
        ArgumentNullException.ThrowIfNull(values);
        _values = new short[values.Length];
        for (int i = 0; i < values.Length; i++)
        {
            _values[i] = ConvertToShort(values[i]);
        }
    }

    public ExpVectorShort(short[] values)
    {
        ArgumentNullException.ThrowIfNull(values);
        _values = values;
    }

    public override ExpVector Clone()
    {
        short[] result = new short[_values.Length];
        Array.Copy(_values, result, _values.Length);
        return new ExpVectorShort(result);
    }

    public override long[] GetVal()
    {
        long[] result = new long[_values.Length];
        for (int i = 0; i < _values.Length; i++)
        {
            result[i] = _values[i];
        }

        return result;
    }

    public override long GetVal(int index)
    {
        return _values[index];
    }

    public override int Length()
    {
        return _values.Length;
    }

    public override ExpVector Extend(int count, int index, long exponent)
    {
        if (count < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count), "Extension count must be non-negative.");
        }

        if (index < 0 || index >= count)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Index must be within the extension range.");
        }

        short[] result = new short[_values.Length + count];
        Array.Copy(_values, 0, result, count, _values.Length);
        result[index] = ConvertToShort(exponent);
        return new ExpVectorShort(result);
    }

    public override ExpVector ExtendLower(int count, int index, long exponent)
    {
        if (count < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count), "Extension count must be non-negative.");
        }

        if (index < 0 || index >= count)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Index must be within the extension range.");
        }

        short[] result = new short[_values.Length + count];
        Array.Copy(_values, 0, result, 0, _values.Length);
        result[_values.Length + index] = ConvertToShort(exponent);
        return new ExpVectorShort(result);
    }

    public override ExpVector Contract(int index, int length)
    {
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Length must be non-negative.");
        }

        if (index < 0 || index + length > _values.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Index and length must define a valid range.");
        }

        short[] result = new short[length];
        Array.Copy(_values, index, result, 0, length);
        return new ExpVectorShort(result);
    }

    public override ExpVector Reverse()
    {
        short[] result = new short[_values.Length];
        for (int i = 0; i < _values.Length; i++)
        {
            result[i] = _values[_values.Length - 1 - i];
        }

        return new ExpVectorShort(result);
    }

    public override ExpVector Reverse(int variables)
    {
        if (variables <= 0 || variables > _values.Length)
        {
            return this;
        }

        short[] result = new short[_values.Length];
        for (int i = 0; i < variables; i++)
        {
            result[i] = _values[variables - 1 - i];
        }

        Array.Copy(_values, variables, result, variables, _values.Length - variables);
        return new ExpVectorShort(result);
    }

    public override ExpVector Combine(ExpVector vector)
    {
        ArgumentNullException.ThrowIfNull(vector);
        short[] other = ExtractValues(vector);
        if (_values.Length == 0)
        {
            return vector.Clone();
        }

        if (other.Length == 0)
        {
            return Clone();
        }

        short[] result = new short[_values.Length + other.Length];
        Array.Copy(_values, 0, result, 0, _values.Length);
        Array.Copy(other, 0, result, _values.Length, other.Length);
        return new ExpVectorShort(result);
    }

    public override ExpVector Abs()
    {
        short[] result = new short[_values.Length];
        for (int i = 0; i < _values.Length; i++)
        {
            short value = _values[i];
            result[i] = value >= 0 ? value : (short)(-value);
        }

        return new ExpVectorShort(result);
    }

    public override ExpVector Negate()
    {
        short[] result = new short[_values.Length];
        for (int i = 0; i < _values.Length; i++)
        {
            result[i] = (short)(-_values[i]);
        }

        return new ExpVectorShort(result);
    }

    public override int Signum()
    {
        int sign = 0;
        for (int i = 0; i < _values.Length; i++)
        {
            short value = _values[i];
            if (value < 0)
            {
                return -1;
            }

            if (value > 0)
            {
                sign = 1;
            }
        }

        return sign;
    }

    public override ExpVector Sum(ExpVector vector)
    {
        ArgumentNullException.ThrowIfNull(vector);
        short[] other = ExtractValues(vector);
        EnsureSameLength(other, nameof(vector));

        short[] result = new short[_values.Length];
        for (int i = 0; i < _values.Length; i++)
        {
            result[i] = (short)(_values[i] + other[i]);
        }

        return new ExpVectorShort(result);
    }

    public override ExpVector Subtract(ExpVector vector)
    {
        ArgumentNullException.ThrowIfNull(vector);
        short[] other = ExtractValues(vector);
        EnsureSameLength(other, nameof(vector));

        short[] result = new short[_values.Length];
        for (int i = 0; i < _values.Length; i++)
        {
            result[i] = (short)(_values[i] - other[i]);
        }

        return new ExpVectorShort(result);
    }

    public override long TotalDeg()
    {
        long total = 0;
        for (int i = 0; i < _values.Length; i++)
        {
            total += _values[i];
        }

        return total;
    }

    public override long MaxDeg()
    {
        long max = 0;
        for (int i = 0; i < _values.Length; i++)
        {
            if (_values[i] > max)
            {
                max = _values[i];
            }
        }

        return max;
    }

    public override ExpVector Lcm(ExpVector vector)
    {
        ArgumentNullException.ThrowIfNull(vector);
        short[] other = ExtractValues(vector);
        EnsureSameLength(other, nameof(vector));

        short[] result = new short[_values.Length];
        for (int i = 0; i < _values.Length; i++)
        {
            result[i] = _values[i] >= other[i] ? _values[i] : other[i];
        }

        return new ExpVectorShort(result);
    }

    public override ExpVector Gcd(ExpVector vector)
    {
        ArgumentNullException.ThrowIfNull(vector);
        short[] other = ExtractValues(vector);
        EnsureSameLength(other, nameof(vector));

        short[] result = new short[_values.Length];
        for (int i = 0; i < _values.Length; i++)
        {
            result[i] = _values[i] <= other[i] ? _values[i] : other[i];
        }

        return new ExpVectorShort(result);
    }

    public override int[] DependencyOnVariables()
    {
        List<int> indices = new ();
        for (int i = 0; i < _values.Length; i++)
        {
            if (_values[i] > 0)
            {
                indices.Add(i);
            }
        }

        return indices.ToArray();
    }

    public override bool MultipleOf(ExpVector vector)
    {
        ArgumentNullException.ThrowIfNull(vector);
        short[] other = ExtractValues(vector);
        EnsureSameLength(other, nameof(vector));

        for (int i = 0; i < _values.Length; i++)
        {
            if (_values[i] < other[i])
            {
                return false;
            }
        }

        return true;
    }

    public override int InvLexCompareTo(ExpVector vector)
    {
        ArgumentNullException.ThrowIfNull(vector);
        short[] other = ExtractValues(vector);
        EnsureSameLength(other, nameof(vector));

        for (int i = 0; i < _values.Length; i++)
        {
            if (_values[i] > other[i])
            {
                return 1;
            }

            if (_values[i] < other[i])
            {
                return -1;
            }
        }

        return 0;
    }

    public override int InvLexCompareTo(ExpVector vector, int begin, int end)
    {
        ArgumentNullException.ThrowIfNull(vector);
        ValidateRange(begin, end);

        short[] other = ExtractValues(vector);
        EnsureSameLength(other, nameof(vector));

        for (int i = begin; i < end; i++)
        {
            if (_values[i] > other[i])
            {
                return 1;
            }

            if (_values[i] < other[i])
            {
                return -1;
            }
        }

        return 0;
    }

    public override int InvGradCompareTo(ExpVector vector)
    {
        ArgumentNullException.ThrowIfNull(vector);
        short[] other = ExtractValues(vector);
        EnsureSameLength(other, nameof(vector));

        int comparison = 0;
        int index = 0;
        for (index = 0; index < _values.Length; index++)
        {
            if (_values[index] > other[index])
            {
                comparison = 1;
                break;
            }

            if (_values[index] < other[index])
            {
                comparison = -1;
                break;
            }
        }

        if (comparison == 0)
        {
            return 0;
        }

        long selfSum = 0;
        long otherSum = 0;
        for (int j = index; j < _values.Length; j++)
        {
            selfSum += _values[j];
            otherSum += other[j];
        }

        if (selfSum > otherSum)
        {
            return 1;
        }

        if (selfSum < otherSum)
        {
            return -1;
        }

        return comparison;
    }

    public override int InvGradCompareTo(ExpVector vector, int begin, int end)
    {
        ArgumentNullException.ThrowIfNull(vector);
        ValidateRange(begin, end);

        short[] other = ExtractValues(vector);
        EnsureSameLength(other, nameof(vector));

        int comparison = 0;
        int index = begin;
        for (index = begin; index < end; index++)
        {
            if (_values[index] > other[index])
            {
                comparison = 1;
                break;
            }

            if (_values[index] < other[index])
            {
                comparison = -1;
                break;
            }
        }

        if (comparison == 0)
        {
            return 0;
        }

        long selfSum = 0;
        long otherSum = 0;
        for (int j = index; j < end; j++)
        {
            selfSum += _values[j];
            otherSum += other[j];
        }

        if (selfSum > otherSum)
        {
            return 1;
        }

        if (selfSum < otherSum)
        {
            return -1;
        }

        return comparison;
    }

    public override int RevInvLexCompareTo(ExpVector vector)
    {
        ArgumentNullException.ThrowIfNull(vector);
        short[] other = ExtractValues(vector);
        EnsureSameLength(other, nameof(vector));

        for (int i = _values.Length - 1; i >= 0; i--)
        {
            if (_values[i] > other[i])
            {
                return 1;
            }

            if (_values[i] < other[i])
            {
                return -1;
            }
        }

        return 0;
    }

    public override int RevInvLexCompareTo(ExpVector vector, int begin, int end)
    {
        ArgumentNullException.ThrowIfNull(vector);
        ValidateRange(begin, end);

        short[] other = ExtractValues(vector);
        EnsureSameLength(other, nameof(vector));

        for (int i = end - 1; i >= begin; i--)
        {
            if (_values[i] > other[i])
            {
                return 1;
            }

            if (_values[i] < other[i])
            {
                return -1;
            }
        }

        return 0;
    }

    public override int RevInvGradCompareTo(ExpVector vector)
    {
        ArgumentNullException.ThrowIfNull(vector);
        short[] other = ExtractValues(vector);
        EnsureSameLength(other, nameof(vector));

        int comparison = 0;
        int index = _values.Length - 1;
        for (index = _values.Length - 1; index >= 0; index--)
        {
            if (_values[index] > other[index])
            {
                comparison = 1;
                break;
            }

            if (_values[index] < other[index])
            {
                comparison = -1;
                break;
            }
        }

        if (comparison == 0)
        {
            return 0;
        }

        long selfSum = 0;
        long otherSum = 0;
        for (int j = index; j >= 0; j--)
        {
            selfSum += _values[j];
            otherSum += other[j];
        }

        if (selfSum > otherSum)
        {
            return 1;
        }

        if (selfSum < otherSum)
        {
            return -1;
        }

        return comparison;
    }

    public override int RevInvGradCompareTo(ExpVector vector, int begin, int end)
    {
        ArgumentNullException.ThrowIfNull(vector);
        ValidateRange(begin, end);

        short[] other = ExtractValues(vector);
        EnsureSameLength(other, nameof(vector));

        int comparison = 0;
        int index = end - 1;
        for (index = end - 1; index >= begin; index--)
        {
            if (_values[index] > other[index])
            {
                comparison = 1;
                break;
            }

            if (_values[index] < other[index])
            {
                comparison = -1;
                break;
            }
        }

        if (comparison == 0)
        {
            return 0;
        }

        long selfSum = 0;
        long otherSum = 0;
        for (int j = index; j >= begin; j--)
        {
            selfSum += _values[j];
            otherSum += other[j];
        }

        if (selfSum > otherSum)
        {
            return 1;
        }

        if (selfSum < otherSum)
        {
            return -1;
        }

        return comparison;
    }

    public override int InvWeightCompareTo(long[][] weights, ExpVector vector)
    {
        ArgumentNullException.ThrowIfNull(weights);
        ArgumentNullException.ThrowIfNull(vector);

        short[] other = ExtractValues(vector);
        EnsureSameLength(other, nameof(vector));
        ValidateWeights(weights);

        int comparison = 0;
        int index = 0;
        for (index = 0; index < _values.Length; index++)
        {
            if (_values[index] > other[index])
            {
                comparison = 1;
                break;
            }

            if (_values[index] < other[index])
            {
                comparison = -1;
                break;
            }
        }

        if (comparison == 0)
        {
            return 0;
        }

        foreach (long[] weight in weights)
        {
            long selfSum = 0;
            long otherSum = 0;
            for (int j = index; j < _values.Length; j++)
            {
                selfSum += weight[j] * _values[j];
                otherSum += weight[j] * other[j];
            }

            if (selfSum > otherSum)
            {
                return 1;
            }

            if (selfSum < otherSum)
            {
                return -1;
            }
        }

        return comparison;
    }

    public override string ToString()
    {
        return base.ToString() + ":short";
    }

    private static short[] ExtractValues(ExpVector vector)
    {
        if (vector is ExpVectorShort shortVector)
        {
            return shortVector._values;
        }

        long[] values = vector.GetVal();
        short[] result = new short[values.Length];
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = ConvertToShort(values[i]);
        }

        return result;
    }

    private static short ConvertToShort(long value)
    {
        if (value >= MaxShort || value <= MinShort)
        {
            throw new ArgumentOutOfRangeException(nameof(value), $"Exponent {value} exceeds supported short range.");
        }

        return (short)value;
    }

    private void EnsureSameLength(short[] other, string parameterName)
    {
        if (other.Length != _values.Length)
        {
            throw new ArgumentException("Exponent vectors must have the same length.", parameterName);
        }
    }

    private void ValidateRange(int begin, int end)
    {
        if (begin < 0 || end < begin || end > _values.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(begin), "Invalid begin/end range specified.");
        }
    }

    private void ValidateWeights(long[][] weights)
    {
        for (int i = 0; i < weights.Length; i++)
        {
            long[] row = weights[i];
            if (row == null || row.Length != _values.Length)
            {
                throw new ArgumentException("Weight matrix must match the exponent vector length.", nameof(weights));
            }
        }
    }
}
