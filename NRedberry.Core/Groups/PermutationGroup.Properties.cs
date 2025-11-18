using System.Numerics;
using NRedberry.Core.Combinatorics;

namespace NRedberry.Core.Groups;

public sealed partial class PermutationGroup
{
    public bool IsTransitive()
    {
        if (IsTrivial())
        {
            return false;
        }

        return _orbits.Length == 1;
    }

    public bool IsTransitive(int from, int to)
    {
        if (from > to)
        {
            throw new ArgumentException("Specified from less then specified to.");
        }

        if (to >= _internalDegree)
        {
            return false;
        }

        for (int i = from + 1; i < to; ++i)
        {
            if (_positionsInOrbits[i] != _positionsInOrbits[i - 1])
            {
                return false;
            }
        }

        return true;
    }

    public bool IsTrivial()
    {
        if (_isTrivial is not null)
        {
            return _isTrivial.Value;
        }

        _isTrivial = true;
        foreach (Permutation permutation in _generators)
        {
            if (!permutation.IsIdentity)
            {
                _isTrivial = false;
                break;
            }
        }

        return _isTrivial.Value;
    }

    public bool IsAbelian()
    {
        if (IsTrivial())
        {
            return true;
        }

        if (_isAbelian is not null)
        {
            return _isAbelian.Value;
        }

        _isAbelian = true;
        List<Permutation> generators = _generators.ToList();
        int size = generators.Count;
        for (int i = 0; i < size; ++i)
        {
            for (int j = i + 1; j < size; ++j)
            {
                if (!generators[i].Commutator(generators[j]).IsIdentity)
                {
                    _isAbelian = false;
                    return _isAbelian.Value;
                }
            }
        }

        return _isAbelian.Value;
    }

    public bool IsSymmetric()
    {
        if (_isSymmetric is not null)
        {
            return _isSymmetric.Value;
        }

        _isSymmetric = IsSym0();
        if (!_isSymmetric.Value)
        {
            _isSymmetric = Order.Equals(FactorialInternal(_internalDegree));
        }

        return _isSymmetric.Value;
    }

    public bool IsAlternating()
    {
        if (_isAlternating is not null)
        {
            return _isAlternating.Value;
        }

        _isAlternating = IsAlt0();
        if (_isAlternating.Value)
        {
            return _isAlternating.Value;
        }

        _isAlternating = Order.Equals(FactorialInternal(_internalDegree) / new BigInteger(2));
        if (_isAlternating.Value)
        {
            foreach (Permutation permutation in _generators)
            {
                if (permutation.Parity == 1)
                {
                    return _isAlternating == false;
                }
            }
        }

        return _isAlternating.Value;
    }

    public bool IsRegular()
    {
        return IsTransitive() && Order.Equals(new BigInteger(_internalDegree));
    }

    private bool IsSym0()
    {
        if (IsTrivial() || !IsTransitive())
        {
            return _isSymmetric == false;
        }

        if (_internalDegree > 2 && _generators.Count == 1)
        {
            return _isSymmetric == false;
        }

        bool isSym = IsSymOrAlt(DefaultConfidenceLevel);
        if (!isSym)
        {
            return false;
        }

        bool containsOdd = false;
        foreach (Permutation permutation in _generators)
        {
            if (permutation.Parity == 1)
            {
                containsOdd = true;
                break;
            }
        }

        return _isSymmetric == containsOdd;
    }

    private bool IsAlt0()
    {
        if (IsTrivial() || !IsTransitive())
        {
            return _isAlternating == false;
        }

        bool isAlt = IsSymOrAlt(DefaultConfidenceLevel);
        if (!isAlt)
        {
            return false;
        }

        foreach (Permutation permutation in _generators)
        {
            if (permutation.Parity == 1)
            {
                return _isAlternating == false;
            }
        }

        return _isAlternating == true;
    }

    private const double DefaultConfidenceLevel = 1 - 1E-6;

    private bool IsSymOrAlt(double confidenceLevel)
    {
        if (_internalDegree < 8)
        {
            return false;
        }

        double c = _internalDegree <= 16 ? 0.34 : 0.57;
        int num = (int)(-Math.Log(1 - confidenceLevel) * Math.Log(_internalDegree, 2) / c);
        List<Permutation> randomSource = RandomSource();
        for (int i = 0; i < num; ++i)
        {
            int[] lengths = RandomPermutation.Random(randomSource).LengthsOfCycles;
            foreach (int length in lengths)
            {
                if (length > _internalDegree / 2 && length < _internalDegree - 2 && IsPrime(length))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private static BigInteger FactorialInternal(int n)
    {
        BigInteger value = BigInteger.One;
        for (int i = 2; i <= n; ++i)
        {
            value *= i;
        }

        return value;
    }

    private static bool IsPrime(int value)
    {
        if (value < 2)
        {
            return false;
        }

        if (value % 2 == 0)
        {
            return value == 2;
        }

        int limit = (int)Math.Sqrt(value);
        for (int i = 3; i <= limit; i += 2)
        {
            if (value % i == 0)
            {
                return false;
            }
        }

        return true;
    }
}
