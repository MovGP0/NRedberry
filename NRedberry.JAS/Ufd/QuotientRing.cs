using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Quotient ring factory based on GenPolynomial with RingElem interface.
/// Objects of this class are immutable.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.QuotientRing
/// </remarks>
public class QuotientRing<C> : RingFactory<Quotient<C>> where C : GcdRingElem<C>
{
    public readonly GenPolynomialRing<C> Ring;
    public readonly GreatestCommonDivisor<C>? Engine;
    public readonly bool UfdGCD;

    public QuotientRing(GenPolynomialRing<C> r, bool ufdGCD)
    {
        Ring = r;
        UfdGCD = ufdGCD;
        if (ufdGCD)
        {
            Engine = null; // Will be initialized lazily
        }
        else
        {
            Engine = null;
        }
    }

    public bool IsFinite() => throw new NotImplementedException();

    public static Quotient<C> Clone(Quotient<C> c) => throw new NotImplementedException();

    public static Quotient<C> Zero => throw new NotImplementedException();

    public static Quotient<C> One => throw new NotImplementedException();

    public List<Quotient<C>> Generators() => throw new NotImplementedException();

    public bool IsCommutative() => throw new NotImplementedException();

    public bool IsAssociative() => throw new NotImplementedException();

    public bool IsField() => throw new NotImplementedException();

    public Quotient<C> FromInteger(long a) => throw new NotImplementedException();

    public Quotient<C> FromInteger(System.Numerics.BigInteger a) => throw new NotImplementedException();

    public Quotient<C> Random(int n) => throw new NotImplementedException();

    public Quotient<C> Random(int n, Random rnd) => throw new NotImplementedException();

    public System.Numerics.BigInteger Characteristic() => throw new NotImplementedException();

    public Quotient<C> Parse(string s) => throw new NotImplementedException();

    public override string ToString() => throw new NotImplementedException();

    public override bool Equals(object? b) => throw new NotImplementedException();

    public override int GetHashCode() => throw new NotImplementedException();

    public string ToScript() => throw new NotImplementedException();
}
