using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

public static partial class HenselUtil
{
    /// <summary>
    /// Quadratic Hensel lifting that refines factors A and B along with Bézout coefficients so that <c>C = A₁ · B₁</c> modulo higher powers of p.
    /// </summary>
    /// <typeparam name="MOD">Modular coefficient type.</typeparam>
    /// <param name="C">Integral polynomial being factored.</param>
    /// <param name="M">Coefficient bound for lifted factors.</param>
    /// <param name="A">First modular factor.</param>
    /// <param name="B">Second modular factor.</param>
    /// <param name="S">Initial Bézout coefficient for A.</param>
    /// <param name="T">Initial Bézout coefficient for B.</param>
    /// <returns>Hensel approximation containing lifted factors and coefficients.</returns>
    /// <remarks>Original Java method: HenselUtil#liftHenselQuadratic(C,M,A,B,S,T).</remarks>
    public static HenselApprox<MOD> LiftHenselQuadratic<MOD>(
        GenPolynomial<BigInteger> C,
        BigInteger M,
        GenPolynomial<MOD> A,
        GenPolynomial<MOD> B,
        GenPolynomial<MOD> S,
        GenPolynomial<MOD> T)
        where MOD : GcdRingElem<MOD>, Modular
    {
        ArgumentNullException.ThrowIfNull(C);
        ArgumentNullException.ThrowIfNull(A);
        ArgumentNullException.ThrowIfNull(B);
        ArgumentNullException.ThrowIfNull(S);
        ArgumentNullException.ThrowIfNull(T);

        if (C.IsZero())
        {
            return new HenselApprox<MOD>(C, C, A, B);
        }

        if (A.IsZero() || B.IsZero())
        {
            throw new ArgumentException("A and B must be nonzero.");
        }

        GenPolynomialRing<BigInteger> integerRing = C.Ring;
        if (integerRing.Nvar != 1)
        {
            throw new ArgumentException("polynomial ring must be univariate");
        }

        GenPolynomialRing<MOD> polynomialRing = A.Ring;
        ModularRingFactory<MOD> pFactory = (ModularRingFactory<MOD>)polynomialRing.CoFac;
        ModularRingFactory<MOD> qFactory = pFactory;
        BigInteger currentModulus = qFactory.GetIntegerModul();
        BigInteger targetBound = M.Multiply(M.FromInteger(2));
        BigInteger accumulatedModulus = currentModulus;
        GenPolynomialRing<MOD> qPolynomialRing = new(qFactory, polynomialRing);

        BigInteger leading = C.LeadingBaseCoefficient();
        C = C.Multiply(leading);

        MOD leadingA = A.LeadingBaseCoefficient();
        if (!leadingA.IsOne())
        {
            A = A.Divide(leadingA);
            S = S.Multiply(leadingA);
        }

        MOD leadingB = B.LeadingBaseCoefficient();
        if (!leadingB.IsOne())
        {
            B = B.Divide(leadingB);
            T = T.Multiply(leadingB);
        }

        MOD leadingCoefficient = pFactory.FromInteger(leading.Val);
        A = A.Multiply(leadingCoefficient);
        B = B.Multiply(leadingCoefficient);
        T = T.Divide(leadingCoefficient);
        S = S.Divide(leadingCoefficient);

        GenPolynomial<BigInteger> Ai = PolyUtil.IntegerFromModularCoefficients(integerRing, A);
        GenPolynomial<BigInteger> Bi = PolyUtil.IntegerFromModularCoefficients(integerRing, B);
        ExpVector exponentA = Ai.LeadingExpVector();
        ExpVector exponentB = Bi.LeadingExpVector();
        Ai.DoPutToMap(exponentA, leading);
        Bi.DoPutToMap(exponentB, leading);

        GenPolynomial<MOD> Sp = S;
        GenPolynomial<MOD> Tp = T;
        GenPolynomial<BigInteger> Si = PolyUtil.IntegerFromModularCoefficients(integerRing, S);
        GenPolynomial<BigInteger> Ti = PolyUtil.IntegerFromModularCoefficients(integerRing, T);
        GenPolynomial<MOD> Aq = PolyUtil.FromIntegerCoefficients(qPolynomialRing, Ai)
            ?? throw new InvalidOperationException("Failed to map Ai to modular coefficients.");
        GenPolynomial<MOD> Bq = PolyUtil.FromIntegerCoefficients(qPolynomialRing, Bi)
            ?? throw new InvalidOperationException("Failed to map Bi to modular coefficients.");

        GenPolynomial<MOD> A1p = A;
        GenPolynomial<MOD> B1p = B;

        while (accumulatedModulus.CompareTo(targetBound) < 0)
        {
            GenPolynomial<BigInteger> error = C.Subtract(Ai.Multiply(Bi));
            if (error.IsZero())
            {
                break;
            }

            error = error.Divide(currentModulus);
            GenPolynomial<MOD> modularError = PolyUtil.FromIntegerCoefficients(qPolynomialRing, error)
                ?? throw new InvalidOperationException("Failed to reduce error polynomial.");

            GenPolynomial<MOD> Ap = Sp.Multiply(modularError);
            GenPolynomial<MOD> Bp = Tp.Multiply(modularError);
            GenPolynomial<MOD>[] qr = Ap.QuotientRemainder(Bq);
            GenPolynomial<MOD> Qp = qr[0];
            GenPolynomial<MOD> Rp = qr[1];
            A1p = Rp;
            B1p = Bp.Sum(Aq.Multiply(Qp));

            GenPolynomial<BigInteger> Ea = PolyUtil.IntegerFromModularCoefficients(integerRing, A1p);
            GenPolynomial<BigInteger> Eb = PolyUtil.IntegerFromModularCoefficients(integerRing, B1p);
            GenPolynomial<BigInteger> EaScaled = Ea.Multiply(currentModulus);
            GenPolynomial<BigInteger> EbScaled = Eb.Multiply(currentModulus);
            Ai = Ai.Sum(EbScaled);
            Bi = Bi.Sum(EaScaled);
            if (Debug && Ai.Degree(0) + Bi.Degree(0) > C.Degree(0))
            {
                throw new InvalidOperationException("deg(A) + deg(B) > deg(C)");
            }

            error = integerRing.FromInteger(1)
                .Subtract(Si.Multiply(Ai))
                .Subtract(Ti.Multiply(Bi));
            error = error.Divide(currentModulus);
            modularError = PolyUtil.FromIntegerCoefficients(qPolynomialRing, error)
                ?? throw new InvalidOperationException("Failed to reduce auxiliary error.");

            Ap = Sp.Multiply(modularError);
            Bp = Tp.Multiply(modularError);
            qr = Bp.QuotientRemainder(Aq);
            Qp = qr[0];
            Rp = qr[1];
            B1p = Rp;
            A1p = Ap.Sum(Bq.Multiply(Qp));

            Ea = PolyUtil.IntegerFromModularCoefficients(integerRing, A1p);
            Eb = PolyUtil.IntegerFromModularCoefficients(integerRing, B1p);
            EaScaled = Ea.Multiply(currentModulus);
            EbScaled = Eb.Multiply(currentModulus);
            Si = Si.Sum(EaScaled);
            Ti = Ti.Sum(EbScaled);
            Sp = PolyUtil.FromIntegerCoefficients(qPolynomialRing, Si)
                ?? throw new InvalidOperationException("Failed to reduce S approximation.");
            Tp = PolyUtil.FromIntegerCoefficients(qPolynomialRing, Ti)
                ?? throw new InvalidOperationException("Failed to reduce T approximation.");

            accumulatedModulus = currentModulus;
            currentModulus = qFactory.GetIntegerModul().Multiply(qFactory.GetIntegerModul());
            qFactory = ModLongRing.MAX_LONG.CompareTo(currentModulus.Val) > 0
                ? (ModularRingFactory<MOD>)(object)new ModLongRing(currentModulus.Val)
                : (ModularRingFactory<MOD>)(object)new ModIntegerRing(currentModulus);
            qPolynomialRing = new GenPolynomialRing<MOD>(qFactory, polynomialRing);
            Aq = PolyUtil.FromIntegerCoefficients(qPolynomialRing, Ai)
                ?? throw new InvalidOperationException("Failed to map Ai to new modulus.");
            Bq = PolyUtil.FromIntegerCoefficients(qPolynomialRing, Bi)
                ?? throw new InvalidOperationException("Failed to map Bi to new modulus.");
            Sp = PolyUtil.FromIntegerCoefficients(qPolynomialRing, Si)
                ?? throw new InvalidOperationException("Failed to reduce S approximation.");
            Tp = PolyUtil.FromIntegerCoefficients(qPolynomialRing, Ti)
                ?? throw new InvalidOperationException("Failed to reduce T approximation.");
        }

        GreatestCommonDivisorAbstract<BigInteger> gcdEngine = new GreatestCommonDivisorPrimitive<BigInteger>();
        BigInteger contentA = gcdEngine.BaseContent(Ai);
        Ai = Ai.Divide(contentA);

        try
        {
            BigInteger contentB = leading.Divide(contentA);
            Bi = Bi.Divide(contentB);
        }
        catch (Exception exception)
        {
            throw new NoLiftingException("no exact lifting possible", exception);
        }

        return new HenselApprox<MOD>(Ai, Bi, A1p, B1p);
    }

