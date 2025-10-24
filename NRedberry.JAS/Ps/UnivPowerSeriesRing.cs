using System.Numerics;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ps;

/// <summary>
/// Univariate power series ring implementation. Uses lazy evaluated generating function for coefficients.
/// </summary>
/// <typeparam name="C">ring element type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ps.UnivPowerSeriesRing
/// </remarks>
public class UnivPowerSeriesRing<C> : RingFactory<UnivPowerSeries<C>> where C : RingElem<C>
{
    private static readonly Random random = new();
    public const int DEFAULT_TRUNCATE = 11;

    internal int Truncate;
    private string var;
    public readonly RingFactory<C> CoFac;
    public readonly UnivPowerSeries<C> ONE;
    public readonly UnivPowerSeries<C> ZERO;

    public UnivPowerSeriesRing(object pfac)
    {
        throw new NotImplementedException();
    }

    public UnivPowerSeriesRing(RingFactory<C> cofac, int truncate, string name)
    {
        throw new NotImplementedException();
    }

    public override string ToString() { throw new NotImplementedException(); }
    public override bool Equals(object? B) { throw new NotImplementedException(); }
    public override int GetHashCode() { throw new NotImplementedException(); }
    public UnivPowerSeries<C> GetZERO() { throw new NotImplementedException(); }
    public UnivPowerSeries<C> GetONE() { throw new NotImplementedException(); }
    public List<UnivPowerSeries<C>> Generators() { throw new NotImplementedException(); }
    public bool IsFinite() { throw new NotImplementedException(); }
    public bool IsCommutative() { throw new NotImplementedException(); }
    public bool IsAssociative() { throw new NotImplementedException(); }
    public bool IsField() { throw new NotImplementedException(); }
    public BigInteger Characteristic() { throw new NotImplementedException(); }
    public UnivPowerSeries<C> FromInteger(long a) { throw new NotImplementedException(); }
    public UnivPowerSeries<C> FromInteger(BigInteger a) { throw new NotImplementedException(); }
    UnivPowerSeries<C> ElemFactory<UnivPowerSeries<C>>.FromInteger(BigInteger a) { throw new NotImplementedException(); }
    public UnivPowerSeries<C> Random() { throw new NotImplementedException(); }
    public UnivPowerSeries<C> Random(int k) { throw new NotImplementedException(); }
    public UnivPowerSeries<C> Random(int k, Random rnd) { throw new NotImplementedException(); }
    public UnivPowerSeries<C> Random(int k, float d) { throw new NotImplementedException(); }
    public UnivPowerSeries<C> Random(int k, float d, Random rnd) { throw new NotImplementedException(); }
    public UnivPowerSeries<C> Copy(UnivPowerSeries<C> c) { throw new NotImplementedException(); }
}
