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

        roundTrip.ToString(["x"]).ShouldBe(complexPolynomial.ToString(["x"]));
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

        converted.ToString(["x"]).ShouldBe(polynomial.ToString(["x"]));
    }

    [Fact]
    public void ShouldComputeDerivativeAndPseudoDivisionForBasePolynomials()
    {
        GenPolynomialRing<BigRational> ring = new(new BigRational(), 1, ["x"]);
        GenPolynomial<BigRational> x = ring.Univariate(0);
        GenPolynomial<BigRational> dividend = x.Multiply(new BigRational(2), ExpVector.Create([1L])).Sum(new BigRational(1));
        GenPolynomial<BigRational> divisor = x.Sum(new BigRational(1));
        GenPolynomial<BigRational> exactDividend = divisor.Multiply(x.Sum(new BigRational(2)));

        PolyUtil.BaseDeriviative(dividend).ToString(["x"]).ShouldBe("4 x");
        PolyUtil.BasePseudoDivide(exactDividend, divisor).ToString(["x"]).ShouldBe("x + 2");
        PolyUtil.BaseDensePseudoRemainder(exactDividend, divisor).IsZero().ShouldBeTrue();
        PolyUtil.BaseSparsePseudoRemainder(exactDividend, divisor).IsZero().ShouldBeTrue();
        PolyUtil.CoefficientBasePseudoDivide(exactDividend, new BigRational(2)).ToString(["x"]).ShouldBe("1/2 x^2 + 3/2 x + 1");
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

        dividedByCoefficient.Coefficient(ExpVector.Create([1L])).ToString(["x"]).ShouldBe("1/2 x^2");
        dividedByPolynomial.Coefficient(ExpVector.Create([1L])).ToString(["x"]).ShouldBe("x");
        PolyUtil.CoeffMaxDegree(recursivePolynomial).ShouldBe(2L);
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

        combined.ToString(["x"]).ShouldBe("7 x + 11");
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

        distributed.ToString(["x", "y"]).ShouldBe(polynomial.ToString(["x", "y"]));
        evaluated.ToString(["x", "y"]).ShouldBe("x + 6");
        firstEvaluated.ToString(["y"]).ShouldBe("8");
        PolyUtil.Monic(monicRecursive)!.LeadingBaseCoefficient().LeadingBaseCoefficient().ToString().ShouldBe("1");
    }

    [Fact]
    public void ShouldSupportMappingLeadingExponentAndTrailingCoefficientHelpers()
    {
        GenPolynomialRing<BigInteger> integerRing = new(new BigInteger(), 1, ["x"]);
        GenPolynomialRing<BigRational> rationalRing = new(new BigRational(), 1, ["x"]);
        GenPolynomial<BigInteger> integerPolynomial = integerRing.Univariate(0).Sum(new BigInteger(3));
        GenPolynomial<BigRational> mapped = PolyUtil.Map(rationalRing, integerPolynomial, new BigIntegerToRationalFunctor());

        List<ExpVector?> leading = PolyUtil.LeadingExpVector([mapped]);

        mapped.ToString(["x"]).ShouldBe(integerPolynomial.ToString(["x"]));
        leading.ShouldNotBeNull();
        leading[0]!.GetVal().ShouldBe([1L]);
        PolyUtil.GetTrailingCoefficient(mapped).ToString().ShouldBe("3");
        PolyUtil.PopCount(new System.Numerics.BigInteger(-13)).ShouldBe(3);
        PolyUtil.FactorBound(ExpVector.Create([3L])).ToString().ShouldBe("16");
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

        PolyUtil.FromIntegerCoefficients(rationalRing, integerPolynomial).ToString(["x"]).ShouldBe("x + 3");
        PolyUtil.IntegerFromModularCoefficients(integerRing, modularPolynomial).ToString(["x"]).ShouldBe("x - 1");
        PolyUtil.IntegerFromRationalCoefficients(integerRing, rationalPolynomial).ToString(["x"]).ShouldBe("2 x + 1");
        cleared[0].ShouldBe(System.Numerics.BigInteger.One);
        cleared[1].ShouldBe(new System.Numerics.BigInteger(2));
        ((GenPolynomial<BigInteger>)cleared[2]).ToString(["x"]).ShouldBe("2 x + 1");
        PolyUtil.FromAlgebraicCoefficients(expandedAlgebraicRing, algebraicPolynomial).ToString(["x"]).ShouldBe(algebraicPolynomial.ToString(["x"]));
    }

    [Fact]
    public void ShouldInterpolateUnivariateAndRecursivePolynomials()
    {
        GenPolynomialRing<BigRational> ring = new(new BigRational(), 1, ["x"]);
        GenPolynomial<BigRational> source = ring.Univariate(0).Sum(new BigRational(1));
        GenPolynomial<BigRational> modulus = ring.Univariate(0, 2).Sum(new BigRational(1));

        GenPolynomial<BigRational> interpolated = PolyUtil.Interpolate(
            ring,
            source,
            modulus,
            new BigRational(1, 5),
            new BigRational(10),
            new BigRational(2));

        PolyUtil.EvaluateMain(ring.CoFac, interpolated, new BigRational(2)).ToString().ShouldBe("10");
        PolyUtil.BaseSparsePseudoRemainder(interpolated.Subtract(source), modulus).IsZero().ShouldBeTrue();

        GenPolynomialRing<BigRational> baseRing = new(new BigRational(), 2, ["x", "y"]);
        GenPolynomialRing<GenPolynomial<BigRational>> recursiveRing = baseRing.Recursive(1);
        GenPolynomialRing<BigRational> coefficientRing = (GenPolynomialRing<BigRational>)recursiveRing.CoFac;
        GenPolynomial<GenPolynomial<BigRational>> recursiveSource = new(recursiveRing);
        recursiveSource = recursiveSource.Sum(coefficientRing.FromInteger(1), ExpVector.Create([1L]));
        recursiveSource = recursiveSource.Sum(coefficientRing.Univariate(0));

        GenPolynomial<BigRational> evaluation = new(coefficientRing);
        evaluation = evaluation.Sum(new BigRational(5), ExpVector.Create([1L]));
        evaluation = evaluation.Sum(new BigRational(3));

        GenPolynomial<BigRational> linearModulus = coefficientRing.Univariate(0).Sum(new BigRational(1));
        GenPolynomial<GenPolynomial<BigRational>> recursiveInterpolated = PolyUtil.Interpolate(
            recursiveRing,
            recursiveSource,
            linearModulus,
            new BigRational(1, 3),
            evaluation,
            new BigRational(2));

        GenPolynomial<BigRational> yCoefficient = recursiveInterpolated.Coefficient(ExpVector.Create([1L]));
        GenPolynomial<BigRational> constantCoefficient = recursiveInterpolated.Coefficient(ExpVector.Create([0L]));
        GenPolynomial<BigRational> yDifference =
            PolyUtil.BaseSparsePseudoRemainder(
                yCoefficient.Subtract(recursiveSource.Coefficient(ExpVector.Create([1L]))),
                linearModulus);
        GenPolynomial<BigRational> constantDifference =
            PolyUtil.BaseSparsePseudoRemainder(
                constantCoefficient.Subtract(recursiveSource.Coefficient(ExpVector.Create([0L]))),
                linearModulus);

        PolyUtil.EvaluateMain(coefficientRing.CoFac, yCoefficient, new BigRational(2)).ToString().ShouldBe("5");
        PolyUtil.EvaluateMain(coefficientRing.CoFac, constantCoefficient, new BigRational(2)).ToString().ShouldBe("3");
        yDifference.IsZero().ShouldBeTrue();
        constantDifference.IsZero().ShouldBeTrue();
    }

    [Fact]
    public void ShouldHandleRecursiveDerivativeDivisionAndPseudoRemainderHelpers()
    {
        GenPolynomialRing<BigRational> baseRing = new(new BigRational(), 2, ["x", "y"]);
        GenPolynomialRing<GenPolynomial<BigRational>> recursiveRing = baseRing.Recursive(1);
        GenPolynomialRing<BigRational> coefficientRing = (GenPolynomialRing<BigRational>)recursiveRing.CoFac;

        GenPolynomial<GenPolynomial<BigRational>> recursivePolynomial = new(recursiveRing);
        recursivePolynomial = recursivePolynomial.Sum(coefficientRing.Univariate(0), ExpVector.Create([2L]));
        recursivePolynomial = recursivePolynomial.Sum(coefficientRing.FromInteger(3), ExpVector.Create([1L]));
        recursivePolynomial = recursivePolynomial.Sum(coefficientRing.FromInteger(1));

        GenPolynomial<GenPolynomial<BigRational>> derivative = PolyUtil.RecursiveDeriviative(recursivePolynomial);

        derivative.Coefficient(ExpVector.Create([1L])).ToString(["x"]).ShouldBe("2 x");
        derivative.Coefficient(ExpVector.Create([0L])).ToString(["x"]).ShouldBe("3");

        GenPolynomial<BigRational> coefficientDivisor = coefficientRing.Univariate(0).Sum(new BigRational(1));
        GenPolynomial<GenPolynomial<BigRational>> divisibleRecursive = new(recursiveRing);
        divisibleRecursive = divisibleRecursive.Sum(
            coefficientDivisor.Multiply(coefficientRing.Univariate(0).Sum(new BigRational(2))),
            ExpVector.Create([1L]));
        divisibleRecursive = divisibleRecursive.Sum(coefficientDivisor.Multiply(new BigRational(2)));

        GenPolynomial<GenPolynomial<BigRational>> divided = PolyUtil.RecursiveDivide(divisibleRecursive, coefficientDivisor);

        divided.Coefficient(ExpVector.Create([1L])).ToString(["x"]).ShouldBe("x + 2");
        divided.Coefficient(ExpVector.Create([0L])).ToString(["x"]).ShouldBe("2");

        GenPolynomial<GenPolynomial<BigRational>> recursiveDivisor = new(recursiveRing);
        recursiveDivisor = recursiveDivisor.Sum(coefficientRing.FromInteger(1), ExpVector.Create([1L]));
        recursiveDivisor = recursiveDivisor.Sum(coefficientRing.FromInteger(1));

        GenPolynomial<GenPolynomial<BigRational>> recursiveQuotient = new(recursiveRing);
        recursiveQuotient = recursiveQuotient.Sum(coefficientRing.FromInteger(1), ExpVector.Create([1L]));
        recursiveQuotient = recursiveQuotient.Sum(coefficientRing.FromInteger(2));

        GenPolynomial<GenPolynomial<BigRational>> recursiveDividend = recursiveDivisor.Multiply(recursiveQuotient);
        GenPolynomial<GenPolynomial<BigRational>> pseudoQuotient =
            PolyUtil.RecursivePseudoDivide(recursiveDividend, recursiveDivisor);

        pseudoQuotient.Coefficient(ExpVector.Create([1L])).ToString(["x"]).ShouldBe("1");
        pseudoQuotient.Coefficient(ExpVector.Create([0L])).ToString(["x"]).ShouldBe("2");
        PolyUtil.RecursiveDensePseudoRemainder(recursiveDividend, recursiveDivisor).IsZero().ShouldBeTrue();
        PolyUtil.RecursiveSparsePseudoRemainder(recursiveDividend, recursiveDivisor).IsZero().ShouldBeTrue();
        PolyUtil.RecursivePseudoRemainder(recursiveDividend, recursiveDivisor).IsZero().ShouldBeTrue();
    }

    [Fact]
    public void ShouldSubstituteAndSwitchRecursiveVariables()
    {
        GenPolynomialRing<BigRational> univariateRing = new(new BigRational(), 1, ["x"]);
        GenPolynomial<BigRational> polynomial = univariateRing.Univariate(0, 2).Sum(new BigRational(1));
        GenPolynomial<BigRational> substitution = univariateRing.Univariate(0).Sum(new BigRational(1));

        GenPolynomial<BigRational> substituted = PolyUtil.SubstituteMain(polynomial, substitution);

        substituted.ToString(["x"]).ShouldBe("x^2 + 2 x + 2");

        GenPolynomialRing<BigRational> baseRing = new(new BigRational(), 2, ["x", "y"]);
        GenPolynomial<BigRational> basePolynomial = baseRing.Univariate(1).Sum(new BigRational(2), ExpVector.Create([1L, 0L]));
        GenPolynomialRing<GenPolynomial<BigRational>> recursiveRing = baseRing.Recursive(1);
        GenPolynomial<GenPolynomial<BigRational>> recursive = PolyUtil.Recursive(recursiveRing, basePolynomial);

        GenPolynomial<GenPolynomial<BigRational>> switched = PolyUtil.SwitchVariables(recursive);
        GenPolynomial<GenPolynomial<BigRational>> roundTrip = PolyUtil.SwitchVariables(switched);

        PolyUtil.Distribute(baseRing, roundTrip).ToString(["x", "y"]).ShouldBe(basePolynomial.ToString(["x", "y"]));
    }
}

file sealed class BigIntegerToRationalFunctor : UnaryFunctor<BigInteger, BigRational>
{
    public BigRational Eval(BigInteger c)
    {
        return new BigRational(c);
    }
}