    /// <summary>
    /// Quadratic Hensel lifting that derives Bézout coefficients internally before refining the factors.
    /// </summary>
    /// <typeparam name="MOD">Modular coefficient type.</typeparam>
    /// <param name="C">Integral polynomial being factored.</param>
    /// <param name="M">Coefficient bound for lifted factors.</param>
    /// <param name="A">First modular factor.</param>
    /// <param name="B">Second modular factor.</param>
    /// <returns>Hensel approximation with lifted factors.</returns>
    /// <remarks>Original Java method: HenselUtil#liftHenselQuadratic(C,M,A,B).</remarks>
    public static HenselApprox<MOD> LiftHenselQuadratic<MOD>(
        GenPolynomial<BigInteger> C,
        BigInteger M,
        GenPolynomial<MOD> A,
        GenPolynomial<MOD> B)
        where MOD : GcdRingElem<MOD>, Modular
    {
        ArgumentNullException.ThrowIfNull(C);
        ArgumentNullException.ThrowIfNull(A);
        ArgumentNullException.ThrowIfNull(B);

        if (C.IsZero())
        {
            return new HenselApprox<MOD>(C, C, A, B);
        }

        if (A.IsZero() || B.IsZero())
        {
            throw new ArgumentException("A and B must be nonzero.");
        }

        GenPolynomialRing<BigInteger> integerRing = C.Ring;
        if (integerRing.Nvar != 1)
        {
            throw new ArgumentException("polynomial ring must be univariate");
        }

        try
        {
            GenPolynomial<MOD>[] gcdResult = A.Egcd(B);
            if (!gcdResult[0].IsOne())
            {
                throw new NoLiftingException($"A and B not coprime, gcd = {gcdResult[0]}, A = {A}, B = {B}");
            }

            GenPolynomial<MOD> s = gcdResult[1];
            GenPolynomial<MOD> t = gcdResult[2];
            return LiftHenselQuadratic(C, M, A, B, s, t);
        }
        catch (ArithmeticException exception)
        {
            throw new NoLiftingException($"coefficient error {exception.Message}", exception);
        }
    }
}
