using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Vector;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Gb;

/// <summary>
/// Groebner Bases abstract class. Implements common Groebner bases and GB test methods.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
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
    public int CommonZeroTest(List<GenPolynomial<C>> F)
    {
        throw new NotImplementedException();
    }
}
