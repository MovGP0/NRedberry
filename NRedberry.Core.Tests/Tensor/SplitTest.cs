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

        split.Factor.ToString(OutputFormat.Redberry).ShouldBe("2*a");
        split.Summand.ToString(OutputFormat.Redberry).ShouldBe("1");
    }

    [Fact]
    public void ShouldSplitScalarsWithTensorFactor()
    {
        Split split = Split.SplitScalars(TensorFactory.Parse("2*a*g_mn"));

        split.Factor.ToString(OutputFormat.Redberry).ShouldBe("g_{mn}");
        split.Summand.ToString(OutputFormat.Redberry).ShouldBe("2*a");
    }

    [Fact]
    public void ShouldSplitScalarsWithMetricFactor()
    {
        Split split = Split.SplitScalars(TensorFactory.Parse("g^ab*g^cd*g_mn*F_ab*K_cd"));
        SimpleTensor factor = split.Factor.ShouldBeOfType<SimpleTensor>();
        Product summand = split.Summand.ShouldBeOfType<Product>();

        factor.Indices.GetFree().Size().ShouldBe(2);
        summand.Size.ShouldBe(4);
        summand.Indices.GetFree().Size().ShouldBe(0);
    }

    [Fact]
    public void ShouldProduceEquivalentFactorsWithMatchingFreeIndices()
    {
        Product left = Split.SplitScalars(TensorFactory.Parse("c1*k_b*k^c")).Factor.ShouldBeOfType<Product>();
        Product right = Split.SplitScalars(TensorFactory.Parse("(c0-c0*a**(-1))*k_i*k^i*k_b*k^c")).Factor.ShouldBeOfType<Product>();

        left.Size.ShouldBe(2);
        right.Size.ShouldBe(2);
        GetVarianceSignature(right).ShouldBe(GetVarianceSignature(left));
        left.Indices.GetFree().EqualsRegardlessOrder(right.Indices.GetFree()).ShouldBeTrue();
    }

    private static string GetVarianceSignature(Product product)
    {
        List<string> signature = [];

        foreach (TensorType factor in product)
        {
            SimpleTensor simpleTensor = factor.ShouldBeOfType<SimpleTensor>();
            int freeIndex = simpleTensor.Indices.GetFree()[0];
            signature.Add($"{IndicesUtils.GetStateInt(freeIndex)}");
        }

        signature.Sort(StringComparer.Ordinal);
        return string.Join("|", signature);
    }
}
