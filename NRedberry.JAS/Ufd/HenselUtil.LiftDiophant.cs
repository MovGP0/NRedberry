using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

public static partial class HenselUtil
{
    /// <summary>
    /// Lifts a modular Diophantine equation <c>A*S1 + B*S2 = C</c> to precision <c>p^k</c>.
    /// </summary>
    /// <typeparam name="MOD">Modular coefficient type.</typeparam>
    /// <param name="A">First modular polynomial.</param>
    /// <param name="B">Second modular polynomial.</param>
    /// <param name="C">Target polynomial.</param>
    /// <param name="k">Desired lifting exponent.</param>
    /// <returns>List containing the lifted coefficients [S1, S2].</returns>
    /// <remarks>Original Java method: HenselUtil#liftDiophant(A,B,C,k).</remarks>
    public static List<GenPolynomial<MOD>> LiftDiophant<MOD>(
        GenPolynomial<MOD> A,
        GenPolynomial<MOD> B,
        GenPolynomial<MOD> C,
        long k)
        where MOD : GcdRingElem<MOD>, Modular
    {
        ArgumentNullException.ThrowIfNull(A);
        ArgumentNullException.ThrowIfNull(B);
        ArgumentNullException.ThrowIfNull(C);
        if (A.IsZero() || B.IsZero())
        {
            throw new ArgumentException($"A and B must be nonzero, A = {A}, B = {B}");
        }

        GenPolynomialRing<MOD> polynomialRing = C.Ring;
        if (polynomialRing.Nvar != 1)
        {
            throw new ArgumentException("polynomial ring must be univariate");
        }

        List<GenPolynomial<MOD>> solution = new(2)
        {
            new GenPolynomial<MOD>(polynomialRing),
            new GenPolynomial<MOD>(polynomialRing)
        };

        GenPolynomialRing<BigInteger> integerRing = CreateIntegerPolynomialRing(polynomialRing);
        foreach (Monomial<MOD> term in C)
        {
            long exponent = term.E.GetVal(0);
            List<GenPolynomial<MOD>> partial = LiftDiophant(A, B, exponent, k);
            MOD coefficient = polynomialRing.CoFac.FromInteger(term.C.GetSymmetricInteger().Val);
            for (int i = 0; i < partial.Count; i++)
            {
                GenPolynomial<BigInteger> lifted = PolyUtil.IntegerFromModularCoefficients(integerRing, partial[i]);
                GenPolynomial<MOD> normalized = PolyUtil.FromIntegerCoefficients(polynomialRing, lifted)
                    ?? throw new InvalidOperationException("Failed to normalize lifted polynomial.");
                normalized = normalized.Multiply(coefficient);
                solution[i] = solution[i].Sum(normalized);
            }
        }

        GenPolynomial<MOD> normalizedA = PolyUtil.FromIntegerCoefficients(
            polynomialRing,
            PolyUtil.IntegerFromModularCoefficients(integerRing, A))
            ?? throw new InvalidOperationException("Failed to normalize A.");
        GenPolynomial<MOD> normalizedB = PolyUtil.FromIntegerCoefficients(
            polynomialRing,
            PolyUtil.IntegerFromModularCoefficients(integerRing, B))
            ?? throw new InvalidOperationException("Failed to normalize B.");
        GenPolynomial<MOD> normalizedC = PolyUtil.FromIntegerCoefficients(
            polynomialRing,
            PolyUtil.IntegerFromModularCoefficients(integerRing, C))
            ?? throw new InvalidOperationException("Failed to normalize C.");
        GenPolynomial<MOD> check = normalizedB.Multiply(solution[0]).Sum(normalizedA.Multiply(solution[1]));
        if (Debug && !check.Equals(normalizedC))
        {
            throw new InvalidOperationException("Diophant lift verification failed.");
        }

        return solution;
    }

    /// <summary>
    /// Lifts a system of Diophantine equations <c>∑ s_i * Π_{j≠i} A_j = C</c> to precision <c>p^k</c>.
    /// </summary>
    /// <typeparam name="MOD">Modular coefficient type.</typeparam>
    /// <param name="A">List of modular polynomials.</param>
    /// <param name="C">Target polynomial.</param>
    /// <param name="k">Desired lifting exponent.</param>
    /// <returns>Coefficients <c>[s₁,…,sₙ]</c> that satisfy the lifted equation.</returns>
    /// <remarks>Original Java method: HenselUtil#liftDiophant(List,C,k).</remarks>
    public static List<GenPolynomial<MOD>> LiftDiophant<MOD>(
        List<GenPolynomial<MOD>> A,
        GenPolynomial<MOD> C,
        long k)
        where MOD : GcdRingElem<MOD>, Modular
    {
        ArgumentNullException.ThrowIfNull(A);
        ArgumentNullException.ThrowIfNull(C);
        if (A.Count == 0)
        {
            throw new ArgumentException("Polynomial list must not be empty.", nameof(A));
        }

        GenPolynomialRing<MOD> polynomialRing = C.Ring;
        if (polynomialRing.Nvar != 1)
        {
            throw new ArgumentException("polynomial ring must be univariate");
        }

        List<GenPolynomial<MOD>> solution = new(A.Count);
        for (int i = 0; i < A.Count; i++)
        {
            solution.Add(new GenPolynomial<MOD>(polynomialRing));
        }

        GenPolynomialRing<BigInteger> integerRing = CreateIntegerPolynomialRing(polynomialRing);
        foreach (Monomial<MOD> term in C)
        {
            long exponent = term.E.GetVal(0);
            List<GenPolynomial<MOD>> partial = LiftDiophant(A, exponent, k);
            MOD coefficient = polynomialRing.CoFac.FromInteger(term.C.GetSymmetricInteger().Val);
            for (int i = 0; i < partial.Count; i++)
            {
                GenPolynomial<BigInteger> lifted = PolyUtil.IntegerFromModularCoefficients(integerRing, partial[i]);
                GenPolynomial<MOD> normalized = PolyUtil.FromIntegerCoefficients(polynomialRing, lifted)
                    ?? throw new InvalidOperationException("Failed to normalize lifted polynomial.");
                normalized = normalized.Multiply(coefficient);
                solution[i] = solution[i].Sum(normalized);
            }
        }

        return solution;
    }

