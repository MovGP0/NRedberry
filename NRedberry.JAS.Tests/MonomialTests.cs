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

        Assert.Equal([2L], monomial.Exponent().GetVal());
        Assert.Equal("3", monomial.Coefficient().ToString());
        string text = monomial.ToString();
        Assert.Contains("3", text, System.StringComparison.Ordinal);
    }

    [Fact]
    public void ShouldConstructFromKeyValuePair()
    {
        KeyValuePair<ExpVector, BigRational> pair = new(ExpVector.Create([1L]), new BigRational(2));
        Monomial<BigRational> monomial = new(pair);

        Assert.Equal([1L], monomial.Exponent().GetVal());
        Assert.Equal("2", monomial.Coefficient().ToString());
    }
}
