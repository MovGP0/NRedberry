using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class UnaryFunctorTests
{
    [Fact]
    public void ShouldEvaluateViaUnaryFunctorContract()
    {
        UnaryFunctor<BigRational, BigRational> functor = new IncrementUnaryFunctor();

        functor.Eval(new BigRational(4)).ShouldBe(new BigRational(5));
    }
}

file sealed class IncrementUnaryFunctor : UnaryFunctor<BigRational, BigRational>
{
    public BigRational Eval(BigRational c)
    {
        return c.Sum(new BigRational(1));
    }
}
