using System.Numerics;
using Xunit;

namespace NRedberry.Apache.Commons.Math.Tests;

public sealed class IFieldTests
{
    [Fact(DisplayName = "Should expose identities through interface")]
    public void ShouldExposeIdentitiesThroughInterface()
    {
        IField<BigFraction> field = new BigFractionField();

        field.Zero.Numerator.ShouldBe(BigInteger.Zero);
        field.Zero.Denominator.ShouldBe(BigInteger.One);
        field.One.Numerator.ShouldBe(BigInteger.One);
        field.One.Denominator.ShouldBe(BigInteger.One);
    }

    [Fact(DisplayName = "Should provide runtime class through interface")]
    public void ShouldProvideRuntimeClassThroughInterface()
    {
        IField<BigFraction> field = new BigFractionField();

        field.GetRuntimeClass().ShouldBe(typeof(BigFraction));
    }
}
