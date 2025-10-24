using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ps;

/// <summary>
/// Polynomial functions capable for Taylor series expansion.
/// </summary>
/// <typeparam name="C">ring element type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ps.PolynomialTaylorFunction
/// </remarks>
public class PolynomialTaylorFunction<C> : TaylorFunction<C> where C : RingElem<C>
{
    private readonly GenPolynomial<C> pol;

    public PolynomialTaylorFunction(GenPolynomial<C> p)
    {
        pol = p;
    }

    public override string ToString()
    {
        throw new NotImplementedException();
    }

    public bool IsZERO()
    {
        throw new NotImplementedException();
    }

    public TaylorFunction<C> Deriviative()
    {
        throw new NotImplementedException();
    }

    public C Evaluate(C a)
    {
        throw new NotImplementedException();
    }
}
