using NRedberry.Tensors;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Parser;

public sealed class ParserPowerAstTest
{
    [Fact]
    public void ShouldParsePowerExpression()
    {
        NRedberry.Tensors.Tensor tensor = TensorApi.Parse("Power[a,b]");

        tensor.ShouldBeOfType<Power>();
        TensorUtils.EqualsExactly(tensor, "Power[a,b]").ShouldBeTrue();
    }

    [Fact]
    public void ShouldParseNestedPowerAssociativity()
    {
        TensorUtils.EqualsExactly(TensorApi.Parse("Power[a,Power[b,c]]"), "Power[a,Power[b,c]]").ShouldBeTrue();
        TensorUtils.EqualsExactly(TensorApi.Parse("Power[a,Power[b,c]]"), "Power[a,Power[b,c]]").ShouldBeTrue();
        TensorUtils.EqualsExactly(TensorApi.Parse("Power[a,b*c]"), "Power[a,b*c]").ShouldBeTrue();
    }

    [Fact]
    public void ShouldParsePowerWithProductSuffix()
    {
        TensorUtils.EqualsExactly(TensorApi.Parse("Power[a,b]*c"), "Power[a,b]*c").ShouldBeTrue();
    }

    [Fact]
    public void ShouldParsePowerWithProductPrefix()
    {
        TensorUtils.EqualsExactly(TensorApi.Parse("c*Power[a,b]"), "Power[a,b]*c").ShouldBeTrue();
    }

    [Fact]
    public void ShouldParsePowerWithSumSuffix()
    {
        TensorUtils.EqualsExactly(TensorApi.Parse("Power[a,b]+c"), "Power[a,b]+c").ShouldBeTrue();
    }

    [Fact]
    public void ShouldParsePowerWithSumInExponent()
    {
        TensorUtils.EqualsExactly(TensorApi.Parse("Power[a,b+c]"), "Power[a,b+c]").ShouldBeTrue();
    }

    [Fact]
    public void ShouldParsePowerWithSumPrefix()
    {
        TensorUtils.EqualsExactly(TensorApi.Parse("c+Power[a,b]"), "Power[a,b]+c").ShouldBeTrue();
    }
}
