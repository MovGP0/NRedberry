using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Gb;

/// <summary>
/// Polynomial reduction sequential-use algorithm that implements the normalform.
/// </summary>
/// <typeparam name="C">Coefficient type that should represent a field element.</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.gb.ReductionSeq
/// </remarks>
public class ReductionSeq<C> : ReductionAbstract<C> where C : RingElem<C>
{
    /// <summary>
    /// Computes the normalform of <paramref name="Ap"/> with respect to the sequential list <paramref name="Pp"/>.
    /// Coefficients must come from a field.
    /// </summary>
    /// <param name="Pp">Polynomial list used as divisors.</param>
    /// <param name="Ap">Polynomial to reduce.</param>
    /// <returns>nf(Ap) with respect to <paramref name="Pp"/>.</returns>
    public override GenPolynomial<C> Normalform(List<GenPolynomial<C>> Pp, GenPolynomial<C> Ap)
    {
        if (Pp.Count == 0)
        {
            return Ap;
        }

        if (Ap.IsZero())
        {
            return Ap;
        }

        if (!Ap.Ring.CoFac.IsField())
        {
            throw new ArgumentException("coefficients not from a field");
        }

        int length = Pp.Count;
        GenPolynomial<C>[] polynomials = new GenPolynomial<C>[length];
        for (int index = 0; index < length; index++)
        {
            polynomials[index] = Pp[index];
        }

        ExpVector?[] leadingTerms = new ExpVector?[length];
        C?[] leadingCoefficients = new C?[length];
        GenPolynomial<C>[] filtered = new GenPolynomial<C>[length];
        int count = 0;
        for (int i = 0; i < length; i++)
        {
            GenPolynomial<C> polynomial = polynomials[i];
            Monomial<C>? monomial = polynomial.LeadingMonomial();
            if (monomial == null)
            {
                continue;
            }

            filtered[count] = polynomial;
            leadingTerms[count] = monomial.Exponent();
            leadingCoefficients[count] = monomial.Coefficient();
            count++;
        }

        if (count == 0)
        {
            return Ap;
        }

        GenPolynomial<C> remainder = new(Ap.Ring);
        GenPolynomial<C> current = Ap;

        while (current.Length() > 0)
        {
            Monomial<C>? leadMonomial = current.LeadingMonomial();
            if (leadMonomial == null)
            {
                break;
            }

            ExpVector expVector = leadMonomial.Exponent();
            C coefficient = leadMonomial.Coefficient();

            int reducerIndex = -1;
            for (int i = 0; i < count; i++)
            {
                ExpVector? divisorExp = leadingTerms[i];
                if (divisorExp != null && expVector.MultipleOf(divisorExp))
                {
                    reducerIndex = i;
                    break;
                }
            }

            if (reducerIndex < 0)
            {
                remainder = remainder.Sum(coefficient, expVector);
                current = current.Subtract(coefficient, expVector);
                continue;
            }

            ExpVector divisorVector = leadingTerms[reducerIndex]!;
            ExpVector diffVector = expVector.Subtract(divisorVector);

            C divisorCoeff = leadingCoefficients[reducerIndex]!;
            C quotientCoeff = coefficient.Divide(divisorCoeff);
            GenPolynomial<C> product = filtered[reducerIndex].Multiply(quotientCoeff, diffVector);
            current = current.Subtract(product);
        }

        return remainder;
    }
}
