using System.Collections;
using System.Numerics;
using Xunit;

namespace NRedberry.Core.Entities.Tests;

public sealed class RationalTests
{
    [Fact]
    public void ShouldNormalizeBasicArithmeticAndPredicates()
    {
        Rational value = new(1, 2);

        Assert.Equal("5/6", value.Add(new Rational(1, 3)).ToString());
        Assert.Equal("1/6", value.Subtract(new Rational(1, 3)).ToString());
        Assert.Equal("1/6", value.Multiply(new Rational(1, 3)).ToString());
        Assert.Equal("3/2", value.Divide(new Rational(1, 3)).ToString());
        Assert.False(value.IsInteger());
        Assert.False(value.IsNumeric());
        Assert.True(Rational.One.IsOne());
        Assert.True(Rational.MinusOne.IsMinusOne());
    }

    [Fact]
    public void ShouldSupportPowerReciprocalAndConversions()
    {
        Rational value = new(2, 3);

        Assert.Equal("4/9", value.Pow(2).ToString());
        Assert.Equal("3/2", value.Pow(-1).ToString());
        Assert.Equal("3/2", value.Reciprocal().ToString());
        Assert.Equal(new BigInteger(0), value.BigIntValue());
        Assert.Equal(0, value.IntValue());
        Assert.Equal(0L, value.LongValue());
    }
}

public sealed class RationalExtensionsTests
{
    [Fact]
    public void ShouldReturnAbsoluteValueForWrappedRationals()
    {
        Rationals.Rational value = new(-2, 3);
        Rationals.Rational zero = new(0);

        Assert.Equal(new Rationals.Rational(2, 3), value.Abs());
        Assert.Equal(Rationals.Rational.Zero, zero.Abs());
        Assert.True(Rationals.Rational.NaN.Abs().IsNaN);
    }

    [Fact]
    public void ShouldReturnAbsoluteValueForEntityRationals()
    {
        Rational value = new(-5, 7);

        Assert.Equal(new Rational(5, 7), RationalExtensions.Abs(value));
        Assert.Equal(Rational.Zero, RationalExtensions.Abs(Rational.Zero));
        Assert.True(RationalExtensions.Abs(Rational.NaN).IsNaN());
    }
}

public sealed class RealTests
{
    [Fact]
    public void ShouldExposeConcreteMembersThroughDerivedImplementations()
    {
        Real numeric = new Numeric(3.5);
        Real rational = new Rational(7, 2);

        Assert.Equal(3.5, numeric.ToDouble());
        Assert.Equal(3.5, rational.ToDouble());
        Assert.Same(RealField.Instance, numeric.Field);
        Assert.Same(RealField.Instance, rational.Field);
    }
}

public sealed class RealFieldTests
{
    [Fact]
    public void ShouldExposeSingletonIdentityAndFieldConstants()
    {
        RealField field = RealField.Instance;

        Assert.Same(field, RealField.Instance);
        Assert.Equal(typeof(Real), field.GetRuntimeClass());
        Assert.Same(Rational.Zero, field.Zero);
        Assert.Same(Rational.One, field.One);
    }
}

public sealed class TypeDataTests
{
    [Fact]
    public void ShouldCloneIncomingStates()
    {
        BitArray states = new([true, false, true]);
        TypeData data = new(2, 3, states);

        states[0] = false;

        Assert.Equal(2, data.From);
        Assert.Equal(3, data.Length);
        Assert.NotNull(data.States);
        Assert.True(data.States![0]);
    }

    [Fact]
    public void ShouldAllowNullStates()
    {
        TypeData data = new(4, 0, null);

        Assert.Equal(4, data.From);
        Assert.Equal(0, data.Length);
        Assert.Null(data.States);
    }
}

public sealed class UpperLowerIndicesTests
{
    [Fact]
    public void ShouldExposeProvidedUpperAndLowerArrays()
    {
        int[] upper = [1, 2];
        int[] lower = [3, 4];
        UpperLowerIndices indices = new(upper, lower);

        Assert.Same(upper, indices.Upper);
        Assert.Same(lower, indices.Lower);
        Assert.Equal([1, 2], indices.Upper);
        Assert.Equal([3, 4], indices.Lower);
    }
}

