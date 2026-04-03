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

        value.Add(new Rational(1, 3)).ToString().ShouldBe("5/6");
        value.Subtract(new Rational(1, 3)).ToString().ShouldBe("1/6");
        value.Multiply(new Rational(1, 3)).ToString().ShouldBe("1/6");
        value.Divide(new Rational(1, 3)).ToString().ShouldBe("3/2");
        value.IsInteger().ShouldBeFalse();
        value.IsNumeric().ShouldBeFalse();
        Rational.One.IsOne().ShouldBeTrue();
        Rational.MinusOne.IsMinusOne().ShouldBeTrue();
    }

    [Fact]
    public void ShouldSupportPowerReciprocalAndConversions()
    {
        Rational value = new(2, 3);

        value.Pow(2).ToString().ShouldBe("4/9");
        value.Pow(-1).ToString().ShouldBe("3/2");
        value.Reciprocal().ToString().ShouldBe("3/2");
        value.BigIntValue().ShouldBe(new BigInteger(0));
        value.IntValue().ShouldBe(0);
        value.LongValue().ShouldBe(0L);
    }
}

public sealed class RationalExtensionsTests
{
    [Fact]
    public void ShouldReturnAbsoluteValueForWrappedRationals()
    {
        Rationals.Rational value = new(-2, 3);
        Rationals.Rational zero = new(0);

        value.Abs().ShouldBe(new Rationals.Rational(2, 3));
        zero.Abs().ShouldBe(Rationals.Rational.Zero);
        Rationals.Rational.NaN.Abs().IsNaN.ShouldBeTrue();
    }

    [Fact]
    public void ShouldReturnAbsoluteValueForEntityRationals()
    {
        Rational value = new(-5, 7);

        RationalExtensions.Abs(value).ShouldBe(new Rational(5, 7));
        RationalExtensions.Abs(Rational.Zero).ShouldBe(Rational.Zero);
        RationalExtensions.Abs(Rational.NaN).IsNaN().ShouldBeTrue();
    }
}

public sealed class RealTests
{
    [Fact]
    public void ShouldExposeConcreteMembersThroughDerivedImplementations()
    {
        Real numeric = new Numeric(3.5);
        Real rational = new Rational(7, 2);

        numeric.ToDouble().ShouldBe(3.5);
        rational.ToDouble().ShouldBe(3.5);
        numeric.Field.ShouldBeSameAs(RealField.Instance);
        rational.Field.ShouldBeSameAs(RealField.Instance);
    }
}

public sealed class RealFieldTests
{
    [Fact]
    public void ShouldExposeSingletonIdentityAndFieldConstants()
    {
        RealField field = RealField.Instance;

        RealField.Instance.ShouldBeSameAs(field);
        field.GetRuntimeClass().ShouldBe(typeof(Real));
        field.Zero.ShouldBeSameAs(Rational.Zero);
        field.One.ShouldBeSameAs(Rational.One);
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

        data.From.ShouldBe(2);
        data.Length.ShouldBe(3);
        data.States.ShouldNotBeNull();
        data.States![0].ShouldBeTrue();
    }

    [Fact]
    public void ShouldAllowNullStates()
    {
        TypeData data = new(4, 0, null);

        data.From.ShouldBe(4);
        data.Length.ShouldBe(0);
        data.States.ShouldBeNull();
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

        indices.Upper.ShouldBeSameAs(upper);
        indices.Lower.ShouldBeSameAs(lower);
        indices.Upper.ShouldBe([1, 2]);
        indices.Lower.ShouldBe([3, 4]);
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

        comparer.Equals(left, equal).ShouldBeTrue();
        comparer.Equals(left, different).ShouldBeFalse();
        comparer.Equals(left, null).ShouldBeFalse();
        comparer.GetHashCode(left).ShouldBe(comparer.GetHashCode(equal));
    }
}

public sealed class BitArrayExtensionsTests
{
    [Fact]
    public void ShouldReturnNextSetBitStartingFromProvidedIndex()
    {
        BitArray bitArray = new([false, true, false, true]);

        bitArray.NextTrailingBit(0).ShouldBe(1);
        bitArray.NextTrailingBit(2).ShouldBe(3);
        bitArray.NextTrailingBit(4).ShouldBe(-1);
        BitArrayExtensions.Empty.Count.ShouldBe(0);
    }
}

public sealed class IIndexSymbolConverterTests
{
    [Fact]
    public void ShouldExposeRequiredMembersViaConcreteConverter()
    {
        IIndexSymbolConverter converter = new StubIndexSymbolConverter();

        converter.ApplicableToSymbol("a").ShouldBeTrue();
        converter.GetSymbol(5, OutputFormat.Redberry).ShouldBe("s5:Redberry");
        converter.GetCode("abc").ShouldBe(3);
        converter.MaxNumberOfSymbols.ShouldBe(32);
        converter.Type.ShouldBe((byte)7);
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
        IndexType[] expected =
        [
            IndexType.LatinLower,
            IndexType.LatinUpper,
            IndexType.GreekLower,
            IndexType.GreekUpper,
            IndexType.Matrix1,
            IndexType.Matrix2,
            IndexType.Matrix3,
            IndexType.Matrix4,
        ];

        Enum.GetValues<IndexType>().ShouldBe(expected);
    }
}

public sealed class INumberTests
{
    [Fact]
    public void ShouldExposeNonGenericNumberContractThroughReal()
    {
        INumber number = new Numeric(3.5);

        number.IntValue().ShouldBe(3);
        number.LongValue().ShouldBe(3L);
        number.DoubleValue().ShouldBe(3.5);
        number.FloatValue().ShouldBe(3.5f);
        number.IsInteger().ShouldBeFalse();
        number.IsNumeric().ShouldBeTrue();
        number.BigIntValue().ShouldBe(new BigInteger(3));
    }

    [Fact]
    public void ShouldExposeGenericNumberOperationsThroughReal()
    {
        INumber<Real> number = new Numeric(2.0);

        number.Add(1).ToString().ShouldBe("3");
        number.Multiply(2).ToString().ShouldBe("4");
        number.Pow(3).ToString().ShouldBe("8");
        number.GetNumericValue().ToString().ShouldBe("2");
        number.Field.ShouldBeSameAs(RealField.Instance);
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

        zero.ShouldBeSameAs(Numeric.Zero);
        one.ShouldBeSameAs(Numeric.One);
        nan.ShouldBeSameAs(Numeric.NaN);
    }

    [Fact]
    public void ShouldSupportArithmeticEqualityAndPredicates()
    {
        Numeric value = new(2.5);

        value.Add(1).ToString().ShouldBe("3.5");
        value.Multiply(2).ToString().ShouldBe("5");
        value.Divide(2).ToString().ShouldBe("1.25");
        value.Pow(2).ToString().ShouldBe("6.25");
        new Numeric(2.5).Equals(value).ShouldBeTrue();
        Numeric.One.IsOne().ShouldBeTrue();
        Numeric.MinusOne.IsMinusOne().ShouldBeTrue();
        Numeric.PositiveInfinity.IsInfinite().ShouldBeTrue();
    }
}
