using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// GenSolvablePolynomialRing generic solvable polynomial factory implementing RingFactory.
/// Factory for n-variate ordered solvable polynomials over C.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.GenSolvablePolynomialRing
/// </remarks>
public class GenSolvablePolynomialRing<C> : GenPolynomialRing<C> where C : RingElem<C>
{
    public readonly RelationTable<C> Table;
    public readonly new GenSolvablePolynomial<C> ZERO;
    public readonly new GenSolvablePolynomial<C> ONE;

    public GenSolvablePolynomialRing(RingFactory<C> cf, int n, TermOrder t, string[] v)
        : base(cf, n, t, v)
    {
        throw new NotImplementedException();
    }

    public GenSolvablePolynomialRing(RingFactory<C> cf, int n, TermOrder t, string[] v, RelationTable<C> rt)
        : base(cf, n, t, v)
    {
        throw new NotImplementedException();
    }

    public override string ToString() { throw new NotImplementedException(); }
    public override bool Equals(object? other) { throw new NotImplementedException(); }
    public override int GetHashCode() { throw new NotImplementedException(); }
    public new GenSolvablePolynomial<C> GetZERO() { throw new NotImplementedException(); }
    public new GenSolvablePolynomial<C> GetONE() { throw new NotImplementedException(); }
}
