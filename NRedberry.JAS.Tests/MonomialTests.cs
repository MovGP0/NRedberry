using System.Collections.Generic;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class MonomialTests
{
    [Fact]
    public void ShouldExposeExponentAndCoefficient()
    {
        ExpVector exponent = ExpVector.Create([2L]);
        BigRational coefficient = new(3);
        Monomial<BigRational> monomial = new(exponent, coefficient);

        monomial.Exponent().GetVal().ShouldBe([2L]);
        monomial.Coefficient().ToString().ShouldBe("3");
        string text = monomial.ToString();
        text.ShouldContain("3");
    }

    [Fact]
    public void ShouldConstructFromKeyValuePair()
    {
        KeyValuePair<ExpVector, BigRational> pair = new(ExpVector.Create([1L]), new BigRational(2));
        Monomial<BigRational> monomial = new(pair);

        monomial.Exponent().GetVal().ShouldBe([1L]);
        monomial.Coefficient().ToString().ShouldBe("2");
    }
}
