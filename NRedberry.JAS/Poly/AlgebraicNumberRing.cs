using System.Numerics;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// Algebraic number ring factory based on GenPolynomial with RingFactory interface.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.AlgebraicNumberRing
/// </remarks>
public class AlgebraicNumberRing<C> : RingFactory<AlgebraicNumber<C>> where C : RingElem<C>
{
    public readonly GenPolynomialRing<C> Ring;
    public readonly GenPolynomial<C> Modul;

    public AlgebraicNumberRing(GenPolynomial<C> m)
    {
        throw new NotImplementedException();
    }

    public AlgebraicNumberRing(GenPolynomial<C> m, bool isField)
    {
        throw new NotImplementedException();
    }

    public GenPolynomial<C> GetModul() => Modul;
    public static AlgebraicNumber<C> Clone(AlgebraicNumber<C> c) { throw new NotImplementedException(); }
    public static AlgebraicNumber<C> Zero => throw new NotImplementedException();
    public static AlgebraicNumber<C> One => throw new NotImplementedException();
    public AlgebraicNumber<C> GetGenerator() { throw new NotImplementedException(); }
    public List<AlgebraicNumber<C>> Generators() { throw new NotImplementedException(); }
    public bool IsFinite() { throw new NotImplementedException(); }
    public bool IsCommutative() { throw new NotImplementedException(); }
    public bool IsAssociative() { throw new NotImplementedException(); }
    public bool IsField() { throw new NotImplementedException(); }
    public BigInteger Characteristic() { throw new NotImplementedException(); }
    public AlgebraicNumber<C> FromInteger(long a) { throw new NotImplementedException(); }
    public AlgebraicNumber<C> FromInteger(BigInteger a) { throw new NotImplementedException(); }
    AlgebraicNumber<C> ElemFactory<AlgebraicNumber<C>>.FromInteger(BigInteger a) { throw new NotImplementedException(); }
    public AlgebraicNumber<C> Random() { throw new NotImplementedException(); }
    public AlgebraicNumber<C> Random(int k) { throw new NotImplementedException(); }
    public AlgebraicNumber<C> Random(int k, Random rnd) { throw new NotImplementedException(); }
    public override string ToString() { throw new NotImplementedException(); }
    public override bool Equals(object? obj) { throw new NotImplementedException(); }
    public override int GetHashCode() { throw new NotImplementedException(); }
}
