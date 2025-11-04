using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// Generic Complex class implementing the RingElem interface. Objects of this class are immutable.
/// </summary>
/// <typeparam name="C">Base ring element type.</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.Complex
/// </remarks>
public class Complex<C> : GcdRingElem<Complex<C>> where C : RingElem<C>
{
    public Complex(ComplexRing<C> ring, C r, C i)
    {
        ArgumentNullException.ThrowIfNull(ring);
        ArgumentNullException.ThrowIfNull(r);
        ArgumentNullException.ThrowIfNull(i);
        Ring = ring;
        Re = r;
        Im = i;
    }

    public Complex(ComplexRing<C> ring, C r)
        : this(ring, r, ring.Ring.FromInteger(0))
    {
    }

    public Complex(ComplexRing<C> ring, long r)
        : this(ring, ring.Ring.FromInteger(r))
    {
    }

    public Complex(ComplexRing<C> ring)
        : this(ring, ring.Ring.FromInteger(0))
    {
    }

    /// <summary>
    /// Complex class factory data structure.
    /// </summary>
    public ComplexRing<C> Ring { get; }

    /// <summary>
    /// Real part of the data structure.
    /// </summary>
    public C Re { get; }

    /// <summary>
    /// Imaginary part of the data structure.
    /// </summary>
    public C Im { get; }

    /// <inheritdoc />
    public ComplexRing<C> Factory()
    {
        return Ring;
    }

    /// <inheritdoc />
    public Complex<C> Clone()
    {
        return new Complex<C>(Ring, Re, Im);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        string s = Re.ToString();
        if (Im.IsZero())
        {
            return s;
        }

        s += "i" + Im;
        return s;
    }

    /// <inheritdoc />
    public bool IsZero()
    {
        return Re.IsZero() && Im.IsZero();
    }

    /// <inheritdoc />
    public bool IsOne()
    {
        return Re.IsOne() && Im.IsZero();
    }

    /// <summary>
    /// Determines whether the complex number is the imaginary unit.
    /// </summary>
    /// <returns>True if this instance equals i; otherwise false.</returns>
    public bool IsImag()
    {
        return Re.IsZero() && Im.IsOne();
    }

    /// <inheritdoc />
    public bool IsUnit()
    {
        if (IsZero())
        {
            return false;
        }

        if (Ring.IsField())
        {
            return true;
        }

        return Norm().Re.IsUnit();
    }

    /// <inheritdoc />
    public int CompareTo(Complex<C>? other)
    {
        if (other is null)
        {
            return 1;
        }

        int s = Re.CompareTo(other.Re);
        if (s != 0)
        {
            return s;
        }

        return Im.CompareTo(other.Im);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj is not Complex<C> other)
        {
            return false;
        }

        if (!Ring.Equals(other.Ring))
        {
            return false;
        }

