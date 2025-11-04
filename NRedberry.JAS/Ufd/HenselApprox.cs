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
public class HenselApprox<MOD> where MOD : GcdRingElem<MOD>, Modular
{
    public readonly GenPolynomial<Arith.BigInteger> A;
    public readonly GenPolynomial<Arith.BigInteger> B;
    public readonly GenPolynomial<MOD> Am;
    public readonly GenPolynomial<MOD> Bm;

    public HenselApprox(
        GenPolynomial<Arith.BigInteger> A,
        GenPolynomial<Arith.BigInteger> B,
        GenPolynomial<MOD> Am,
        GenPolynomial<MOD> Bm)
    {
        this.A = A;
        this.B = B;
        this.Am = Am;
        this.Bm = Bm;
    }

    public override string ToString()
    {
        throw new NotImplementedException();
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }

    public override bool Equals(object? obj)
    {
        throw new NotImplementedException();
    }
}
