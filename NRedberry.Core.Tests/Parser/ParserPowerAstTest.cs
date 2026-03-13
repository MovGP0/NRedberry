using NRedberry.Tensors;
using Xunit;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Parser;

public sealed class ParserPowerAstTest
{
    [Fact]
    public void ShouldParsePowerExpression()
    {
        NRedberry.Tensors.Tensor tensor = TensorApi.Parse("Power[a,b]");

        Assert.IsType<Power>(tensor);
        Assert.True(TensorUtils.EqualsExactly(tensor, "Power[a,b]"));
    }

    [Fact]
    public void ShouldParseNestedPowerAssociativity()
    {
        Assert.True(TensorUtils.EqualsExactly(TensorApi.Parse("Power[a,Power[b,c]]"), "Power[a,Power[b,c]]"));
        Assert.True(TensorUtils.EqualsExactly(TensorApi.Parse("Power[a,Power[b,c]]"), "Power[a,Power[b,c]]"));
        Assert.True(TensorUtils.EqualsExactly(TensorApi.Parse("Power[a,b*c]"), "Power[a,b*c]"));
    }

    [Fact]
    public void ShouldParsePowerWithProductSuffix()
    {
        Assert.True(TensorUtils.EqualsExactly(TensorApi.Parse("Power[a,b]*c"), "Power[a,b]*c"));
    }

    [Fact]
    public void ShouldParsePowerWithProductPrefix()
    {
        Assert.True(TensorUtils.EqualsExactly(TensorApi.Parse("c*Power[a,b]"), "Power[a,b]*c"));
    }

    [Fact]
    public void ShouldParsePowerWithSumSuffix()
    {
        Assert.True(TensorUtils.EqualsExactly(TensorApi.Parse("Power[a,b]+c"), "Power[a,b]+c"));
    }

    [Fact]
    public void ShouldParsePowerWithSumInExponent()
    {
        Assert.True(TensorUtils.EqualsExactly(TensorApi.Parse("Power[a,b+c]"), "Power[a,b+c]"));
    }

    [Fact]
    public void ShouldParsePowerWithSumPrefix()
    {
        Assert.True(TensorUtils.EqualsExactly(TensorApi.Parse("c+Power[a,b]"), "Power[a,b]+c"));
    }
}
