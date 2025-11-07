using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Squarefree decomposition for infinite fields of characteristic p.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.SquarefreeInfiniteFieldCharP
/// </remarks>
public class SquarefreeInfiniteFieldCharP<C> : SquarefreeFieldCharP<Quotient<C>>
    where C : GcdRingElem<C>
{
    protected readonly Quotient<C> qone;
    protected readonly Quotient<C> qzero;
    protected readonly SquarefreeAbstract<C> qengine;
    protected readonly QuotientRing<C> quotientRing;

    public SquarefreeInfiniteFieldCharP(RingFactory<Quotient<C>> fac)
        : base(fac)
    {
        if (fac is not QuotientRing<C> quotient)
        {
            throw new ArgumentException("fac must be a QuotientRing instance.", nameof(fac));
        }

        if (fac.IsFinite())
        {
            throw new ArgumentException("fac must represent an infinite field.", nameof(fac));
        }

        quotientRing = quotient;
        qengine = SquarefreeFactory.GetImplementation(quotientRing.Ring);

        GenPolynomial<C> one = quotientRing.Ring.FromInteger(1);
        GenPolynomial<C> zero = quotientRing.Ring.FromInteger(0);
        qone = new Quotient<C>(quotientRing, one);
        qzero = new Quotient<C>(quotientRing, zero);
    }

    public override GenPolynomial<Quotient<C>>? BaseSquarefreePRoot(GenPolynomial<Quotient<C>> polynomial)
    {
        throw new NotImplementedException();
    }

    public override GenPolynomial<GenPolynomial<Quotient<C>>>? RecursiveUnivariateRootCharacteristic(
        GenPolynomial<GenPolynomial<Quotient<C>>> polynomial)
    {
        throw new NotImplementedException();
    }
}
