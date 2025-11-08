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
        return pol.ToString();
    }

    public bool IsZERO()
    {
        return pol.IsZero();
    }

    public TaylorFunction<C> Deriviative()
    {
        return new PolynomialTaylorFunction<C>(PolyUtil.BaseDeriviative(pol));
    }

    public C Evaluate(C a)
    {
        return PolyUtil.EvaluateMain(pol.Ring.CoFac, pol, a);
    }
}
