using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

public static partial class HenselUtil
{
    public static GenPolynomial<MOD>[] LiftExtendedEuclidean<MOD>(
        GenPolynomial<MOD> A,
        GenPolynomial<MOD> B,
        long k)
        where MOD : GcdRingElem<MOD>, Modular
    {
        ArgumentNullException.ThrowIfNull(A);
        ArgumentNullException.ThrowIfNull(B);
        if (A.IsZero() || B.IsZero())
        {
            throw new ArgumentException($"A and B must be nonzero, A = {A}, B = {B}");
        }

        GenPolynomialRing<MOD> polynomialRing = A.Ring;
        if (polynomialRing.Nvar != 1)
        {
            throw new ArgumentException("polynomial ring must be univariate");
        }

        GenPolynomial<MOD>[] gcdResult;
        try
        {
            gcdResult = A.Egcd(B);
            if (!gcdResult[0].IsOne())
            {
                throw new NoLiftingException($"A and B not coprime, gcd = {gcdResult[0]}, A = {A}, B = {B}");
            }
        }
        catch (ArithmeticException exception)
        {
            throw new NoLiftingException($"coefficient error {exception.Message}", exception);
        }

        GenPolynomial<MOD> S = gcdResult[1];
        GenPolynomial<MOD> T = gcdResult[2];
        GenPolynomialRing<BigInteger> integerRing = CreateIntegerPolynomialRing(polynomialRing);
        GenPolynomial<BigInteger> one = integerRing.FromInteger(1);
        GenPolynomial<BigInteger> Ai = PolyUtil.IntegerFromModularCoefficients(integerRing, A);
        GenPolynomial<BigInteger> Bi = PolyUtil.IntegerFromModularCoefficients(integerRing, B);
        GenPolynomial<BigInteger> Si = PolyUtil.IntegerFromModularCoefficients(integerRing, S);
        GenPolynomial<BigInteger> Ti = PolyUtil.IntegerFromModularCoefficients(integerRing, T);

        ModularRingFactory<MOD> modularRing = (ModularRingFactory<MOD>)polynomialRing.CoFac;
        BigInteger baseModulus = modularRing.GetIntegerModul();
        BigInteger modulus = baseModulus;

        for (int i = 1; i < k; i++)
        {
            GenPolynomial<BigInteger> error = one
                .Subtract(Si.Multiply(Ai))
                .Subtract(Ti.Multiply(Bi));
            if (error.IsZero())
            {
                break;
            }

            error = error.Divide(modulus);
            GenPolynomial<MOD> correction = PolyUtil.FromIntegerCoefficients(polynomialRing, error)
                ?? throw new InvalidOperationException("Failed to reduce correction polynomial.");

            GenPolynomial<MOD> s = S.Multiply(correction);
            GenPolynomial<MOD> t = T.Multiply(correction);
            GenPolynomial<MOD>[] qr = s.QuotientRemainder(B);
            GenPolynomial<MOD> q = qr[0];
            s = qr[1];
            t = t.Sum(q.Multiply(A));

            GenPolynomial<BigInteger> si = PolyUtil.IntegerFromModularCoefficients(integerRing, s);
            GenPolynomial<BigInteger> ti = PolyUtil.IntegerFromModularCoefficients(integerRing, t);
            Si = Si.Sum(si.Multiply(modulus));
            Ti = Ti.Sum(ti.Multiply(modulus));
            modulus = modulus.Multiply(baseModulus);
        }

        ModularRingFactory<MOD> liftedRing = ModLongRing.MAX_LONG.CompareTo(modulus.Val) > 0
            ? (ModularRingFactory<MOD>)(object)new ModLongRing(modulus.Val)
            : (ModularRingFactory<MOD>)(object)new ModIntegerRing(modulus.Val);
        GenPolynomialRing<MOD> liftedPolynomialRing = new(liftedRing, polynomialRing);
        S = PolyUtil.FromIntegerCoefficients(liftedPolynomialRing, Si)
            ?? throw new InvalidOperationException("Failed to map lifted S.");
        T = PolyUtil.FromIntegerCoefficients(liftedPolynomialRing, Ti)
            ?? throw new InvalidOperationException("Failed to map lifted T.");
        return new[] { S, T };
    }

    public static List<GenPolynomial<MOD>> LiftExtendedEuclidean<MOD>(
        List<GenPolynomial<MOD>> A,
        long k)
        where MOD : GcdRingElem<MOD>, Modular
    {
        ArgumentNullException.ThrowIfNull(A);
        if (A.Count == 0)
        {
            throw new ArgumentException("Polynomial list must not be empty.", nameof(A));
        }

        GenPolynomialRing<MOD> polynomialRing = A[0].Ring;
        if (polynomialRing.Nvar != 1)
        {
            throw new ArgumentException("polynomial ring must be univariate");
        }

        GenPolynomial<MOD> zero = new(polynomialRing);
        int r = A.Count;
        List<GenPolynomial<MOD>> Q = new(r);
        for (int i = 0; i < r; i++)
        {
            Q.Add(new GenPolynomial<MOD>(polynomialRing));
        }

        Q[r - 2] = A[r - 1];
        for (int j = r - 3; j >= 0; j--)
        {
            Q[j] = A[j + 1].Multiply(Q[j + 1]);
        }

        List<GenPolynomial<MOD>> B = new(r + 1);
        List<GenPolynomial<MOD>> lift = new(r);
        for (int j = 0; j < r; j++)
        {
            B.Add(new GenPolynomial<MOD>(polynomialRing));
            lift.Add(new GenPolynomial<MOD>(polynomialRing));
        }

        GenPolynomial<MOD> one = polynomialRing.FromInteger(1);
        GenPolynomialRing<BigInteger> integerRing = CreateIntegerPolynomialRing(polynomialRing);
        B.Insert(0, one);
        GenPolynomial<MOD> current = one;
        for (int j = 0; j < r - 1; j++)
        {
            List<GenPolynomial<MOD>> solution = LiftDiophant(Q[j], A[j], B[j], k);
            current = solution[0];
            GenPolynomial<BigInteger> liftedInteger = PolyUtil.IntegerFromModularCoefficients(integerRing, current);
            GenPolynomial<MOD> normalized = PolyUtil.FromIntegerCoefficients(polynomialRing, liftedInteger)
                ?? throw new InvalidOperationException("Failed to normalize lifted coefficient.");
            B[j + 1] = normalized;
            lift[j] = solution[1];
        }

        lift[r - 1] = current;
        return lift;
    }
}
