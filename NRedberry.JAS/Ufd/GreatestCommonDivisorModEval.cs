using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Greatest common divisor algorithms with modular evaluation algorithm for recursion.
/// </summary>
/// <typeparam name="MOD">modular coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.GreatestCommonDivisorModEval
/// </remarks>
public class GreatestCommonDivisorModEval<MOD> : GreatestCommonDivisorAbstract<MOD>
    where MOD : GcdRingElem<MOD>, Modular
{
    private const bool Debug = false;

    protected readonly GreatestCommonDivisorAbstract<MOD> ModularEngine;

    public GreatestCommonDivisorModEval()
    {
        ModularEngine = new GreatestCommonDivisorSimple<MOD>();
    }

    public override GenPolynomial<MOD> BaseGcd(GenPolynomial<MOD> first, GenPolynomial<MOD> second)
    {
        return ModularEngine.BaseGcd(first, second);
    }

    public override GenPolynomial<GenPolynomial<MOD>> RecursiveUnivariateGcd(
        GenPolynomial<GenPolynomial<MOD>> first,
        GenPolynomial<GenPolynomial<MOD>> second)
    {
        return ModularEngine.RecursiveUnivariateGcd(first, second);
    }

    public override GenPolynomial<MOD> Gcd(GenPolynomial<MOD> first, GenPolynomial<MOD> second)
    {
        if (second.IsZero())
        {
            return first;
        }

        if (first.IsZero())
        {
            return second;
        }

        GenPolynomialRing<MOD> ring = first.Ring;
        if (ring.Nvar <= 1)
        {
            return BaseGcd(first, second);
        }

        long degreeFirst = first.Degree(ring.Nvar - 1);
        long degreeSecond = second.Degree(ring.Nvar - 1);
        if (degreeFirst == 0 && degreeSecond == 0)
        {
            GenPolynomialRing<GenPolynomial<MOD>> recursiveRing = ring.Recursive(1);
            GenPolynomial<GenPolynomial<MOD>> recursiveFirst = PolyUtil.Recursive(recursiveRing, first);
            GenPolynomial<GenPolynomial<MOD>> recursiveSecond = PolyUtil.Recursive(recursiveRing, second);
            GenPolynomial<MOD> leadingFirst = recursiveFirst.LeadingBaseCoefficient();
            GenPolynomial<MOD> leadingSecond = recursiveSecond.LeadingBaseCoefficient();
            GenPolynomial<MOD> gcdCoefficients = Gcd(leadingFirst, leadingSecond);
            return gcdCoefficients.Extend(ring, 0, 0);
        }

        GenPolynomial<MOD> q;
        GenPolynomial<MOD> r;
        if (degreeSecond > degreeFirst)
        {
            r = first;
            q = second;
            long temp = degreeSecond;
            degreeSecond = degreeFirst;
            degreeFirst = temp;
        }
        else
        {
            q = first;
            r = second;
        }

        q = q.Abs();
        r = r.Abs();

        ModularRingFactory<MOD> coefficientFactory = (ModularRingFactory<MOD>)ring.CoFac;
        GenPolynomialRing<GenPolynomial<MOD>> recursiveRingFull = ring.Recursive(ring.Nvar - 1);
        GenPolynomialRing<MOD> modularRing = new (coefficientFactory, recursiveRingFull.Nvar, recursiveRingFull.Tord, recursiveRingFull.GetVars());
        GenPolynomialRing<MOD> univariateRing = (GenPolynomialRing<MOD>)recursiveRingFull.CoFac;

        GenPolynomial<GenPolynomial<MOD>> recursiveQ = PolyUtil.Recursive(recursiveRingFull, q);
        GenPolynomial<GenPolynomial<MOD>> recursiveR = PolyUtil.Recursive(recursiveRingFull, r);

        GenPolynomial<MOD> contentR = RecursiveContent(recursiveR);
        GenPolynomial<MOD> contentQ = RecursiveContent(recursiveQ);
        GenPolynomial<MOD> contentGcd = Gcd(contentR, contentQ);

        recursiveR = PolyUtil.RecursiveDivide(recursiveR, contentR);
        recursiveQ = PolyUtil.RecursiveDivide(recursiveQ, contentQ);

        if (recursiveR.IsOne())
        {
            GenPolynomial<GenPolynomial<MOD>> multiplied = recursiveR.Multiply(contentGcd);
            return PolyUtil.Distribute(ring, multiplied);
        }

        if (recursiveQ.IsOne())
        {
            GenPolynomial<GenPolynomial<MOD>> multiplied = recursiveQ.Multiply(contentGcd);
            return PolyUtil.Distribute(ring, multiplied);
        }

        GenPolynomial<MOD> leadingR = recursiveR.LeadingBaseCoefficient();
        GenPolynomial<MOD> leadingQ = recursiveQ.LeadingBaseCoefficient();
        GenPolynomial<MOD> normalization = Gcd(leadingR, leadingQ);

        ExpVector rDegreeVector = recursiveR.DegreeVector();
        ExpVector qDegreeVector = recursiveQ.DegreeVector();
        long rd0 = PolyUtil.CoeffMaxDegree(recursiveR);
        long qd0 = PolyUtil.CoeffMaxDegree(recursiveQ);
        long cd0 = normalization.Degree(0);
        long interpolationBound = Math.Max(rd0, qd0) + cd0;

        ExpVector targetDegreeVector = rDegreeVector.Subst(0, rDegreeVector.GetVal(0) + 1);

        MOD increment = coefficientFactory.FromInteger(1);
        long iteration = 0;
        long modulusLimit = coefficientFactory.GetIntegerModul().LongValue() - 1;
        MOD end = coefficientFactory.FromInteger(modulusLimit);
        GenPolynomial<MOD>? interpolationModulus = null;
        GenPolynomial<GenPolynomial<MOD>>? interpolationPolynomial = null;

        for (MOD evaluationPoint = coefficientFactory.FromInteger(0);
             evaluationPoint.CompareTo(end) <= 0;
             evaluationPoint = evaluationPoint.Sum(increment))
        {
            if (++iteration >= modulusLimit)
            {
                return ModularEngine.Gcd(first, second);
            }

            MOD normalizationFactor = PolyUtil.EvaluateMain(coefficientFactory, normalization, evaluationPoint);
            if (normalizationFactor.IsZero())
            {
                continue;
            }

            GenPolynomial<MOD> mappedQ = PolyUtil.EvaluateFirstRec(univariateRing, modularRing, recursiveQ, evaluationPoint);
            if (mappedQ.IsZero() || !mappedQ.DegreeVector().Equals(qDegreeVector))
            {
                continue;
            }

            GenPolynomial<MOD> mappedR = PolyUtil.EvaluateFirstRec(univariateRing, modularRing, recursiveR, evaluationPoint);
            if (mappedR.IsZero() || !mappedR.DegreeVector().Equals(rDegreeVector))
            {
                continue;
            }

            GenPolynomial<MOD> modularGcd = Gcd(mappedR, mappedQ);

            if (modularGcd.IsConstant())
            {
                if (contentGcd.Ring.Nvar < modularGcd.Ring.Nvar)
                {
                    contentGcd = contentGcd.Extend(modularRing, 0, 0);
                }

                modularGcd = modularGcd.Abs().Multiply(contentGcd);
                return modularGcd.Extend(ring, 0, 0);
            }

            ExpVector modularDegreeVector = modularGcd.DegreeVector();

            if (targetDegreeVector.Equals(modularDegreeVector))
            {
                if (interpolationModulus is not null && interpolationModulus.Degree(0) > interpolationBound)
                {
                    // skip - already sufficient precision
                }
            }
            else
            {
                bool ok = false;
                if (targetDegreeVector.MultipleOf(modularDegreeVector))
                {
                    interpolationModulus = null;
                    ok = true;
                }

                if (modularDegreeVector.MultipleOf(targetDegreeVector))
                {
                    continue;
                }

                if (!ok)
                {
                    interpolationModulus = null;
                    continue;
                }
            }

            modularGcd = modularGcd.Multiply(normalizationFactor);

            if (interpolationModulus is null)
            {
                interpolationModulus = univariateRing.FromInteger(1);
                interpolationPolynomial = new GenPolynomial<GenPolynomial<MOD>>(recursiveRingFull);
                targetDegreeVector = targetDegreeVector.Gcd(modularDegreeVector);
            }

            MOD modulusEvaluation = PolyUtil.EvaluateMain(coefficientFactory, interpolationModulus, evaluationPoint).Inverse();
            interpolationPolynomial = PolyUtil.Interpolate(
                recursiveRingFull,
                interpolationPolynomial!,
                interpolationModulus,
                modulusEvaluation,
                modularGcd,
                evaluationPoint);

            GenPolynomial<MOD> linearFactor = univariateRing.Univariate(0, 1)
                .Subtract(univariateRing.FromInteger(1).Multiply(evaluationPoint));
            interpolationModulus = interpolationModulus.Multiply(linearFactor);

            if (interpolationModulus.Degree(0) > interpolationBound)
            {
                break;
            }
        }

        if (interpolationPolynomial is null)
        {
            return ModularEngine.Gcd(first, second);
        }

        interpolationPolynomial = RecursivePrimitivePart(interpolationPolynomial).Abs();
        interpolationPolynomial = interpolationPolynomial.Multiply(contentGcd);
        return PolyUtil.Distribute(ring, interpolationPolynomial);
    }

    public override GenPolynomial<MOD> Resultant(GenPolynomial<MOD> first, GenPolynomial<MOD> second)
    {
        if (second.IsZero())
        {
            return second;
        }

        if (first.IsZero())
        {
            return first;
        }

        GenPolynomialRing<MOD> ring = first.Ring;
        if (ring.Nvar <= 1)
        {
            return ModularEngine.BaseResultant(first, second);
        }

        long degreeFirst = first.Degree(ring.Nvar - 1);
        long degreeSecond = second.Degree(ring.Nvar - 1);
        if (degreeFirst == 0 && degreeSecond == 0)
        {
            GenPolynomialRing<GenPolynomial<MOD>> recursiveRing = ring.Recursive(1);
            GenPolynomial<GenPolynomial<MOD>> recursiveFirst = PolyUtil.Recursive(recursiveRing, first);
            GenPolynomial<GenPolynomial<MOD>> recursiveSecond = PolyUtil.Recursive(recursiveRing, second);
            GenPolynomial<MOD> leadingFirst = recursiveFirst.LeadingBaseCoefficient();
            GenPolynomial<MOD> leadingSecond = recursiveSecond.LeadingBaseCoefficient();
            GenPolynomial<MOD> resultantCoefficients = Resultant(leadingFirst, leadingSecond);
            return resultantCoefficients.Extend(ring, 0, 0);
        }

        GenPolynomial<MOD> q;
        GenPolynomial<MOD> r;
        if (degreeSecond > degreeFirst)
        {
            r = first;
            q = second;
            long temp = degreeSecond;
            degreeSecond = degreeFirst;
            degreeFirst = temp;
        }
        else
        {
            q = first;
            r = second;
        }

        q = q.Abs();
        r = r.Abs();

        ModularRingFactory<MOD> coefficientFactory = (ModularRingFactory<MOD>)ring.CoFac;
        GenPolynomialRing<GenPolynomial<MOD>> recursiveRingFull = ring.Recursive(ring.Nvar - 1);
        GenPolynomialRing<MOD> modularRing = new (coefficientFactory, recursiveRingFull.Nvar, recursiveRingFull.Tord, recursiveRingFull.GetVars());
        GenPolynomialRing<MOD> univariateRing = (GenPolynomialRing<MOD>)recursiveRingFull.CoFac;

        GenPolynomial<GenPolynomial<MOD>> recursiveQ = PolyUtil.Recursive(recursiveRingFull, q);
        GenPolynomial<GenPolynomial<MOD>> recursiveR = PolyUtil.Recursive(recursiveRingFull, r);

        ExpVector qDegreeVector = recursiveQ.DegreeVector();
        ExpVector rDegreeVector = recursiveR.DegreeVector();

        long qd0 = PolyUtil.CoeffMaxDegree(recursiveQ);
        long rd0 = PolyUtil.CoeffMaxDegree(recursiveR);
        qd0 = qd0 == 0 ? 1 : qd0;
        rd0 = rd0 == 0 ? 1 : rd0;
        long qd1 = recursiveQ.Degree();
        long rd1 = recursiveR.Degree();
        qd1 = qd1 == 0 ? 1 : qd1;
        rd1 = rd1 == 0 ? 1 : rd1;
        long interpolationBound = qd0 * rd1 + rd0 * qd1 + 1;

        MOD increment = coefficientFactory.FromInteger(1);
        long iteration = 0;
        long modulusLimit = coefficientFactory.GetIntegerModul().LongValue() - 1;
        MOD end = coefficientFactory.FromInteger(modulusLimit);

        GenPolynomial<MOD>? interpolationModulus = null;
        GenPolynomial<GenPolynomial<MOD>>? interpolationPolynomial = null;

        for (MOD evaluationPoint = coefficientFactory.FromInteger(0);
             evaluationPoint.CompareTo(end) <= 0;
             evaluationPoint = evaluationPoint.Sum(increment))
        {
            if (++iteration >= modulusLimit)
            {
                return ModularEngine.Resultant(first, second);
            }

            GenPolynomial<MOD> mappedQ = PolyUtil.EvaluateFirstRec(univariateRing, modularRing, recursiveQ, evaluationPoint);
            if (mappedQ.IsZero() || !mappedQ.DegreeVector().Equals(qDegreeVector))
            {
                continue;
            }

            GenPolynomial<MOD> mappedR = PolyUtil.EvaluateFirstRec(univariateRing, modularRing, recursiveR, evaluationPoint);
            if (mappedR.IsZero() || !mappedR.DegreeVector().Equals(rDegreeVector))
            {
                continue;
            }

            GenPolynomial<MOD> modularResultant = Resultant(mappedQ, mappedR);

            if (interpolationModulus is null)
            {
                interpolationModulus = univariateRing.FromInteger(1);
                interpolationPolynomial = new GenPolynomial<GenPolynomial<MOD>>(recursiveRingFull);
            }

            MOD modulusEvaluation = PolyUtil.EvaluateMain(coefficientFactory, interpolationModulus, evaluationPoint).Inverse();
            interpolationPolynomial = PolyUtil.Interpolate(
                recursiveRingFull,
                interpolationPolynomial!,
                interpolationModulus,
                modulusEvaluation,
                modularResultant,
                evaluationPoint);

            GenPolynomial<MOD> linearFactor = univariateRing.Univariate(0, 1)
                .Subtract(univariateRing.FromInteger(1).Multiply(evaluationPoint));
            interpolationModulus = interpolationModulus.Multiply(linearFactor);

            if (interpolationModulus.Degree(0) > interpolationBound)
            {
                break;
            }
        }

        if (interpolationPolynomial is null)
        {
            return ModularEngine.Resultant(first, second);
        }

        GenPolynomial<GenPolynomial<MOD>> distributed = RecursivePrimitivePart(interpolationPolynomial).Abs();
        return PolyUtil.Distribute(ring, distributed);
    }
}
