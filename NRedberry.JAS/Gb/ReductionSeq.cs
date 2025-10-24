using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Gb;

/// <summary>
/// Polynomial Reduction sequential use algorithm. Implements normalform.
/// </summary>
/// <typeparam name="C">coefficient type (should be FieldElem)</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.gb.ReductionSeq
/// </remarks>
public class ReductionSeq<C> : ReductionAbstract<C> where C : RingElem<C>
{
    public ReductionSeq()
    {
    }

    /// <summary>
    /// Normalform.
    /// </summary>
    /// <param name="Pp">polynomial list</param>
    /// <param name="Ap">polynomial</param>
    /// <returns>nf(Ap) with respect to Pp.</returns>
    public override GenPolynomial<C> Normalform(List<GenPolynomial<C>> Pp, GenPolynomial<C> Ap)
    {
        throw new NotImplementedException();
    }
}
