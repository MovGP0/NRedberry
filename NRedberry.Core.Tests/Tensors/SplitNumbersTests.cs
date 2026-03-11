using NRedberry.Numbers;
using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class SplitNumbersTests
{
    [Fact]
    public void ShouldBeUsedForScalarOnlySplitScalars()
    {
        Split split = Split.SplitScalars(TensorFactory.Parse("2*a"));

        Assert.Equal("SplitNumbers", split.GetType().Name);
        Assert.Equal("2*a", split.Factor.ToString(OutputFormat.Redberry));
        Assert.Equal("1", split.Summand.ToString(OutputFormat.Redberry));
        Assert.Equal("ComplexSumBuilder", split.GetBuilder().GetType().Name);
    }

    [Fact]
    public void ShouldBeUsedForIndexlessSplitIndexless()
    {
        Split split = Split.SplitIndexless(TensorFactory.Parse("2*a"));

        Assert.Equal("SplitNumbers", split.GetType().Name);
        Assert.Equal("a", split.Factor.ToString(OutputFormat.Redberry));
        Assert.Equal(new Complex(2), split.Summand);
        Assert.Equal("ComplexSumBuilder", split.GetBuilder().GetType().Name);
    }
}
