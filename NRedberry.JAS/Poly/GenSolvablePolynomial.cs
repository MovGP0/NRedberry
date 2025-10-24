using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// GenSolvablePolynomial generic solvable polynomials implementing RingElem.
/// n-variate ordered solvable polynomials over C. Objects of this class are intended to be immutable.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.GenSolvablePolynomial
/// </remarks>
public class GenSolvablePolynomial<C> : GenPolynomial<C> where C : RingElem<C>
{
    public readonly GenSolvablePolynomialRing<C> Ring;

    public GenSolvablePolynomial(GenSolvablePolynomialRing<C> r)
    {
        throw new NotImplementedException();
    }

    public GenSolvablePolynomial(GenSolvablePolynomialRing<C> r, object c)
    {
        throw new NotImplementedException();
    }

    public new GenSolvablePolynomialRing<C> Factory() { throw new NotImplementedException(); }
    public new GenSolvablePolynomial<C> Copy() { throw new NotImplementedException(); }
    public override bool Equals(object? B) { throw new NotImplementedException(); }
    public GenSolvablePolynomial<C> Multiply(GenSolvablePolynomial<C> Bp) { throw new NotImplementedException(); }
    public GenSolvablePolynomial<C> Multiply(GenSolvablePolynomial<C> S, GenSolvablePolynomial<C> T) { throw new NotImplementedException(); }
    public new GenSolvablePolynomial<C> Multiply(C b) { throw new NotImplementedException(); }
    public GenSolvablePolynomial<C> Multiply(C b, C c) { throw new NotImplementedException(); }
}
