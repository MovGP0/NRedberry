using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ps;

/// <summary>
/// Polynomial functions capable of Taylor series expansion and implementing the
/// {@link TaylorFunction} behavior from the Java source.
/// </summary>
/// <typeparam name="C">ring element type.</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ps.PolynomialTaylorFunction
/// </remarks>
public class PolynomialTaylorFunction<C>(GenPolynomial<C> p) : TaylorFunction<C>
    where C : RingElem<C>
{
    public override string ToString()
    {
        return p.ToString();
    }

    public bool IsZERO()
    {
        return p.IsZero();
    }

    public TaylorFunction<C> Deriviative()
    {
        return new PolynomialTaylorFunction<C>(PolyUtil.BaseDeriviative(p));
    }

    public C Evaluate(C a)
    {
        return PolyUtil.EvaluateMain(p.Ring.CoFac, p, a);
    }
}
