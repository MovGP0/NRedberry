using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Quotient ring element, basically a rational function.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.Quotient
/// </remarks>
public class Quotient<C> : GcdRingElem<Quotient<C>> where C : GcdRingElem<C>
{
    public readonly QuotientRing<C> Ring;
    public readonly GenPolynomial<C> Num;
    public readonly GenPolynomial<C> Den;

    public Quotient(QuotientRing<C> r, GenPolynomial<C> n)
    {
        Ring = r;
        Num = n;
        Den = r.Ring.GetONE();
    }

    public Quotient(QuotientRing<C> r, GenPolynomial<C> n, GenPolynomial<C> d)
    {
        Ring = r;
        Num = n;
        Den = d;
    }

    public ElemFactory<Quotient<C>> Factory()
    {
        return Ring;
    }

    public Quotient<C> Copy()
    {
        throw new NotImplementedException();
    }

    public bool IsZERO()
    {
        throw new NotImplementedException();
    }

    public bool IsONE()
    {
        throw new NotImplementedException();
    }

    public bool IsUnit()
    {
        throw new NotImplementedException();
    }

    public bool IsConstant()
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        throw new NotImplementedException();
    }

    public Quotient<C> Sum(Quotient<C> S)
    {
        throw new NotImplementedException();
    }

    public Quotient<C> Subtract(Quotient<C> S)
    {
        throw new NotImplementedException();
    }

    public Quotient<C> Negate()
    {
        throw new NotImplementedException();
    }

    public Quotient<C> Abs()
    {
        throw new NotImplementedException();
    }

    public Quotient<C> Multiply(Quotient<C> S)
    {
        throw new NotImplementedException();
    }

    public Quotient<C> Divide(Quotient<C> S)
    {
        throw new NotImplementedException();
    }

    public Quotient<C> Inverse()
    {
        throw new NotImplementedException();
    }

    public Quotient<C> Remainder(Quotient<C> S)
    {
        throw new NotImplementedException();
    }

    public Quotient<C> Gcd(Quotient<C> S)
    {
        throw new NotImplementedException();
    }

    public Quotient<C>[] Egcd(Quotient<C> S)
    {
        throw new NotImplementedException();
    }

    public int Signum()
    {
        throw new NotImplementedException();
    }

    public int CompareTo(Quotient<C>? S)
    {
        throw new NotImplementedException();
    }

    public override bool Equals(object? b)
    {
        throw new NotImplementedException();
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }
}
