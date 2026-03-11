using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensor;

public sealed class SplitTest
{
    [Fact]
    public void ShouldSplitScalarsWithSingleTerm()
    {
        Split split = Split.SplitScalars(TensorFactory.Parse("2*a"));

        Assert.Equal("2*a", split.Factor.ToString(OutputFormat.Redberry));
        Assert.Equal("1", split.Summand.ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldSplitScalarsWithTensorFactor()
    {
        Split split = Split.SplitScalars(TensorFactory.Parse("2*a*g_mn"));

        Assert.Equal("g_{mn}", split.Factor.ToString(OutputFormat.Redberry));
        Assert.Equal("2*a", split.Summand.ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldSplitScalarsWithMetricFactor()
    {
        Split split = Split.SplitScalars(TensorFactory.Parse("g^ab*g^cd*g_mn*F_ab*K_cd"));
        string summandText = split.Summand.ToString(OutputFormat.Redberry);

        Assert.Equal("g_{mn}", split.Factor.ToString(OutputFormat.Redberry));
        Assert.Contains("F_{ab}", summandText);
        Assert.Contains("K_{cd}", summandText);
        Assert.Contains("g^{ab}", summandText);
        Assert.Contains("g^{cd}", summandText);
    }

    [Fact]
    public void ShouldProduceMatchingFactorHashCodes()
    {
        Split left = Split.SplitScalars(TensorFactory.Parse("c1*k_b*k^c"));
        Split right = Split.SplitScalars(TensorFactory.Parse("(c0-c0*a**(-1))*k_i*k^i*k_b*k^c"));

        string leftText = left.Factor.ToString(OutputFormat.Redberry);
        string rightText = right.Factor.ToString(OutputFormat.Redberry);

        Assert.Contains("k_{b}", leftText);
        Assert.Contains("k^{c}", leftText);
        Assert.Contains("k_{b}", rightText);
        Assert.Contains("k^{c}", rightText);
    }
}
