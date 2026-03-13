using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;
using Xunit;
using PolyComplex = NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly.Complex<NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith.BigRational>;
using PolyComplexRing = NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly.ComplexRing<NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith.BigRational>;

namespace NRedberry.JAS.Tests;

public sealed class PolyUtilTests
{
    [Fact]
    public void ShouldConvertBetweenComplexAndAlgebraicCoefficients()
    {
        PolyComplexRing complexCoefficientRing = new(new BigRational());
        AlgebraicNumberRing<BigRational> algebraicCoefficientRing = complexCoefficientRing.AlgebraicRing();
        GenPolynomialRing<PolyComplex> complexRing = new(complexCoefficientRing, 1, ["x"]);
        GenPolynomialRing<AlgebraicNumber<BigRational>> algebraicRing = new(algebraicCoefficientRing, 1, ["x"]);

        GenPolynomial<PolyComplex> complexPolynomial = complexRing.Univariate(0).Sum(
            new PolyComplex(complexCoefficientRing, new BigRational(1), new BigRational(2)));
        GenPolynomial<AlgebraicNumber<BigRational>> algebraicPolynomial =
            PolyUtil.AlgebraicFromComplex(algebraicRing, complexPolynomial);
        GenPolynomial<PolyComplex> roundTrip = PolyUtil.ComplexFromAlgebraic(complexRing, algebraicPolynomial);

        Assert.Equal(complexPolynomial.ToString(["x"]), roundTrip.ToString(["x"]));
    }

    [Fact]
    public void ShouldConvertBaseCoefficientsToAlgebraicCoefficients()
    {
        AlgebraicNumberRing<BigRational> algebraicCoefficientRing = new PolyComplexRing(new BigRational()).AlgebraicRing();
        GenPolynomialRing<BigRational> rationalRing = new(new BigRational(), 1, ["x"]);
        GenPolynomialRing<AlgebraicNumber<BigRational>> algebraicRing = new(algebraicCoefficientRing, 1, ["x"]);
        GenPolynomial<BigRational> polynomial = rationalRing.Univariate(0).Sum(new BigRational(3));

        GenPolynomial<AlgebraicNumber<BigRational>> converted =
            PolyUtil.ConvertToAlgebraicCoefficients(algebraicRing, polynomial);

        Assert.Equal(polynomial.ToString(["x"]), converted.ToString(["x"]));
    }

    [Fact]
    public void ShouldComputeDerivativeAndPseudoDivisionForBasePolynomials()
    {
        GenPolynomialRing<BigRational> ring = new(new BigRational(), 1, ["x"]);
        GenPolynomial<BigRational> x = ring.Univariate(0);
        GenPolynomial<BigRational> dividend = x.Multiply(new BigRational(2), ExpVector.Create([1L])).Sum(new BigRational(1));
        GenPolynomial<BigRational> divisor = x.Sum(new BigRational(1));
        GenPolynomial<BigRational> exactDividend = divisor.Multiply(x.Sum(new BigRational(2)));

        Assert.Equal("4 x", PolyUtil.BaseDeriviative(dividend).ToString(["x"]));
        Assert.Equal("x + 2", PolyUtil.BasePseudoDivide(exactDividend, divisor).ToString(["x"]));
        Assert.True(PolyUtil.BaseDensePseudoRemainder(exactDividend, divisor).IsZero());
        Assert.True(PolyUtil.BaseSparsePseudoRemainder(exactDividend, divisor).IsZero());
        Assert.Equal("1/2 x^2 + 3/2 x + 1", PolyUtil.CoefficientBasePseudoDivide(exactDividend, new BigRational(2)).ToString(["x"]));
    }

    [Fact]
    public void ShouldHandleRecursivePseudoDivisionHelpers()
    {
        GenPolynomialRing<BigRational> baseRing = new(new BigRational(), 2, ["x", "y"]);
        GenPolynomialRing<GenPolynomial<BigRational>> recursiveRing = baseRing.Recursive(1);
        GenPolynomialRing<BigRational> coefficientRing = (GenPolynomialRing<BigRational>)recursiveRing.CoFac;
        GenPolynomial<BigRational> coefficientDividend = coefficientRing.Univariate(0).Multiply(coefficientRing.Univariate(0));
        GenPolynomial<GenPolynomial<BigRational>> recursivePolynomial = new(recursiveRing);
        recursivePolynomial = recursivePolynomial.Sum(coefficientDividend, ExpVector.Create([1L]));

        GenPolynomial<GenPolynomial<BigRational>> dividedByCoefficient =
            PolyUtil.BaseRecursiveDivide(recursivePolynomial, new BigRational(2));
        GenPolynomial<GenPolynomial<BigRational>> dividedByPolynomial =
            PolyUtil.CoefficientPseudoDivide(recursivePolynomial, coefficientRing.Univariate(0));

        Assert.Equal("1/2 x^2", dividedByCoefficient.Coefficient(ExpVector.Create([1L])).ToString(["x"]));
        Assert.Equal("x", dividedByPolynomial.Coefficient(ExpVector.Create([1L])).ToString(["x"]));
        Assert.Equal(2L, PolyUtil.CoeffMaxDegree(recursivePolynomial));
    }

