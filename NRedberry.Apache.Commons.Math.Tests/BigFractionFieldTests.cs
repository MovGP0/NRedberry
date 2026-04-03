using System.Numerics;
using Xunit;

namespace NRedberry.Apache.Commons.Math.Tests;

public sealed class BigFractionFieldTests
{
    [Fact(DisplayName = "Should expose zero and one")]
    public void ShouldExposeZeroAndOne()
    {
        var field = new BigFractionField();

        field.Zero.Numerator.ShouldBe(BigInteger.Zero);
        field.Zero.Denominator.ShouldBe(BigInteger.One);
        field.One.Numerator.ShouldBe(BigInteger.One);
        field.One.Denominator.ShouldBe(BigInteger.One);
    }

    [Fact(DisplayName = "Should return runtime class")]
    public void ShouldReturnRuntimeClass()
    {
        var field = new BigFractionField();

        field.GetRuntimeClass().ShouldBe(typeof(BigFraction));
    }
}
