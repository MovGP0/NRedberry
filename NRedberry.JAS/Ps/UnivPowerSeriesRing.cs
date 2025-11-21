using System.Numerics;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ps;

/// <summary>
/// Univariate power series ring implementation. Uses lazy evaluated generating functions for coefficients,
/// provides zero/one constants, and mirrors the Java version by exposing truncation, variable name, and random sampling.
/// </summary>
/// <typeparam name="C">Ring element type.</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ps.UnivPowerSeriesRing
/// </remarks>
public class UnivPowerSeriesRing<C> : RingFactory<UnivPowerSeries<C>> where C : RingElem<C>
{
    private static readonly Random random = new();

    public const int DEFAULT_TRUNCATE = 11;

    private int truncate;

    public string Var { get; }

    public int Truncate => truncate;

    public readonly RingFactory<C> CoFac;
    public readonly UnivPowerSeries<C> ONE;
    public readonly UnivPowerSeries<C> ZERO;

    private UnivPowerSeriesRing()
    {
        throw new ArgumentException("do not use no-argument constructor");
    }

    public UnivPowerSeriesRing(GenPolynomialRing<C> pfac)
        : this(pfac?.CoFac ?? throw new ArgumentNullException(nameof(pfac)), DEFAULT_TRUNCATE, "t")
    {
    }

    public UnivPowerSeriesRing(RingFactory<C> cofac, int truncate, string name)
    {
        CoFac = cofac ?? throw new ArgumentNullException(nameof(cofac));
        this.truncate = truncate;
        Var = string.IsNullOrWhiteSpace(name) ? "t" : name;

        ONE = new UnivPowerSeries<C>(this, new LambdaCoefficients((i, _) => i == 0 ? RingFactory<C>.One : RingFactory<C>.Zero));
        ZERO = new UnivPowerSeries<C>(this, new LambdaCoefficients((_, _) => RingFactory<C>.Zero));
    }

    public override string ToString()
    {
        string scf = CoFac.GetType().Name;
        return $"{scf}(({Var}))";
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj is not UnivPowerSeriesRing<C> other)
        {
            return false;
        }

        return CoFac.Equals(other.CoFac) && string.Equals(Var, other.Var, StringComparison.Ordinal);
    }

    public override int GetHashCode()
    {
        int h = CoFac.GetHashCode();
        h += Var.GetHashCode() << 27;
        h += truncate;
        return h;
    }

    public UnivPowerSeries<C> GetZero() => ZERO;

    public UnivPowerSeries<C> GetONE() => ONE;

    public List<UnivPowerSeries<C>> Generators()
    {
        List<C> coefficientGenerators = CoFac.Generators();
        List<UnivPowerSeries<C>> gens = new(coefficientGenerators.Count + 1);

        foreach (C cg in coefficientGenerators)
        {
            gens.Add(new UnivPowerSeries<C>(
                this,
                new LambdaCoefficients((i, _) => i == 0 ? cg : RingFactory<C>.Zero)));
        }

        gens.Add(ONE.Shift(1));
        return gens;
    }

    public bool IsFinite() => false;

    public bool IsCommutative() => CoFac.IsCommutative();

    public bool IsAssociative() => CoFac.IsAssociative();

    public bool IsField() => false;

    public BigInteger Characteristic() => CoFac.Characteristic();

    public UnivPowerSeries<C> FromInteger(long a) => ONE.Multiply(CoFac.FromInteger(a));

    public UnivPowerSeries<C> FromInteger(BigInteger a) => ONE.Multiply(CoFac.FromInteger(a));

    UnivPowerSeries<C> ElemFactory<UnivPowerSeries<C>>.FromInteger(BigInteger a) => FromInteger(a);

    public UnivPowerSeries<C> Random() => Random(5, 0.7f, random);

    public UnivPowerSeries<C> Random(int k) => Random(k, 0.7f, random);

    public UnivPowerSeries<C> Random(int k, Random rnd) => Random(k, 0.7f, rnd);

    public UnivPowerSeries<C> Random(int k, float d) => Random(k, d, random);

    public UnivPowerSeries<C> Random(int k, float d, Random rnd)
    {
        return new UnivPowerSeries<C>(
            this,
            new LambdaCoefficients((_, _) =>
            {
                double f = rnd.NextDouble();
                return f < d ? CoFac.Random(k, rnd) : RingFactory<C>.Zero;
            }));
    }

    public static UnivPowerSeries<C> Clone(UnivPowerSeries<C> c) => c.Clone();

    public UnivPowerSeries<C> Copy(UnivPowerSeries<C> c)
    {
        ArgumentNullException.ThrowIfNull(c);
        return new UnivPowerSeries<C>(this, c.LazyCoeffs);
    }

    public UnivPowerSeries<C> SeriesOfTaylor(TaylorFunction<C> f, C a)
    {
        ArgumentNullException.ThrowIfNull(f);
        ArgumentNullException.ThrowIfNull(a);

        return new UnivPowerSeries<C>(this, new TaylorCoefficients(this, f, a));
    }

    private sealed class LambdaCoefficients(Func<int, Func<int, C>, C> generator) : Coefficients<C>
    {
        private readonly Func<int, Func<int, C>, C> generator = generator ?? throw new ArgumentNullException(nameof(generator));

        protected override C Generate(int index) => generator(index, idx => Get(idx));
    }

    private sealed class TaylorCoefficients(UnivPowerSeriesRing<C> owner, TaylorFunction<C> function, C expansionPoint)
        : Coefficients<C>
    {
        private TaylorFunction<C> derivative = function;
        private long k;
        private long factorial = 1;

        protected override C Generate(int index)
        {
            if (index == 0)
            {
                C value = derivative.Evaluate(expansionPoint);
                derivative = derivative.Deriviative();
                return value;
            }

            // ensure derivatives are advanced
            Get(index - 1);

            k++;
            factorial *= k;
            C result = derivative.Evaluate(expansionPoint);
            C denom = owner.CoFac.FromInteger(factorial);
            result = result.Divide(denom);
            derivative = derivative.Deriviative();
            return result;
        }
    }
}
