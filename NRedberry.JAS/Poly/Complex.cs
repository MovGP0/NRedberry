using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// Generic Complex class implementing the RingElem interface. Objects of this class are immutable.
/// </summary>
/// <typeparam name="C">base type of RingElem (for complex polynomials)</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.Complex
/// </remarks>
public class Complex<C> : GcdRingElem<Complex<C>> where C : RingElem<C>
{
    public readonly ComplexRing<C> Ring;
    protected readonly C re;
    protected readonly C im;

    public Complex(ComplexRing<C> ring, C r, C i)
    {
        Ring = ring;
        re = r;
        im = i;
    }

    public Complex(ComplexRing<C> ring, C r) : this(ring, r, ring.Ring.GetZERO()) { }

    public Complex(ComplexRing<C> ring, long r) : this(ring, ring.Ring.FromInteger(r)) { }

    public Complex(ComplexRing<C> ring) : this(ring, ring.Ring.GetZERO()) { }

    public ComplexRing<C> Factory() => Ring;
    public C GetRe() => re;
    public C GetIm() => im;
    public Complex<C> Copy() { throw new NotImplementedException(); }
    public override string ToString() { throw new NotImplementedException(); }
    public bool IsZERO() { throw new NotImplementedException(); }
    public bool IsONE() { throw new NotImplementedException(); }
    public bool IsUnit() { throw new NotImplementedException(); }
    public int CompareTo(Complex<C>? other) { throw new NotImplementedException(); }
    public override bool Equals(object? obj) { throw new NotImplementedException(); }
    public override int GetHashCode() { throw new NotImplementedException(); }
    public Complex<C> Abs() { throw new NotImplementedException(); }
    public Complex<C> Negate() { throw new NotImplementedException(); }
    public int Signum() { throw new NotImplementedException(); }
    public Complex<C> Subtract(Complex<C> S) { throw new NotImplementedException(); }
    public Complex<C> Divide(Complex<C> S) { throw new NotImplementedException(); }
    public Complex<C> Inverse() { throw new NotImplementedException(); }
    public Complex<C> Remainder(Complex<C> S) { throw new NotImplementedException(); }
    public Complex<C> Gcd(Complex<C> b) { throw new NotImplementedException(); }
    public Complex<C>[] Egcd(Complex<C> b) { throw new NotImplementedException(); }
    public Complex<C> Multiply(Complex<C> S) { throw new NotImplementedException(); }
    public Complex<C> Sum(Complex<C> S) { throw new NotImplementedException(); }
    
    ElemFactory<Complex<C>> Element<Complex<C>>.Factory() => Factory();
}