public sealed class BitArrayEqualityComparerTests
{
    [Fact]
    public void ShouldCompareBitArraysByContents()
    {
        BitArrayEqualityComparer comparer = new();
        BitArray left = new([true, false, true]);
        BitArray equal = new([true, false, true]);
        BitArray different = new([true, true, false]);

        Assert.True(comparer.Equals(left, equal));
        Assert.False(comparer.Equals(left, different));
        Assert.False(comparer.Equals(left, null));
        Assert.Equal(comparer.GetHashCode(left), comparer.GetHashCode(equal));
    }
}

public sealed class BitArrayExtensionsTests
{
    [Fact]
    public void ShouldReturnNextSetBitStartingFromProvidedIndex()
    {
        BitArray bitArray = new([false, true, false, true]);

        Assert.Equal(1, bitArray.NextTrailingBit(0));
        Assert.Equal(3, bitArray.NextTrailingBit(2));
        Assert.Equal(-1, bitArray.NextTrailingBit(4));
        Assert.Empty(BitArrayExtensions.Empty);
    }
}

public sealed class IIndexSymbolConverterTests
{
    [Fact]
    public void ShouldExposeRequiredMembersViaConcreteConverter()
    {
        IIndexSymbolConverter converter = new StubIndexSymbolConverter();

        Assert.True(converter.ApplicableToSymbol("a"));
        Assert.Equal("s5:Redberry", converter.GetSymbol(5, OutputFormat.Redberry));
        Assert.Equal(3, converter.GetCode("abc"));
        Assert.Equal(32, converter.MaxNumberOfSymbols);
        Assert.Equal((byte)7, converter.Type);
    }

    private sealed class StubIndexSymbolConverter : IIndexSymbolConverter
    {
        public bool ApplicableToSymbol(string symbol) => symbol.Length > 0;

        public string GetSymbol(int code, OutputFormat outputFormat) => $"s{code}:{outputFormat}";

        public int GetCode(string symbol) => symbol.Length;

        public int MaxNumberOfSymbols => 32;

        public byte Type => 7;
    }
}

public sealed class IndexTypeTests
{
    [Fact]
    public void ShouldExposeExpectedEnumMembersInDeclarationOrder()
    {
        Assert.Equal(
            [
                IndexType.LatinLower,
                IndexType.LatinUpper,
                IndexType.GreekLower,
                IndexType.GreekUpper,
                IndexType.Matrix1,
                IndexType.Matrix2,
                IndexType.Matrix3,
                IndexType.Matrix4,
            ],
            Enum.GetValues<IndexType>());
    }
}

public sealed class INumberTests
{
    [Fact]
    public void ShouldExposeNonGenericNumberContractThroughReal()
    {
        INumber number = new Numeric(3.5);

        Assert.Equal(3, number.IntValue());
        Assert.Equal(3L, number.LongValue());
        Assert.Equal(3.5, number.DoubleValue());
        Assert.Equal(3.5f, number.FloatValue());
        Assert.False(number.IsInteger());
        Assert.True(number.IsNumeric());
        Assert.Equal(new BigInteger(3), number.BigIntValue());
    }

    [Fact]
    public void ShouldExposeGenericNumberOperationsThroughReal()
    {
        INumber<Real> number = new Numeric(2.0);

        Assert.Equal("3", number.Add(1).ToString());
        Assert.Equal("4", number.Multiply(2).ToString());
        Assert.Equal("8", number.Pow(3).ToString());
        Assert.Equal("2", number.GetNumericValue().ToString());
        Assert.Same(RealField.Instance, number.Field);
    }
}

public sealed class NumericTests
{
    [Fact]
    public void ShouldReuseCanonicalNumericInstances()
    {
        Numeric zero = (Numeric)Numeric.One.Subtract(1);
        Numeric one = Numeric.FromNumber(new Rational(1));
        Numeric nan = (Numeric)Numeric.Zero.Divide(0.0);

        Assert.Same(Numeric.Zero, zero);
        Assert.Same(Numeric.One, one);
        Assert.Same(Numeric.NaN, nan);
    }

    [Fact]
    public void ShouldSupportArithmeticEqualityAndPredicates()
    {
        Numeric value = new(2.5);

        Assert.Equal("3.5", value.Add(1).ToString());
        Assert.Equal("5", value.Multiply(2).ToString());
        Assert.Equal("1.25", value.Divide(2).ToString());
        Assert.Equal("6.25", value.Pow(2).ToString());
        Assert.True(new Numeric(2.5).Equals(value));
        Assert.True(Numeric.One.IsOne());
        Assert.True(Numeric.MinusOne.IsMinusOne());
        Assert.True(Numeric.PositiveInfinity.IsInfinite());
    }
}
