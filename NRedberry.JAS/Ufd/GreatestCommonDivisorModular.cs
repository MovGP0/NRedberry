using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Greatest common divisor algorithms with modular computation and Chinese remainder accumulation.
/// </summary>
/// <typeparam name="MOD">modular coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.GreatestCommonDivisorModular
/// </remarks>
public class GreatestCommonDivisorModular<MOD> : GreatestCommonDivisorAbstract<BigInteger>
    where MOD : GcdRingElem<MOD>, Modular
{
    private const int PrimeBudget = 30;

    protected readonly GreatestCommonDivisorAbstract<MOD> modularEngine;
    protected readonly GreatestCommonDivisorAbstract<BigInteger> integerFallback;

    public GreatestCommonDivisorModular()
        : this(false)
    {
    }

    public GreatestCommonDivisorModular(bool simple)
    {
        modularEngine = simple ? new GreatestCommonDivisorSimple<MOD>() : new GreatestCommonDivisorModEval<MOD>();
        integerFallback = new GreatestCommonDivisorSubres<BigInteger>();
    }

    public override GenPolynomial<BigInteger> BaseGcd(GenPolynomial<BigInteger> first, GenPolynomial<BigInteger> second)
    {
        return integerFallback.BaseGcd(first, second);
    }

    public override GenPolynomial<GenPolynomial<BigInteger>> RecursiveUnivariateGcd(
        GenPolynomial<GenPolynomial<BigInteger>> first,
        GenPolynomial<GenPolynomial<BigInteger>> second)
    {
        return integerFallback.RecursiveUnivariateGcd(first, second);
    }

    public override GenPolynomial<BigInteger> Gcd(GenPolynomial<BigInteger> first, GenPolynomial<BigInteger> second)
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(second);

        if (second.IsZero())
        {
            return first;
        }

        if (first.IsZero())
        {
            return second;
        }

        // cast to (ModularRingFactory) is not possible. use (ModularRingFactory<MOD>) instead.
        throw new NotImplementedException();
    }

    public override GenPolynomial<BigInteger> Resultant(GenPolynomial<BigInteger> first, GenPolynomial<BigInteger> second)
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(second);

        if (second.IsZero())
        {
            return second;
        }

        if (first.IsZero())
        {
            return first;
        }

        // cast to (ModularRingFactory) is not possible. use (ModularRingFactory<MOD>) instead.
        throw new NotImplementedException();
    }

    public override GenPolynomial<BigInteger> BaseResultant(GenPolynomial<BigInteger> first, GenPolynomial<BigInteger> second)
    {
        return Resultant(first, second);
    }

    public override GenPolynomial<GenPolynomial<BigInteger>> RecursiveUnivariateResultant(
        GenPolynomial<GenPolynomial<BigInteger>> first,
        GenPolynomial<GenPolynomial<BigInteger>> second)
    {
        return RecursiveResultant(first, second);
    }

    private static bool DegreeVectorsEqual(ExpVector? left, ExpVector? right)
    {
        if (left is null && right is null)
        {
            return true;
        }

        if (left is null || right is null)
        {
            return false;
        }

        return left.Equals(right);
    }
}
