using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

public static partial class HenselUtil
{
    public static bool IsDiophantLift<MOD>(
        GenPolynomial<MOD> A,
        GenPolynomial<MOD> B,
        GenPolynomial<MOD> S1,
        GenPolynomial<MOD> S2,
        GenPolynomial<MOD> C)
        where MOD : GcdRingElem<MOD>, Modular
    {
        ArgumentNullException.ThrowIfNull(A);
        ArgumentNullException.ThrowIfNull(B);
        ArgumentNullException.ThrowIfNull(S1);
        ArgumentNullException.ThrowIfNull(S2);
        ArgumentNullException.ThrowIfNull(C);

        GenPolynomialRing<MOD> polynomialRing = C.Ring;
        GenPolynomialRing<BigInteger> integerRing = CreateIntegerPolynomialRing(polynomialRing);
        GenPolynomial<MOD> normalizedA = PolyUtil.FromIntegerCoefficients(
            polynomialRing,
            PolyUtil.IntegerFromModularCoefficients(integerRing, A))
            ?? throw new InvalidOperationException("Failed to normalize A.");
        GenPolynomial<MOD> normalizedB = PolyUtil.FromIntegerCoefficients(
            polynomialRing,
            PolyUtil.IntegerFromModularCoefficients(integerRing, B))
            ?? throw new InvalidOperationException("Failed to normalize B.");
        GenPolynomial<MOD> normalizedS1 = PolyUtil.FromIntegerCoefficients(
            polynomialRing,
            PolyUtil.IntegerFromModularCoefficients(integerRing, S1))
            ?? throw new InvalidOperationException("Failed to normalize S1.");
        GenPolynomial<MOD> normalizedS2 = PolyUtil.FromIntegerCoefficients(
            polynomialRing,
            PolyUtil.IntegerFromModularCoefficients(integerRing, S2))
            ?? throw new InvalidOperationException("Failed to normalize S2.");

        GenPolynomial<MOD> combination = normalizedA.Multiply(normalizedS1)
            .Sum(normalizedB.Multiply(normalizedS2));
        return combination.Equals(C);
    }

    public static bool IsDiophantLift<MOD>(
        List<GenPolynomial<MOD>> A,
        List<GenPolynomial<MOD>> S,
        GenPolynomial<MOD> C)
        where MOD : GcdRingElem<MOD>, Modular
    {
        ArgumentNullException.ThrowIfNull(A);
        ArgumentNullException.ThrowIfNull(S);
        ArgumentNullException.ThrowIfNull(C);
        if (A.Count != S.Count)
        {
            throw new ArgumentException("Polynomial and coefficient lists must have the same length.");
        }

        GenPolynomialRing<MOD> polynomialRing = A[0].Ring;
        GenPolynomialRing<BigInteger> integerRing = CreateIntegerPolynomialRing(polynomialRing);
        List<GenPolynomial<MOD>> coproducts = new(A.Count);
        for (int i = 0; i < A.Count; i++)
        {
            GenPolynomial<MOD> product = polynomialRing.FromInteger(1);
            for (int j = 0; j < A.Count; j++)
            {
                if (i == j)
                {
                    continue;
                }

                product = product.Multiply(A[j]);
            }

            GenPolynomial<MOD> normalized = PolyUtil.FromIntegerCoefficients(
                polynomialRing,
                PolyUtil.IntegerFromModularCoefficients(integerRing, product))
                ?? throw new InvalidOperationException("Failed to normalize coproduct.");
            coproducts.Add(normalized);
        }

        GenPolynomial<MOD> sum = new(polynomialRing);
        for (int i = 0; i < coproducts.Count; i++)
        {
            GenPolynomial<MOD> coefficient = PolyUtil.FromIntegerCoefficients(
                polynomialRing,
                PolyUtil.IntegerFromModularCoefficients(integerRing, S[i]))
                ?? throw new InvalidOperationException("Failed to normalize Diophant coefficient.");
            sum = sum.Sum(coproducts[i].Multiply(coefficient));
        }

        return sum.Equals(C);
    }
}
