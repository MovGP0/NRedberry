using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Gb;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Squarefree decomposition for infinite algebraic extensions of characteristic p fields.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.SquarefreeInfiniteAlgebraicFieldCharP
/// </remarks>
public class SquarefreeInfiniteAlgebraicFieldCharP<C> : SquarefreeFieldCharP<AlgebraicNumber<C>>
    where C : GcdRingElem<C>
{
    private readonly SquarefreeAbstract<C> aengine;

    public SquarefreeInfiniteAlgebraicFieldCharP(RingFactory<AlgebraicNumber<C>> fac)
        : base(fac)
    {
        if (fac.IsFinite())
        {
            throw new ArgumentException("fac must represent an infinite field.", nameof(fac));
        }

        AlgebraicNumberRing<C> algebraicRing = fac as AlgebraicNumberRing<C>
            ?? throw new ArgumentException("fac must be an AlgebraicNumberRing instance.", nameof(fac));

        aengine = SquarefreeFactory.GetImplementation(algebraicRing.Ring);
    }

    public override SortedDictionary<AlgebraicNumber<C>, long> SquarefreeFactors(AlgebraicNumber<C> coefficient)
    {
        ArgumentNullException.ThrowIfNull(coefficient);

        SortedDictionary<AlgebraicNumber<C>, long> factors = new();
        if (coefficient.IsZero())
        {
            return factors;
        }

        if (coefficient.IsOne())
        {
            factors[coefficient] = 1L;
            return factors;
        }

        GenPolynomial<C> value = coefficient.Val;
        if (!value.IsOne())
        {
            SortedDictionary<GenPolynomial<C>, long> valueFactors = aengine.SquarefreeFactors(value);
            foreach (KeyValuePair<GenPolynomial<C>, long> entry in valueFactors)
            {
                AlgebraicNumber<C> factor = new(coefficient.Ring, entry.Key);
                factors[factor] = entry.Value;
            }
        }

        if (factors.Count == 0)
        {
            factors[coefficient] = 1L;
        }

        return factors;
    }

    public override GenPolynomial<AlgebraicNumber<C>>? BaseSquarefreePRoot(GenPolynomial<AlgebraicNumber<C>> polynomial)
    {
        return RootCharacteristic(polynomial);
    }

    public override GenPolynomial<GenPolynomial<AlgebraicNumber<C>>>? RecursiveUnivariateRootCharacteristic(
        GenPolynomial<GenPolynomial<AlgebraicNumber<C>>> polynomial)
    {
        if (polynomial.IsZero())
        {
            return polynomial;
        }

        GenPolynomialRing<GenPolynomial<AlgebraicNumber<C>>> ring = polynomial.Ring;
        if (ring.Nvar > 1)
        {
            throw new ArgumentException($"{polynomial.GetType().Name} only for univariate recursive polynomials.", nameof(polynomial));
        }

        BigInteger characteristic = ring.Characteristic();
        if (characteristic.Sign <= 0)
        {
            throw new ArgumentException($"{polynomial.GetType().Name} only for char p > 0 {ring.CoFac}.", nameof(polynomial));
        }

        long modulus = (long)characteristic;
        GenPolynomial<GenPolynomial<AlgebraicNumber<C>>> result = new(ring);
        foreach (Monomial<GenPolynomial<AlgebraicNumber<C>>> term in polynomial)
        {
            long exponent = term.E.GetVal(0);
            if (exponent % modulus != 0)
            {
                return null;
            }

            long adjustedExponent = exponent / modulus;
            GenPolynomial<AlgebraicNumber<C>>? coefficientRoot = RootCharacteristic(term.C);
            if (coefficientRoot is null)
            {
                return null;
            }

            ExpVector newExponent = ExpVector.Create(1, 0, adjustedExponent);
            result.DoPutToMap(newExponent, coefficientRoot);
        }

        return result;
    }

    private SortedDictionary<AlgebraicNumber<C>, long>? RootCharacteristic(AlgebraicNumber<C> value)
    {
        ArgumentNullException.ThrowIfNull(value);

        AlgebraicNumberRing<C> algebraicRing = value.Ring;
        BigInteger characteristic = algebraicRing.Characteristic();
        if (characteristic.Sign == 0)
        {
            return null;
        }

        SortedDictionary<AlgebraicNumber<C>, long> root = new();
        if (value.IsZero())
        {
            return root;
        }

        if (value.IsOne())
        {
            root[value] = 1L;
            return root;
        }

        long degree = algebraicRing.Modul.Degree(0);
        int variableCount = (int)degree;
        string[] variableNames = GenPolynomialRing<C>.NewVars("c", variableCount);
        GenPolynomialRing<AlgebraicNumber<C>> polynomialRing = new(algebraicRing, variableCount, variableNames);

        List<GenPolynomial<AlgebraicNumber<C>>> univariates = polynomialRing.UnivariateList().ToList();
        GenPolynomial<AlgebraicNumber<C>> cp = new(polynomialRing);
        GenPolynomialRing<C> baseRing = algebraicRing.Ring;
        long basisIndex = 0;
        foreach (GenPolynomial<AlgebraicNumber<C>> univariate in univariates)
        {
            GenPolynomial<C> basisPolynomial = baseRing.Univariate(0, basisIndex++);
            GenPolynomial<AlgebraicNumber<C>> scaled = univariate.Multiply(new AlgebraicNumber<C>(algebraicRing, basisPolynomial));
            cp = cp.Sum(scaled);
        }

        long characteristicValue = (long)characteristic;
        GenPolynomial<AlgebraicNumber<C>> powered = Power<GenPolynomial<AlgebraicNumber<C>>>.PositivePower(cp, characteristicValue);
        GenPolynomialRing<C> equationRing = new(baseRing.CoFac, polynomialRing);
        List<GenPolynomial<C>> equations = new();

        if (degree == characteristicValue && algebraicRing.Modul.Length() == 2)
        {
            foreach (Monomial<AlgebraicNumber<C>> monomial in powered)
            {
                ExpVector exponent = monomial.E;
                GenPolynomial<C> coefficientPolynomial = monomial.C.Val;
                foreach (Monomial<C> coefficientMonomial in coefficientPolynomial)
                {
                    ExpVector innerExponent = coefficientMonomial.E;
                    C cc = coefficientMonomial.C;
                    C pc = value.Val.Coefficient(innerExponent);
                    if (pc is null)
                    {
                        continue;
                    }

                    ElemFactory<C> baseFactory = pc.Factory();
                    if (baseFactory is not RingFactory<C> ringFactory)
                    {
                        throw new InvalidOperationException("Coefficient factory must implement RingFactory.");
                    }

                    C cc1 = ringFactory.FromInteger(1);
                    C pc1 = ringFactory.FromInteger(0);

                    if (cc is AlgebraicNumber<C> && pc is AlgebraicNumber<C>)
                    {
                        throw new NotSupportedException("Multiple algebraic extensions are not supported.");
                    }
                    else if (cc is Quotient<C> quotientCc && pc is Quotient<C> quotientPc)
                    {
                        if (quotientPc.IsConstant())
                        {
                            throw new ArithmeticException("finite field not allowed here");
                        }

                        Quotient<C> ratio = quotientCc.Divide(quotientPc);
                        if (ratio.IsConstant())
                        {
                            cc1 = cc;
                            pc1 = pc;
                        }

                        GenPolynomial<C> equation = new(equationRing, cc1, exponent);
                        equation = equation.Subtract(pc1);
                        equations.Add(equation);
                    }
                }
            }
        }
        else
        {
            foreach (Monomial<AlgebraicNumber<C>> monomial in powered)
            {
                ExpVector exponent = monomial.E;
                GenPolynomial<C> coefficientPolynomial = monomial.C.Val;
                foreach (Monomial<C> coefficientMonomial in coefficientPolynomial)
                {
                    ExpVector innerExponent = coefficientMonomial.E;
                    C cc = coefficientMonomial.C;
                    C pc = value.Val.Coefficient(innerExponent);
                    GenPolynomial<C> equation = new(equationRing, cc, exponent);
                    equation = equation.Subtract(pc);
                    equations.Add(equation);
                }
            }
        }

        Reduction<C> reduction = new ReductionSeq<C>();
        List<GenPolynomial<C>> reducedEquations = reduction.IrreducibleSet(equations);
        GroebnerBase<C> groebner = new();
        int dimension = groebner.CommonZeroTest(reducedEquations.Cast<GenPolynomial<C>?>().ToList());
        if (dimension < 0)
        {
            return null;
        }

        GenPolynomial<C> combination = new(baseRing);
        foreach (GenPolynomial<C> equation in reducedEquations)
        {
            if (equation.Length() <= 1)
            {
                continue;
            }

            if (equation.Length() > 2)
            {
                throw new ArgumentException($"dim > 0 not implemented {equation}");
            }

            ExpVector? leading = equation.LeadingExpVector();
            if (leading is null)
            {
                continue;
            }

            int[]? dependency = leading.DependencyOnVariables();
            if (dependency is null || dependency.Length == 0)
            {
                continue;
            }

            int variableIndex = dependency[0];
            long basisDegree = degree - 1 - variableIndex;
            GenPolynomial<C> basisPolynomial = baseRing.Univariate(0, basisDegree);
            C trailing = equation.TrailingBaseCoefficient().Negate();

            if (leading.MaxDeg() == characteristicValue)
            {
                SortedDictionary<C, long> coefficientFactors = aengine.SquarefreeFactors(trailing);
                if (coefficientFactors.Count > 0)
                {
                    C adjusted = baseRing.CoFac.FromInteger(1);
                    foreach (KeyValuePair<C, long> entry in coefficientFactors)
                    {
                        C factor = entry.Key;
                        long exponentValue = entry.Value;
                        if (exponentValue % characteristicValue == 0)
                        {
                            long quotientExponent = exponentValue / characteristicValue;
                            adjusted = adjusted.Multiply(Power<C>.PositivePower(factor, quotientExponent));
                        }
                        else
                        {
                            adjusted = adjusted.Multiply(factor);
                        }
                    }

                    trailing = adjusted;
                }
            }

            basisPolynomial = basisPolynomial.Multiply(trailing);
            combination = combination.Sum(basisPolynomial);
        }

        AlgebraicNumber<C> rootValue = new(algebraicRing, combination);
        root[rootValue] = 1L;
        return root;
    }

    private GenPolynomial<AlgebraicNumber<C>>? RootCharacteristic(GenPolynomial<AlgebraicNumber<C>> polynomial)
    {
        ArgumentNullException.ThrowIfNull(polynomial);

        if (polynomial.IsZero())
        {
            return polynomial;
        }

        GenPolynomialRing<AlgebraicNumber<C>> ring = polynomial.Ring;
        if (ring.Nvar > 1)
        {
            GenPolynomialRing<AlgebraicNumber<C>> coefficientRing = ring.Contract(1);
            GenPolynomialRing<GenPolynomial<AlgebraicNumber<C>>> recursiveRing = new(coefficientRing, 1);
            GenPolynomial<GenPolynomial<AlgebraicNumber<C>>> recursive = PolyUtil.Recursive(recursiveRing, polynomial);
            GenPolynomial<GenPolynomial<AlgebraicNumber<C>>>? recursiveRoot = RecursiveUnivariateRootCharacteristic(recursive);
            return recursiveRoot is null ? null : PolyUtil.Distribute(ring, recursiveRoot);
        }

        BigInteger characteristic = ring.Characteristic();
        if (characteristic.Sign <= 0)
        {
            throw new ArgumentException($"{polynomial.GetType().Name} only for ModInteger polynomials {ring.CoFac}.", nameof(polynomial));
        }

        long modulus = (long)characteristic;
        GenPolynomial<AlgebraicNumber<C>> result = new(ring);
        foreach (Monomial<AlgebraicNumber<C>> term in polynomial)
        {
            long exponent = term.E.GetVal(0);
            if (exponent % modulus != 0)
            {
                return null;
            }

            long adjustedExponent = exponent / modulus;
            SortedDictionary<AlgebraicNumber<C>, long>? coefficientRoot = RootCharacteristic(term.C);
            if (coefficientRoot is null)
            {
                return null;
            }

            AlgebraicNumber<C> coefficient = ring.CoFac.FromInteger(1);
            foreach (KeyValuePair<AlgebraicNumber<C>, long> entry in coefficientRoot)
            {
                AlgebraicNumber<C> factor = entry.Key;
                long factorExponent = entry.Value;
                if (factorExponent > 1)
                {
                    factor = Power<AlgebraicNumber<C>>.PositivePower(factor, factorExponent);
                }

                coefficient = coefficient.Multiply(factor);
            }

            ExpVector newExponent = ExpVector.Create(1, 0, adjustedExponent);
            result.DoPutToMap(newExponent, coefficient);
        }

        return result;
    }
}