    /// <summary>
    /// Lifts Bézout coefficients so that <c>A*s1 + B*s2 = x^exponent</c> modulo <c>p^k</c>.
    /// </summary>
    /// <typeparam name="MOD">Modular coefficient type.</typeparam>
    /// <param name="A">First modular polynomial.</param>
    /// <param name="B">Second modular polynomial.</param>
    /// <param name="exponent">Exponent of the monomial target.</param>
    /// <param name="k">Desired lifting exponent.</param>
    /// <returns>Coefficients [s1, s2] satisfying the lifted relation.</returns>
    /// <remarks>Original Java method: HenselUtil#liftDiophant(A,B,e,k).</remarks>
    public static List<GenPolynomial<MOD>> LiftDiophant<MOD>(
        GenPolynomial<MOD> A,
        GenPolynomial<MOD> B,
        long exponent,
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

        GenPolynomial<MOD>[] extended = LiftExtendedEuclidean(B, A, k);
        GenPolynomial<MOD> s1 = extended[0];
        GenPolynomial<MOD> s2 = extended[1];
        if (exponent == 0)
        {
            return new List<GenPolynomial<MOD>> { s1, s2 };
        }

        GenPolynomialRing<MOD> liftedRing = s1.Ring;
        GenPolynomialRing<BigInteger> integerRing = CreateIntegerPolynomialRing(liftedRing);
        A = PolyUtil.FromIntegerCoefficients(liftedRing, PolyUtil.IntegerFromModularCoefficients(integerRing, A))
            ?? throw new InvalidOperationException("Failed to normalize A.");
        B = PolyUtil.FromIntegerCoefficients(liftedRing, PolyUtil.IntegerFromModularCoefficients(integerRing, B))
            ?? throw new InvalidOperationException("Failed to normalize B.");

        GenPolynomial<MOD> xe = liftedRing.Univariate(0, exponent);
        GenPolynomial<MOD> quotientCandidate = s1.Multiply(xe);
        GenPolynomial<MOD>[] qr = quotientCandidate.QuotientRemainder(A);
        GenPolynomial<MOD> quotient = qr[0];
        GenPolynomial<MOD> r1 = qr[1];
        GenPolynomial<MOD> r2 = s2.Multiply(xe).Sum(quotient.Multiply(B));
        GenPolynomial<MOD> check = B.Multiply(r1).Sum(A.Multiply(r2));
        if (Debug && !check.Equals(xe))
        {
            throw new InvalidOperationException("Diophant lift verification failed.");
        }

        return new List<GenPolynomial<MOD>> { r1, r2 };
    }

    /// <summary>
    /// Lifts a family of modular polynomials so that <c>∑ s_i * Π_{j≠i} A_j = x^exponent</c> modulo <c>p^k</c>.
    /// </summary>
    /// <typeparam name="MOD">Modular coefficient type.</typeparam>
    /// <param name="A">List of modular polynomials.</param>
    /// <param name="exponent">Exponent of the monomial target.</param>
    /// <param name="k">Desired lifting exponent.</param>
    /// <returns>Lifted coefficient list.</returns>
    /// <remarks>Original Java method: HenselUtil#liftDiophant(List,e,k).</remarks>
    public static List<GenPolynomial<MOD>> LiftDiophant<MOD>(
        List<GenPolynomial<MOD>> A,
        long exponent,
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

        List<GenPolynomial<MOD>> extended = LiftExtendedEuclidean(A, k);
        if (exponent == 0)
        {
            return extended;
        }

        GenPolynomialRing<MOD> liftedRing = extended[0].Ring;
        GenPolynomialRing<BigInteger> integerRing = CreateIntegerPolynomialRing(liftedRing);
        List<GenPolynomial<MOD>> normalized = new(extended.Count);
        foreach (GenPolynomial<MOD> polynomial in extended)
        {
            GenPolynomial<BigInteger> lifted = PolyUtil.IntegerFromModularCoefficients(integerRing, polynomial);
            normalized.Add(PolyUtil.FromIntegerCoefficients(liftedRing, lifted)
                ?? throw new InvalidOperationException("Failed to normalize lifted polynomial."));
        }

        GenPolynomial<MOD> xe = liftedRing.Univariate(0, exponent);
        List<GenPolynomial<MOD>> solution = new(normalized.Count);
        for (int i = 0; i < normalized.Count; i++)
        {
            GenPolynomial<MOD> quotient = normalized[i].Multiply(xe);
            GenPolynomial<MOD> remainder = quotient.Remainder(A[i]);
            solution.Add(remainder);
        }

        return solution;
    }
}
