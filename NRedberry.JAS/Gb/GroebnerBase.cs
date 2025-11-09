using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Vector;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Gb;

/// <summary>
/// Gröbner bases abstract class implementing common Gröbner-base routines and GB test methods.
/// </summary>
/// <typeparam name="C">Coefficient type that implements <see cref="RingElem{T}"/>.</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.gb.GroebnerBase
/// </remarks>
public class GroebnerBase<C> where C : RingElem<C>
{
    /// <summary>
    /// Reduction engine.
    /// </summary>
    public readonly Reduction<C> Red;

    /// <summary>
    /// Linear algebra engine.
    /// </summary>
    public readonly BasicLinAlg<GenPolynomial<C>> Blas;

    /// <summary>
    /// Constructor.
    /// </summary>
    public GroebnerBase()
    {
        Red = new ReductionSeq<C>();
        Blas = new BasicLinAlg<GenPolynomial<C>>();
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="red">Reduction engine</param>
    public GroebnerBase(Reduction<C> red)
    {
        Red = red;
        Blas = new BasicLinAlg<GenPolynomial<C>>();
    }

    /// <summary>
    /// Common zero test.
    /// </summary>
    /// <param name="F">polynomial list</param>
    /// <returns>-1, 0 or 1 if dimension(ideal(F)) = -1, 0 or >= 1.</returns>
    public int CommonZeroTest(List<GenPolynomial<C>?> F)
    {
        if (F is null || F.Count == 0)
        {
            return 1;
        }

        GenPolynomial<C>? first = null;
        foreach (GenPolynomial<C>? polynomial in F)
        {
            if (polynomial is not null)
            {
                first = polynomial;
                break;
            }
        }

        if (first is null)
        {
            return 1;
        }

        GenPolynomialRing<C>? polynomialRing = first.Ring;
        if (polynomialRing is null)
        {
            throw new InvalidOperationException("Polynomial ring must be provided.");
        }

        if (polynomialRing.Nvar <= 0)
        {
            return -1;
        }

        HashSet<int> dependentVariables = [];
        foreach (GenPolynomial<C>? polynomial in F)
        {
            if (polynomial?.IsZero() != false)
            {
                continue;
            }

            if (polynomial.IsConstant())
            {
                return -1;
            }

            ExpVector? leadingExponent = polynomial.LeadingExpVector();
            if (leadingExponent is null)
            {
                continue;
            }

            int[]? dependency = leadingExponent.DependencyOnVariables();
            if (dependency is null)
            {
                continue;
            }

            if (dependency.Length == 1)
            {
                dependentVariables.Add(dependency[0]);
            }
        }

        if (polynomialRing.Nvar == dependentVariables.Count)
        {
            return 0;
        }

        return 1;
    }
}
