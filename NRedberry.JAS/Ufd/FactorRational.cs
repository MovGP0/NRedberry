using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Rational number coefficients factorization algorithms.
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.FactorRational
/// </remarks>
public class FactorRational : FactorAbsolute<BigRational>
{
    public FactorRational() : base(BigRational.One)
    {
    }

    public override List<GenPolynomial<BigRational>> BaseFactorsSquarefree(GenPolynomial<BigRational> P)
    {
        throw new NotImplementedException();
    }

    public override List<GenPolynomial<BigRational>> FactorsSquarefree(GenPolynomial<BigRational> P)
    {
        throw new NotImplementedException();
    }
}
