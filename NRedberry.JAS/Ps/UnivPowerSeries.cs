using System.Text;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ps;

/// <summary>
/// Univariate power series implementation with lazily evaluated coefficient-generating functions.
/// Inner helper classes provide the lazy generators, while eager evaluation is limited to <c>ToString()</c>,
/// <c>CompareTo()</c>, <c>Equals()</c>, <c>Evaluate()</c>, and any method that uses <c>Order()</c> (for
/// example, <c>Signum()</c>, <c>Abs()</c>, <c>Divide()</c>, <c>Remainder()</c>, and <c>Gcd()</c>).
/// </summary>
/// <typeparam name="C">Ring element type.</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ps.UnivPowerSeries
/// </remarks>
public class UnivPowerSeries<C> : RingElem<UnivPowerSeries<C>> where C : RingElem<C>
{
    public readonly UnivPowerSeriesRing<C> Ring;
    internal Coefficients<C> LazyCoeffs;
    private int truncate;
    private int order = -1;

    private UnivPowerSeries()
    {
        throw new ArgumentException("do not use no-argument constructor");
    }

    public UnivPowerSeries(UnivPowerSeriesRing<C> ring, Coefficients<C> lazyCoeffs)
    {
        Ring = ring ?? throw new ArgumentNullException(nameof(ring));
        LazyCoeffs = lazyCoeffs ?? throw new ArgumentNullException(nameof(lazyCoeffs));
        truncate = Ring.Truncate;
    }

    public UnivPowerSeriesRing<C> Factory() => Ring;

    public UnivPowerSeries<C> Clone() => new(Ring, LazyCoeffs);

    public override string ToString() => ToString(truncate);

    public string ToString(int requestedTruncate)
    {
        StringBuilder sb = new StringBuilder();
        int limit = Math.Min(requestedTruncate, Ring.Truncate);
        string variable = Ring.Var;

        for (int i = 0; i < limit; i++)
        {
            C coefficient = Coefficient(i);
            int sign = coefficient.Signum();
            if (sign == 0)
            {
                continue;
            }

            if (sign > 0)
            {
                if (sb.Length > 0)
                {
                    sb.Append(" + ");
                }
            }
            else
            {
                coefficient = coefficient.Negate();
                sb.Append(" - ");
            }

            bool wrap = coefficient is GenPolynomial<C>
                || coefficient is AlgebraicNumber<C>;

            if (!coefficient.IsOne() || i == 0)
            {
                if (wrap)
                {
                    sb.Append("{ ");
                }

                sb.Append(coefficient.ToString());

                if (wrap)
                {
                    sb.Append(" }");
                }

                if (i > 0)
                {
                    sb.Append(" * ");
                }
            }

            if (i == 0)
            {
                // nothing extra
            }
            else if (i == 1)
            {
                sb.Append(variable);
            }
            else
            {
                sb.Append(variable).Append("^").Append(i);
            }
        }

        if (sb.Length == 0)
        {
            sb.Append("0");
        }

        sb
            .Append(" + BigO(")
            .Append(variable)
            .Append("^")
            .Append(limit)
            .Append(")");

        return sb.ToString();
    }

    public C Coefficient(int index)
    {
        if (index < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(index), index, "index must be non-negative");
        }

