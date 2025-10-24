using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// GenPolynomial generic polynomials implementing RingElem. n-variate ordered polynomials over C.
/// Objects of this class are intended to be immutable. The implementation is based on TreeMap respectively
/// SortedMap from exponents to coefficients. Only the coefficients are modeled with generic types, the
/// exponents are fixed to ExpVector with long entries.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.GenPolynomial
/// </remarks>
public class GenPolynomial<C> : RingElem<GenPolynomial<C>> where C : RingElem<C>
{
    // Will be fully implemented later
    public GenPolynomial<C> Copy() { throw new NotImplementedException(); }
    public bool IsZERO() { throw new NotImplementedException(); }
    public bool IsONE() { throw new NotImplementedException(); }
    public bool IsUnit() { throw new NotImplementedException(); }
    public int CompareTo(GenPolynomial<C>? other) { throw new NotImplementedException(); }
    public override bool Equals(object? obj) { throw new NotImplementedException(); }
    public override int GetHashCode() { throw new NotImplementedException(); }
    public GenPolynomial<C> Abs() { throw new NotImplementedException(); }
    public GenPolynomial<C> Negate() { throw new NotImplementedException(); }
    public int Signum() { throw new NotImplementedException(); }
    public GenPolynomial<C> Subtract(GenPolynomial<C> S) { throw new NotImplementedException(); }
    public GenPolynomial<C> Divide(GenPolynomial<C> S) { throw new NotImplementedException(); }
    public GenPolynomial<C> Inverse() { throw new NotImplementedException(); }
    public GenPolynomial<C> Remainder(GenPolynomial<C> S) { throw new NotImplementedException(); }
    public GenPolynomial<C> Gcd(GenPolynomial<C> b) { throw new NotImplementedException(); }
    public GenPolynomial<C>[] Egcd(GenPolynomial<C> b) { throw new NotImplementedException(); }
    public GenPolynomial<C> Multiply(GenPolynomial<C> S) { throw new NotImplementedException(); }
    public GenPolynomial<C> Sum(GenPolynomial<C> S) { throw new NotImplementedException(); }
    public ElemFactory<GenPolynomial<C>> Factory() { throw new NotImplementedException(); }
}
