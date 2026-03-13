using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Util;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class ListUtilTests
{
    [Fact]
    public void ShouldMapUnaryFunctorAcrossList()
    {
        List<BigRational> values = [new(1), new(2), new(3)];

        List<BigRational>? mapped = ListUtil.Map(values, new IncrementBigRationalFunctor());

        Assert.Equal([new BigRational(2), new BigRational(3), new BigRational(4)], mapped);
    }

    [Fact]
    public void ShouldReturnNullForNullInputList()
    {
        Assert.Null(ListUtil.Map<BigRational, BigRational>(null, new IncrementBigRationalFunctor()));
    }
}

file sealed class IncrementBigRationalFunctor : UnaryFunctor<BigRational, BigRational>
{
    public BigRational Eval(BigRational c)
    {
        return c.Sum(new BigRational(1));
    }
}