        return Re.Equals(other.Re) && Im.Equals(other.Im);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        HashCode hashCode = new ();
        hashCode.Add(Re);
        hashCode.Add(Im);
        return hashCode.ToHashCode();
    }

    /// <inheritdoc />
    public Complex<C> Abs()
    {
        return Norm();
    }

    /// <inheritdoc />
    public Complex<C> Negate()
    {
        return new Complex<C>(Ring, Re.Negate(), Im.Negate());
    }

    /// <inheritdoc />
    public int Signum()
    {
        int s = Re.Signum();
        if (s != 0)
        {
            return s;
        }

        return Im.Signum();
    }

    /// <inheritdoc />
    public Complex<C> Subtract(Complex<C> S)
    {
        ArgumentNullException.ThrowIfNull(S);
        return new Complex<C>(Ring, Re.Subtract(S.Re), Im.Subtract(S.Im));
    }

    /// <inheritdoc />
    public Complex<C> Divide(Complex<C> S)
    {
        ArgumentNullException.ThrowIfNull(S);
        if (Ring.IsField())
        {
            return Multiply(S.Inverse());
        }

        return QuotientRemainder(S)[0];
    }

    /// <inheritdoc />
    public Complex<C> Inverse()
    {
        C a = Norm().Re.Inverse();
        return new Complex<C>(Ring, Re.Multiply(a), Im.Multiply(a.Negate()));
    }

    /// <inheritdoc />
    public Complex<C> Remainder(Complex<C> S)
    {
        ArgumentNullException.ThrowIfNull(S);
        if (Ring.IsField())
        {
            return Ring.Zero;
        }

        return QuotientRemainder(S)[1];
    }

    /// <inheritdoc />
    public Complex<C> Gcd(Complex<C> b)
    {
        if (b?.IsZero() != false)
        {
            return this;
        }

        if (IsZero())
        {
            return b;
        }

        if (Ring.IsField())
        {
            return Ring.One;
        }

        Complex<C> a = this;
        Complex<C> other = b;
        if (a.Re.Signum() < 0)
        {
            a = a.Negate();
        }

        if (other.Re.Signum() < 0)
        {
            other = other.Negate();
        }

        while (!other.IsZero())
        {
            Complex<C>[] qr = a.QuotientRemainder(other);
            a = other;
            other = qr[1];
        }

        if (a.Re.Signum() < 0)
        {
            a = a.Negate();
        }

        return a;
    }

    /// <inheritdoc />
    public Complex<C>[] Egcd(Complex<C> b)
    {
        Complex<C>[] ret = new Complex<C>[3];
        if (b?.IsZero() != false)
        {
            ret[0] = this;
            return ret;
        }

        if (IsZero())
        {
            ret[0] = b;
            return ret;
        }

        if (Ring.IsField())
        {
            C one = Ring.Ring.FromInteger(1);
            C two = Ring.Ring.FromInteger(2);
            Complex<C> half = new Complex<C>(Ring, one.Divide(two));
            ret[0] = Ring.One;
            ret[1] = Inverse().Multiply(half);
            ret[2] = b.Inverse().Multiply(half);
            return ret;
        }

        Complex<C>[] qr;
        Complex<C> q = this;
        Complex<C> r = b;
        Complex<C> c1 = Ring.One;
        Complex<C> d1 = Ring.Zero;
        Complex<C> c2 = Ring.Zero;
        Complex<C> d2 = Ring.One;

        while (!r.IsZero())
        {
            qr = q.QuotientRemainder(r);
            Complex<C> x1 = c1.Subtract(qr[0].Multiply(d1));
            Complex<C> x2 = c2.Subtract(qr[0].Multiply(d2));
            c1 = d1;
            c2 = d2;
            d1 = x1;
            d2 = x2;
            q = r;
            r = qr[1];
        }

        if (q.Re.Signum() < 0)
        {
            q = q.Negate();
            c1 = c1.Negate();
            c2 = c2.Negate();
        }

        ret[0] = q;
        ret[1] = c1;
        ret[2] = c2;
        return ret;
    }

    /// <inheritdoc />
    public Complex<C> Multiply(Complex<C> S)
    {
        ArgumentNullException.ThrowIfNull(S);
        C real = Re.Multiply(S.Re).Subtract(Im.Multiply(S.Im));
        C imag = Re.Multiply(S.Im).Sum(Im.Multiply(S.Re));
        return new Complex<C>(Ring, real, imag);
    }

    /// <inheritdoc />
    public Complex<C> Sum(Complex<C> S)
    {
        ArgumentNullException.ThrowIfNull(S);
        return new Complex<C>(Ring, Re.Sum(S.Re), Im.Sum(S.Im));
    }

    /// <summary>
    /// Complex number conjugate.
    /// </summary>
    /// <returns>The complex conjugate of this instance.</returns>
    public Complex<C> Conjugate()
    {
        return new Complex<C>(Ring, Re, Im.Negate());
    }

    /// <summary>
    /// Complex number norm.
    /// </summary>
    /// <returns>||this||.</returns>
    public Complex<C> Norm()
    {
        C v = Re.Multiply(Re);
        v = v.Sum(Im.Multiply(Im));
        return new Complex<C>(Ring, v);
    }

    /// <summary>
    /// Complex number quotient and remainder.
    /// </summary>
    /// <param name="S">Divisor.</param>
    /// <returns>Array [q, r] with q = this / S and r = this % S.</returns>
    public Complex<C>[] QuotientRemainder(Complex<C> S)
    {
        ArgumentNullException.ThrowIfNull(S);
        Complex<C>[] ret = new Complex<C>[2];
        C n = S.Norm().Re;
        Complex<C> Sp = Multiply(S.Conjugate());
        C qr = Sp.Re.Divide(n);
        C rr = Sp.Re.Remainder(n);
        C qi = Sp.Im.Divide(n);
        C ri = Sp.Im.Remainder(n);
        C rr1 = rr;
        C ri1 = ri;
        if (rr.Signum() < 0)
        {
            rr = rr.Negate();
        }

        if (ri.Signum() < 0)
        {
            ri = ri.Negate();
        }

        C one = n.Factory().FromInteger(1);
        if (rr.Sum(rr).CompareTo(n) > 0)
        {
            qr = rr1.Signum() < 0 ? qr.Subtract(one) : qr.Sum(one);
        }

        if (ri.Sum(ri).CompareTo(n) > 0)
        {
            qi = ri1.Signum() < 0 ? qi.Subtract(one) : qi.Sum(one);
        }

        Complex<C> quotient = new Complex<C>(Ring, qr, qi);
        Complex<C> remainder = Subtract(quotient.Multiply(S));
        ret[0] = quotient;
        ret[1] = remainder;
        return ret;
    }

    ElemFactory<Complex<C>> Element<Complex<C>>.Factory()
    {
        return Factory();
    }
}