        return LazyCoeffs.Get(index);
    }

    public C LeadingCoefficient() => Coefficient(0);

    public UnivPowerSeries<C> Shift(int k)
    {
        return new UnivPowerSeries<C>(
            Ring,
            new LambdaCoefficients((i, _) =>
            {
                int idx = i - k;
                if (idx < 0)
                {
                    return RingFactory<C>.Zero;
                }

                return Coefficient(idx);
            }));
    }

    public UnivPowerSeries<C> Map(UnaryFunctor<C, C> f)
    {
        ArgumentNullException.ThrowIfNull(f);

        return new UnivPowerSeries<C>(
            Ring,
            new LambdaCoefficients((i, _) => f.Eval(Coefficient(i))));
    }

    public UnivPowerSeries<C> Zip<C2>(BinaryFunctor<C, C2, C> f, UnivPowerSeries<C2> ps) where C2 : RingElem<C2>
    {
        ArgumentNullException.ThrowIfNull(f);
        ArgumentNullException.ThrowIfNull(ps);

        return new UnivPowerSeries<C>(
            Ring,
            new LambdaCoefficients((i, _) => f.Eval(Coefficient(i), ps.Coefficient(i))));
    }

    public UnivPowerSeries<C> Sum(UnivPowerSeries<C> ps) => Zip(new SumFunctor(), ps);

    public UnivPowerSeries<C> Subtract(UnivPowerSeries<C> ps) => Zip(new SubtractFunctor(), ps);

    public UnivPowerSeries<C> Multiply(C c)
    {
        ArgumentNullException.ThrowIfNull(c);
        return Map(new MultiplyFunctor(c));
    }

    public UnivPowerSeries<C> Negate() => Map(new NegateFunctor());

    public UnivPowerSeries<C> Abs() => Signum() < 0 ? Negate() : this;

    public int Order()
    {
        if (order >= 0)
        {
            return order;
        }

        int limit = Ring.Truncate;
        truncate = limit;

        for (int i = 0; i <= limit; i++)
        {
            if (!Coefficient(i).IsZero())
            {
                order = i;
                return order;
            }
        }

        order = limit + 1;
        return order;
    }

    public int Signum() => Coefficient(Order()).Signum();

    public int CompareTo(UnivPowerSeries<C>? ps)
    {
        if (ReferenceEquals(this, ps))
        {
            return 0;
        }

        if (ps is null)
        {
            return Signum();
        }

        int m = Order();
        int n = ps.Order();
        int pos = Math.Min(m, n);
        int s;

        do
        {
            s = Coefficient(pos).CompareTo(ps.Coefficient(pos));
            pos++;
        }
        while (s == 0 && pos <= truncate);

        return s;
    }

    public bool IsZero() => CompareTo(Ring.ZERO) == 0;

    public bool IsOne() => CompareTo(Ring.ONE) == 0;

    public bool IsUnit() => LeadingCoefficient().IsUnit();

    public override bool Equals(object? obj)
    {
        if (obj is not UnivPowerSeries<C> other)
        {
            return false;
        }

        return CompareTo(other) == 0;
    }

    public override int GetHashCode()
    {
        int hash = 0;
        for (int i = 0; i <= truncate; i++)
        {
            hash += Coefficient(i).GetHashCode();
            hash <<= 23;
        }

        return hash;
    }

    public UnivPowerSeries<C> Multiply(UnivPowerSeries<C> ps)
    {
        ArgumentNullException.ThrowIfNull(ps);

        return new UnivPowerSeries<C>(
            Ring,
            new LambdaCoefficients((i, _) =>
            {
                bool first = true;
                C? sum = default;
                for (int k = 0; k <= i; k++)
                {
                    C term = Coefficient(k).Multiply(ps.Coefficient(i - k));
                    if (first)
                    {
                        sum = term;
                        first = false;
                    }
                    else
                    {
                        sum = sum!.Sum(term);
                    }
                }

                return sum!;
            }));
    }

    public UnivPowerSeries<C> Inverse()
    {
        return new UnivPowerSeries<C>(
            Ring,
            new LambdaCoefficients((i, get) =>
            {
                C d = LeadingCoefficient().Inverse();
                if (i == 0)
                {
                    return d;
                }

                bool first = true;
                C? sum = default;
                for (int k = 0; k < i; k++)
                {
                    C term = get(k).Multiply(Coefficient(i - k));
                    if (first)
                    {
                        sum = term;
                        first = false;
                    }
                    else
                    {
                        sum = sum!.Sum(term);
                    }
                }

                return sum!.Multiply(d.Negate());
            }));
    }

    public UnivPowerSeries<C> Divide(UnivPowerSeries<C> ps)
    {
        ArgumentNullException.ThrowIfNull(ps);

        if (ps.IsUnit())
        {
            return Multiply(ps.Inverse());
        }

        int m = Order();
        int n = ps.Order();

        if (m < n)
        {
            return Ring.GetZero();
        }

        if (!ps.Coefficient(n).IsUnit())
        {
            throw new ArithmeticException("division by non unit coefficient " + ps.Coefficient(n) + ", n = " + n);
        }

        UnivPowerSeries<C> shiftedThis = m == 0 ? this : Shift(-m);
        UnivPowerSeries<C> shiftedDivisor = n == 0 ? ps : ps.Shift(-n);
        UnivPowerSeries<C> quotient = shiftedThis.Multiply(shiftedDivisor.Inverse());

        return m == n ? quotient : quotient.Shift(m - n);
    }

    public UnivPowerSeries<C> Remainder(UnivPowerSeries<C> ps)
    {
        ArgumentNullException.ThrowIfNull(ps);

        int m = Order();
        int n = ps.Order();
        if (m >= n)
        {
            return Ring.GetZero();
        }

        return this;
    }

    public UnivPowerSeries<C> Gcd(UnivPowerSeries<C> ps)
    {
        ArgumentNullException.ThrowIfNull(ps);

        if (ps.IsZero())
        {
            return this;
        }

        if (IsZero())
        {
            return ps;
        }

        int ll = Math.Min(Order(), ps.Order());
        return Ring.GetONE().Shift(ll);
    }

    public UnivPowerSeries<C>[] Egcd(UnivPowerSeries<C> ps)
    {
        throw new NotSupportedException("EGCD for power series is not implemented");
    }

    ElemFactory<UnivPowerSeries<C>> Element<UnivPowerSeries<C>>.Factory() => Factory();

    private sealed class LambdaCoefficients : Coefficients<C>
    {
        private readonly Func<int, Func<int, C>, C> generator;

        public LambdaCoefficients(Func<int, Func<int, C>, C> generator)
        {
            this.generator = generator ?? throw new ArgumentNullException(nameof(generator));
        }

        protected override C Generate(int index) => generator(index, idx => Get(idx));
    }

    private sealed class SumFunctor : BinaryFunctor<C, C, C>
    {
        public C Eval(C c1, C c2) => c1.Sum(c2);
    }

    private sealed class SubtractFunctor : BinaryFunctor<C, C, C>
    {
        public C Eval(C c1, C c2) => c1.Subtract(c2);
    }

    private sealed class MultiplyFunctor : UnaryFunctor<C, C>
    {
        private readonly C factor;

        public MultiplyFunctor(C factor)
        {
            this.factor = factor;
        }

        public C Eval(C c) => c.Multiply(factor);
    }

    private sealed class NegateFunctor : UnaryFunctor<C, C>
    {
        public C Eval(C c) => c.Negate();
    }
}
