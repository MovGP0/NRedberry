using System.Text;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Container for the approximation result from a Hensel algorithm.
/// </summary>
/// <typeparam name="MOD">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.HenselApprox
/// </remarks>
public sealed class HenselApprox<MOD>(
    GenPolynomial<Arith.BigInteger> a,
    GenPolynomial<Arith.BigInteger> b,
    GenPolynomial<MOD> am,
    GenPolynomial<MOD> bm)
    where MOD : GcdRingElem<MOD>, Modular
{
    /// <summary>
    /// Approximated polynomial with integer coefficients.
    /// </summary>
    public GenPolynomial<Arith.BigInteger> A { get; } = a ?? throw new ArgumentNullException(nameof(a));

    /// <summary>
    /// Approximated polynomial with integer coefficients.
    /// </summary>
    public GenPolynomial<Arith.BigInteger> B { get; } = b ?? throw new ArgumentNullException(nameof(b));

    /// <summary>
    /// Modular approximated polynomial with modular coefficients.
    /// </summary>
    public GenPolynomial<MOD> Am { get; } = am ?? throw new ArgumentNullException(nameof(am));

    /// <summary>
    /// Modular approximated polynomial with modular coefficients.
    /// </summary>
    public GenPolynomial<MOD> Bm { get; } = bm ?? throw new ArgumentNullException(nameof(bm));

    public override string ToString()
    {
        StringBuilder builder = new();
        builder.Append(A);
        builder.Append(',');
        builder.Append(B);
        builder.Append(',');
        builder.Append(Am);
        builder.Append(',');
        builder.Append(Bm);
        return builder.ToString();
    }

    public override int GetHashCode()
    {
        HashCode hashCode = new();
        hashCode.Add(A);
        hashCode.Add(B);
        hashCode.Add(Am);
        hashCode.Add(Bm);
        return hashCode.ToHashCode();
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj is not HenselApprox<MOD> other)
        {
            return false;
        }

        return A.Equals(other.A)
            && B.Equals(other.B)
            && Am.Equals(other.Am)
            && Bm.Equals(other.Bm);
    }
}