    [Fact]
    public void ShouldApplyChineseRemainderCoefficientWise()
    {
        ModIntegerRing mod3 = new(new BigInteger(3));
        ModIntegerRing mod5 = new(new BigInteger(5));
        ModIntegerRing mod15 = new(new BigInteger(15));
        GenPolynomialRing<ModInteger> ring3 = new(mod3, 1, ["x"]);
        GenPolynomialRing<ModInteger> ring5 = new(mod5, 1, ["x"]);
        GenPolynomialRing<ModInteger> ring15 = new(mod15, 1, ["x"]);
        GenPolynomial<ModInteger> first = ring3.Univariate(0).Sum(new ModInteger(mod3, 2));
        GenPolynomial<ModInteger> second = ring5.Univariate(0).Multiply(new ModInteger(mod5, 2)).Sum(new ModInteger(mod5, 1));

        GenPolynomial<ModInteger> combined = PolyUtil.ChineseRemainder(ring15, first, new ModInteger(mod5, 2), second);

        Assert.Equal("7 x + 11", combined.ToString(["x"]));
    }

    [Fact]
    public void ShouldSupportRecursiveDistributionEvaluationAndNormalizationHelpers()
    {
        GenPolynomialRing<BigRational> baseRing = new(new BigRational(), 2, ["x", "y"]);
        GenPolynomial<BigRational> polynomial = baseRing.Univariate(1).Sum(new BigRational(2), ExpVector.Create([1L, 0L]));
        GenPolynomialRing<GenPolynomial<BigRational>> recursiveRing = baseRing.Recursive(1);

        GenPolynomial<GenPolynomial<BigRational>> recursive = PolyUtil.Recursive(recursiveRing, polynomial);
        GenPolynomial<BigRational> distributed = PolyUtil.Distribute(baseRing, recursive);
        GenPolynomialRing<BigRational> evaluationRing = new(new BigRational(), 3, ["x", "y", "z"]);
        GenPolynomial<BigRational> evaluationPolynomial = evaluationRing.Univariate(2).Sum(new BigRational(2), ExpVector.Create([1L, 0L, 0L]));
        GenPolynomial<BigRational> evaluated = PolyUtil.EvaluateMain(evaluationRing.Contract(1), evaluationPolynomial, new BigRational(3));
        GenPolynomialRing<BigRational> firstVariableRing = new(new BigRational(), 1, ["x"]);
        GenPolynomialRing<GenPolynomial<BigRational>> firstRecursiveRing = new(firstVariableRing, 1, ["y"]);
        GenPolynomial<GenPolynomial<BigRational>> recursiveForFirst = new(firstRecursiveRing);
        recursiveForFirst = recursiveForFirst.Sum(
            firstVariableRing.Univariate(0).Sum(new BigRational(1)),
            firstRecursiveRing.Evzero.Subst(0, 1));
        recursiveForFirst = recursiveForFirst.Sum(firstVariableRing.FromInteger(2));
        GenPolynomial<BigRational> firstEvaluated = PolyUtil.EvaluateFirstRec(
            firstVariableRing,
            new GenPolynomialRing<BigRational>(new BigRational(), 1, ["y"]),
            recursiveForFirst,
            new BigRational(5));
        GenPolynomial<GenPolynomial<BigRational>> monicRecursive = new(recursiveRing);
        monicRecursive = monicRecursive.Sum(
            ((GenPolynomialRing<BigRational>)recursiveRing.CoFac).FromInteger(2),
            recursiveRing.Evzero.Subst(0, 1));

        Assert.Equal(polynomial.ToString(["x", "y"]), distributed.ToString(["x", "y"]));
        Assert.Equal("x + 6", evaluated.ToString(["x", "y"]));
        Assert.Equal("8", firstEvaluated.ToString(["y"]));
        Assert.Equal("1", PolyUtil.Monic(monicRecursive)!.LeadingBaseCoefficient().LeadingBaseCoefficient().ToString());
    }

