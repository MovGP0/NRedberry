using NRedberry.Indices;
using NRedberry.Tensors;
using TensorType = NRedberry.Tensors.Tensor;
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
        SimpleTensor factor = Assert.IsType<SimpleTensor>(split.Factor);
        Product summand = Assert.IsType<Product>(split.Summand);

        Assert.Equal(2, factor.Indices.GetFree().Size());
        Assert.Equal(4, summand.Size);
        Assert.Equal(0, summand.Indices.GetFree().Size());
    }

    [Fact]
    public void ShouldProduceEquivalentFactorsWithMatchingFreeIndices()
    {
        Product left = Assert.IsType<Product>(Split.SplitScalars(TensorFactory.Parse("c1*k_b*k^c")).Factor);
        Product right = Assert.IsType<Product>(Split.SplitScalars(TensorFactory.Parse("(c0-c0*a**(-1))*k_i*k^i*k_b*k^c")).Factor);

        Assert.Equal(2, left.Size);
        Assert.Equal(2, right.Size);
        Assert.Equal(GetVarianceSignature(left), GetVarianceSignature(right));
        Assert.True(left.Indices.GetFree().EqualsRegardlessOrder(right.Indices.GetFree()));
    }

    private static string GetVarianceSignature(Product product)
    {
        List<string> signature = [];

        foreach (TensorType factor in product)
        {
            SimpleTensor simpleTensor = Assert.IsType<SimpleTensor>(factor);
            int freeIndex = simpleTensor.Indices.GetFree()[0];
            signature.Add($"{IndicesUtils.GetStateInt(freeIndex)}");
        }

        signature.Sort(StringComparer.Ordinal);
        return string.Join("|", signature);
    }
}
