namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// ExpVectorByte implements exponent vectors for polynomials using arrays of byte as storage unit.
/// This class is used by ExpVector internally, there is no need to use this class directly.
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.ExpVectorByte
/// </remarks>
public sealed class ExpVectorByte : ExpVector
{
    public static readonly long MaxByte = sbyte.MaxValue / 2L;
    public static readonly long MinByte = sbyte.MinValue / 2L;

    internal readonly sbyte[] _values;

    public ExpVectorByte(int length)
        : this(new sbyte[length])
    {
    }

    public ExpVectorByte(int length, int index, long exponent)
        : this(new sbyte[length])
    {
        if (index < 0 || index >= length)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Index must be within the exponent vector range.");
        }

        _values[index] = ConvertToSByte(exponent);
    }

    public ExpVectorByte(long[] values)
    {
        ArgumentNullException.ThrowIfNull(values);
        _values = new sbyte[values.Length];
        for (int i = 0; i < values.Length; i++)
        {
            _values[i] = ConvertToSByte(values[i]);
        }
    }

    public ExpVectorByte(sbyte[] values)
    {
        ArgumentNullException.ThrowIfNull(values);
        _values = values;
    }

    public override ExpVector Clone()
    {
        sbyte[] result = new sbyte[_values.Length];
        Array.Copy(_values, result, _values.Length);
        return new ExpVectorByte(result);
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

        sbyte[] result = new sbyte[_values.Length + count];
        Array.Copy(_values, 0, result, count, _values.Length);
        result[index] = ConvertToSByte(exponent);
        return new ExpVectorByte(result);
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

        sbyte[] result = new sbyte[_values.Length + count];
        Array.Copy(_values, 0, result, 0, _values.Length);
        result[_values.Length + index] = ConvertToSByte(exponent);
        return new ExpVectorByte(result);
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

        sbyte[] result = new sbyte[length];
        Array.Copy(_values, index, result, 0, length);
        return new ExpVectorByte(result);
    }

    public override ExpVector Reverse()
    {
        sbyte[] result = new sbyte[_values.Length];
        for (int i = 0; i < _values.Length; i++)
        {
            result[i] = _values[_values.Length - 1 - i];
        }

        return new ExpVectorByte(result);
    }

    public override ExpVector Reverse(int variables)
    {
        if (variables <= 0 || variables > _values.Length)
        {
            return this;
        }

        sbyte[] result = new sbyte[_values.Length];
        for (int i = 0; i < variables; i++)
        {
            result[i] = _values[variables - 1 - i];
        }

        Array.Copy(_values, variables, result, variables, _values.Length - variables);
        return new ExpVectorByte(result);
    }

    public override ExpVector Combine(ExpVector vector)
    {
        ArgumentNullException.ThrowIfNull(vector);
        sbyte[] other = ExtractValues(vector);
        if (_values.Length == 0)
        {
            return vector.Clone();
        }

        if (other.Length == 0)
        {
            return Clone();
        }

        sbyte[] result = new sbyte[_values.Length + other.Length];
        Array.Copy(_values, 0, result, 0, _values.Length);
        Array.Copy(other, 0, result, _values.Length, other.Length);
        return new ExpVectorByte(result);
    }

    public override ExpVector Abs()
    {
        sbyte[] result = new sbyte[_values.Length];
        for (int i = 0; i < _values.Length; i++)
        {
            sbyte value = _values[i];
            result[i] = value >= 0 ? value : (sbyte)(-value);
        }

        return new ExpVectorByte(result);
    }

    public override ExpVector Negate()
    {
        sbyte[] result = new sbyte[_values.Length];
        for (int i = 0; i < _values.Length; i++)
        {
            result[i] = (sbyte)(-_values[i]);
        }

        return new ExpVectorByte(result);
    }

    public override int Signum()
    {
        int sign = 0;
        for (int i = 0; i < _values.Length; i++)
        {
            sbyte value = _values[i];
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
        sbyte[] other = ExtractValues(vector);
        EnsureSameLength(other, nameof(vector));

        sbyte[] result = new sbyte[_values.Length];
        for (int i = 0; i < _values.Length; i++)
        {
            result[i] = (sbyte)(_values[i] + other[i]);
        }

        return new ExpVectorByte(result);
    }

    public override ExpVector Subtract(ExpVector vector)
    {
        ArgumentNullException.ThrowIfNull(vector);
        sbyte[] other = ExtractValues(vector);
        EnsureSameLength(other, nameof(vector));

        sbyte[] result = new sbyte[_values.Length];
        for (int i = 0; i < _values.Length; i++)
        {
            result[i] = (sbyte)(_values[i] - other[i]);
        }

        return new ExpVectorByte(result);
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
        sbyte[] other = ExtractValues(vector);
        EnsureSameLength(other, nameof(vector));

        sbyte[] result = new sbyte[_values.Length];
        for (int i = 0; i < _values.Length; i++)
        {
            result[i] = _values[i] >= other[i] ? _values[i] : other[i];
        }

        return new ExpVectorByte(result);
    }

    public override ExpVector Gcd(ExpVector vector)
    {
        ArgumentNullException.ThrowIfNull(vector);
        sbyte[] other = ExtractValues(vector);
        EnsureSameLength(other, nameof(vector));

        sbyte[] result = new sbyte[_values.Length];
        for (int i = 0; i < _values.Length; i++)
        {
            result[i] = _values[i] <= other[i] ? _values[i] : other[i];
        }

        return new ExpVectorByte(result);
    }

    public override int[] DependencyOnVariables()
    {
        List<int> indices = [];
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
        sbyte[] other = ExtractValues(vector);
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
        sbyte[] other = ExtractValues(vector);
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

        sbyte[] other = ExtractValues(vector);
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
        sbyte[] other = ExtractValues(vector);
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

        sbyte[] other = ExtractValues(vector);
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
        sbyte[] other = ExtractValues(vector);
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

        sbyte[] other = ExtractValues(vector);
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
        sbyte[] other = ExtractValues(vector);
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

        sbyte[] other = ExtractValues(vector);
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

        sbyte[] other = ExtractValues(vector);
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
        return base.ToString() + ":byte";
    }

    private static sbyte[] ExtractValues(ExpVector vector)
    {
        if (vector is ExpVectorByte byteVector)
        {
            return byteVector._values;
        }

        long[] values = vector.GetVal();
        sbyte[] result = new sbyte[values.Length];
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = ConvertToSByte(values[i]);
        }

        return result;
    }

    private static sbyte ConvertToSByte(long value)
    {
        if (value >= MaxByte || value <= MinByte)
        {
            throw new ArgumentOutOfRangeException(nameof(value), $"Exponent {value} exceeds supported byte range.");
        }

        return (sbyte)value;
    }

    private void EnsureSameLength(sbyte[] other, string parameterName)
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