    [Fact]
    public void ShouldSupportMappingLeadingExponentAndTrailingCoefficientHelpers()
    {
        GenPolynomialRing<BigInteger> integerRing = new(new BigInteger(), 1, ["x"]);
        GenPolynomialRing<BigRational> rationalRing = new(new BigRational(), 1, ["x"]);
        GenPolynomial<BigInteger> integerPolynomial = integerRing.Univariate(0).Sum(new BigInteger(3));
        GenPolynomial<BigRational> mapped = PolyUtil.Map(rationalRing, integerPolynomial, new BigIntegerToRationalFunctor());

        List<ExpVector?> leading = PolyUtil.LeadingExpVector([mapped]);

        Assert.Equal(integerPolynomial.ToString(["x"]), mapped.ToString(["x"]));
        Assert.NotNull(leading);
        Assert.Equal([1L], leading[0]!.GetVal());
        Assert.Equal("3", PolyUtil.GetTrailingCoefficient(mapped).ToString());
        Assert.Equal(3, PolyUtil.PopCount(new System.Numerics.BigInteger(-13)));
        Assert.Equal("16", PolyUtil.FactorBound(ExpVector.Create([3L])).ToString());
    }

    [Fact]
    public void ShouldConvertIntegerModularAndAlgebraicCoefficientRepresentations()
    {
        GenPolynomialRing<BigInteger> integerRing = new(new BigInteger(), 1, ["x"]);
        GenPolynomialRing<BigInteger> bigIntegerRing = new(new BigInteger(), 1, ["x"]);
        GenPolynomialRing<BigRational> rationalRing = new(new BigRational(), 1, ["x"]);
        ModIntegerRing mod5 = new(new BigInteger(5));
        GenPolynomialRing<ModInteger> modularRing = new(mod5, 1, ["x"]);
        AlgebraicNumberRing<BigRational> algebraicCoefficientRing = new PolyComplexRing(new BigRational()).AlgebraicRing();
        GenPolynomialRing<AlgebraicNumber<BigRational>> algebraicRing = new(algebraicCoefficientRing, 1, ["x"]);
        GenPolynomialRing<GenPolynomial<BigRational>> expandedAlgebraicRing = new(new GenPolynomialRing<BigRational>(new BigRational(), 1, ["t"]), 1, ["x"]);

        GenPolynomial<BigInteger> integerPolynomial = bigIntegerRing.Univariate(0).Sum(new BigInteger(3));
        GenPolynomial<ModInteger> modularPolynomial = modularRing.Univariate(0).Sum(new ModInteger(mod5, 4));
        GenPolynomial<BigRational> rationalPolynomial = rationalRing.Univariate(0).Sum(new BigRational(1, 2));
        GenPolynomial<AlgebraicNumber<BigRational>> algebraicPolynomial =
            PolyUtil.ConvertToAlgebraicCoefficients(algebraicRing, rationalPolynomial);

        object[] cleared = PolyUtil.IntegerFromRationalCoefficientsFactor(integerRing, rationalPolynomial);

        Assert.Equal("x + 3", PolyUtil.FromIntegerCoefficients(rationalRing, integerPolynomial).ToString(["x"]));
        Assert.Equal("x - 1", PolyUtil.IntegerFromModularCoefficients(integerRing, modularPolynomial).ToString(["x"]));
        Assert.Equal("2 x + 1", PolyUtil.IntegerFromRationalCoefficients(integerRing, rationalPolynomial).ToString(["x"]));
        Assert.Equal(System.Numerics.BigInteger.One, cleared[0]);
        Assert.Equal(new System.Numerics.BigInteger(2), cleared[1]);
        Assert.Equal("2 x + 1", ((GenPolynomial<BigInteger>)cleared[2]).ToString(["x"]));
        Assert.Equal(algebraicPolynomial.ToString(["x"]), PolyUtil.FromAlgebraicCoefficients(expandedAlgebraicRing, algebraicPolynomial).ToString(["x"]));
    }
}

file sealed class BigIntegerToRationalFunctor : UnaryFunctor<BigInteger, BigRational>
{
    public BigRational Eval(BigInteger c)
    {
        return new BigRational(c);
    }
}
