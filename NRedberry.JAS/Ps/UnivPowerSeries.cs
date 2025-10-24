using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ps;

/// <summary>
/// Univariate power series implementation. Uses inner classes and lazy evaluated generating function for coefficients.
/// All ring element methods use lazy evaluation except where noted otherwise. Eager evaluated methods are
/// ToString(), CompareTo(), Equals(), Evaluate(), or they use the Order() method, like
/// Signum(), Abs(), Divide(), Remainder() and Gcd().
/// </summary>
/// <typeparam name="C">ring element type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ps.UnivPowerSeries
/// </remarks>
public class UnivPowerSeries<C> : RingElem<UnivPowerSeries<C>> where C : RingElem<C>
{
    public readonly UnivPowerSeriesRing<C> Ring;
    internal Coefficients<C> LazyCoeffs;
    private int truncate = 11;
    private int order = -1;

    public UnivPowerSeries(UnivPowerSeriesRing<C> ring, Coefficients<C> lazyCoeffs)
    {
        Ring = ring ?? throw new ArgumentNullException(nameof(ring));
        LazyCoeffs = lazyCoeffs ?? throw new ArgumentNullException(nameof(lazyCoeffs));
        truncate = ring.Truncate;
    }

    public UnivPowerSeriesRing<C> Factory() => Ring;
    public UnivPowerSeries<C> Copy() { throw new NotImplementedException(); }
    public override string ToString() { throw new NotImplementedException(); }
    public string ToString(int truncate) { throw new NotImplementedException(); }
    public C Coefficient(int index) { throw new NotImplementedException(); }
    public C LeadingCoefficient() { throw new NotImplementedException(); }
    public UnivPowerSeries<C> Shift(int k) { throw new NotImplementedException(); }
    public UnivPowerSeries<C> Map(UnaryFunctor<C, C> f) { throw new NotImplementedException(); }
    public UnivPowerSeries<C> Zip<C2>(BinaryFunctor<C, C2, C> f, UnivPowerSeries<C2> ps) where C2 : RingElem<C2> { throw new NotImplementedException(); }
    public UnivPowerSeries<C> Sum(UnivPowerSeries<C> ps) { throw new NotImplementedException(); }
    public UnivPowerSeries<C> Subtract(UnivPowerSeries<C> ps) { throw new NotImplementedException(); }
    public UnivPowerSeries<C> Multiply(C c) { throw new NotImplementedException(); }
    public UnivPowerSeries<C> Negate() { throw new NotImplementedException(); }
    public UnivPowerSeries<C> Abs() { throw new NotImplementedException(); }
    public int Order() { throw new NotImplementedException(); }
    public int Signum() { throw new NotImplementedException(); }
    public int CompareTo(UnivPowerSeries<C>? ps) { throw new NotImplementedException(); }
    public bool IsZERO() { throw new NotImplementedException(); }
    public bool IsONE() { throw new NotImplementedException(); }
    public bool IsUnit() { throw new NotImplementedException(); }
    public override bool Equals(object? obj) { throw new NotImplementedException(); }
    public override int GetHashCode() { throw new NotImplementedException(); }
    public UnivPowerSeries<C> Divide(UnivPowerSeries<C> S) { throw new NotImplementedException(); }
    public UnivPowerSeries<C> Inverse() { throw new NotImplementedException(); }
    public UnivPowerSeries<C> Remainder(UnivPowerSeries<C> S) { throw new NotImplementedException(); }
    public UnivPowerSeries<C> Gcd(UnivPowerSeries<C> b) { throw new NotImplementedException(); }
    public UnivPowerSeries<C>[] Egcd(UnivPowerSeries<C> b) { throw new NotImplementedException(); }
    public UnivPowerSeries<C> Multiply(UnivPowerSeries<C> S) { throw new NotImplementedException(); }

    ElemFactory<UnivPowerSeries<C>> Element<UnivPowerSeries<C>>.Factory() => Factory();
}
