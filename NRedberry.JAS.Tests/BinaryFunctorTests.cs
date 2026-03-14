using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;
using Shouldly;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class BinaryFunctorTests
{
    [Fact]
    public void ShouldEvaluateBinaryFunctorsAgainstElementInputs()
    {
        BinaryFunctor<BigRational, BigRational, BigRational> functor = new AddBinaryFunctor();

        BigRational result = functor.Eval(new BigRational(2), new BigRational(3, 2));

        result.ToString().ShouldBe("7/2");
    }
}

file sealed class AddBinaryFunctor : BinaryFunctor<BigRational, BigRational, BigRational>
{
    public BigRational Eval(BigRational c1, BigRational c2)
    {
        return c1.Sum(c2);
    }
}
