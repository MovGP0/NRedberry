using System.Collections.Generic;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;
using Shouldly;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class FactorAbsoluteTests
{
    [Fact]
    public void ShouldUseConcreteTypeNameForToString()
    {
        TestFactorAbsolute factor = new(new BigRational());

        factor.ToString().ShouldContain(nameof(TestFactorAbsolute));
    }
}

file sealed class TestFactorAbsolute(RingFactory<BigRational> coefficientFactory) : FactorAbsolute<BigRational>(coefficientFactory)
{
    public override List<GenPolynomial<BigRational>> BaseFactorsSquarefree(GenPolynomial<BigRational> polynomial)
    {
        return [polynomial];
    }
}
