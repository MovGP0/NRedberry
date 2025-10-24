using System.Collections;
using System.Numerics;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// GenPolynomialRing generic polynomial factory implementing RingFactory;
/// Factory for n-variate ordered polynomials over C. Almost immutable object, except variable names.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.GenPolynomialRing
/// </remarks>
public class GenPolynomialRing<C> : RingFactory<GenPolynomial<C>>, IEnumerable<GenPolynomial<C>> where C : RingElem<C>
{
    public readonly RingFactory<C> CoFac;
    public readonly int Nvar;
    public readonly TermOrder Tord;
    protected bool partial;
    protected string[] vars;
    private static readonly HashSet<string> knownVars = new();
    public readonly GenPolynomial<C> ZERO;
    public readonly GenPolynomial<C> ONE;
    public readonly ExpVector Evzero;
    protected static readonly Random random = new();
    protected int isField = -1;

    public GenPolynomialRing(RingFactory<C> cf, int n)
    {
        throw new NotImplementedException();
    }

    public GenPolynomialRing(RingFactory<C> cf, int n, TermOrder t)
    {
        throw new NotImplementedException();
    }

    public GenPolynomialRing(RingFactory<C> cf, string[] v)
    {
        throw new NotImplementedException();
    }

    public GenPolynomialRing(RingFactory<C> cf, int n, string[] v)
    {
        throw new NotImplementedException();
    }

    public GenPolynomialRing(RingFactory<C> cf, int n, TermOrder t, string[] v)
    {
        throw new NotImplementedException();
    }

    public GenPolynomial<C> GetZERO() { throw new NotImplementedException(); }
    public GenPolynomial<C> GetONE() { throw new NotImplementedException(); }
    public List<GenPolynomial<C>> Generators() { throw new NotImplementedException(); }
    public bool IsFinite() { throw new NotImplementedException(); }
    public bool IsCommutative() { throw new NotImplementedException(); }
    public bool IsAssociative() { throw new NotImplementedException(); }
    public bool IsField() { throw new NotImplementedException(); }
    public BigInteger Characteristic() { throw new NotImplementedException(); }
    public GenPolynomial<C> FromInteger(long a) { throw new NotImplementedException(); }
    public GenPolynomial<C> FromInteger(BigInteger a) { throw new NotImplementedException(); }
    GenPolynomial<C> ElemFactory<GenPolynomial<C>>.FromInteger(BigInteger a) { throw new NotImplementedException(); }
    public GenPolynomial<C> Random() { throw new NotImplementedException(); }
    public GenPolynomial<C> Random(int k) { throw new NotImplementedException(); }
    public GenPolynomial<C> Random(int k, Random rnd) { throw new NotImplementedException(); }
    public GenPolynomial<C> Copy(GenPolynomial<C> c) { throw new NotImplementedException(); }
    public IEnumerator<GenPolynomial<C>> GetEnumerator() { throw new NotImplementedException(); }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public override string ToString() { throw new NotImplementedException(); }
    public override bool Equals(object? obj) { throw new NotImplementedException(); }
    public override int GetHashCode() { throw new NotImplementedException(); }
}
