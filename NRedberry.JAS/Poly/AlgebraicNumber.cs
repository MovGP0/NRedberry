using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// Algebraic number class based on GenPolynomial with RingElem interface. Objects of this class are immutable.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.AlgebraicNumber
/// </remarks>
public class AlgebraicNumber<C> : GcdRingElem<AlgebraicNumber<C>> where C : RingElem<C>
{
    public readonly AlgebraicNumberRing<C> Ring;
    public readonly GenPolynomial<C> Val;

    public AlgebraicNumber(AlgebraicNumberRing<C> r, GenPolynomial<C> a)
    {
        Ring = r;
        Val = a;
    }

    public AlgebraicNumber(AlgebraicNumberRing<C> r) : this(r, r.Ring.GetZERO()) { }

    public GenPolynomial<C> GetVal() => Val;
    public AlgebraicNumberRing<C> Factory() => Ring;
    public AlgebraicNumber<C> Copy() { throw new NotImplementedException(); }
    public bool IsZERO() { throw new NotImplementedException(); }
    public bool IsONE() { throw new NotImplementedException(); }
    public bool IsUnit() { throw new NotImplementedException(); }
    public int CompareTo(AlgebraicNumber<C>? other) { throw new NotImplementedException(); }
    public override bool Equals(object? obj) { throw new NotImplementedException(); }
    public override int GetHashCode() { throw new NotImplementedException(); }
    public AlgebraicNumber<C> Abs() { throw new NotImplementedException(); }
    public AlgebraicNumber<C> Negate() { throw new NotImplementedException(); }
    public int Signum() { throw new NotImplementedException(); }
    public AlgebraicNumber<C> Subtract(AlgebraicNumber<C> S) { throw new NotImplementedException(); }
    public AlgebraicNumber<C> Divide(AlgebraicNumber<C> S) { throw new NotImplementedException(); }
    public AlgebraicNumber<C> Inverse() { throw new NotImplementedException(); }
    public AlgebraicNumber<C> Remainder(AlgebraicNumber<C> S) { throw new NotImplementedException(); }
    public AlgebraicNumber<C> Gcd(AlgebraicNumber<C> b) { throw new NotImplementedException(); }
    public AlgebraicNumber<C>[] Egcd(AlgebraicNumber<C> b) { throw new NotImplementedException(); }
    public AlgebraicNumber<C> Multiply(AlgebraicNumber<C> S) { throw new NotImplementedException(); }
    public AlgebraicNumber<C> Sum(AlgebraicNumber<C> S) { throw new NotImplementedException(); }
    public override string ToString() { throw new NotImplementedException(); }

    ElemFactory<AlgebraicNumber<C>> Element<AlgebraicNumber<C>>.Factory() => Factory();
}
