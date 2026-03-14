using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ps;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class TaylorFunctionTests
{
    [Fact]
    public void ShouldExposeDerivativeEvaluationAndZeroContracts()
    {
        StubTaylorFunction secondDerivative = new(true, new BigRational(0));
        StubTaylorFunction firstDerivative = new(false, new BigRational(5), secondDerivative);
        StubTaylorFunction function = new(false, new BigRational(3), firstDerivative);

        TaylorFunction<BigRational> derivative = function.Deriviative();

        function.IsZERO().ShouldBeFalse();
        function.Evaluate(new BigRational(10)).ToString().ShouldBe("3");
        derivative.ShouldBeSameAs(firstDerivative);
        derivative.Evaluate(new BigRational(2)).ToString().ShouldBe("5");
        derivative.Deriviative().IsZERO().ShouldBeTrue();
    }
}

file sealed class StubTaylorFunction : TaylorFunction<BigRational>
{
    private readonly TaylorFunction<BigRational> _derivative;
    private readonly bool _isZero;
    private readonly BigRational _value;

    public StubTaylorFunction(bool isZero, BigRational value, TaylorFunction<BigRational>? derivative = null)
    {
        _isZero = isZero;
        _value = value;
        _derivative = derivative ?? this;
    }

    public bool IsZERO()
    {
        return _isZero;
    }

    public TaylorFunction<BigRational> Deriviative()
    {
        return _derivative;
    }

    public BigRational Evaluate(BigRational a)
    {
        return _value;
    }
}
