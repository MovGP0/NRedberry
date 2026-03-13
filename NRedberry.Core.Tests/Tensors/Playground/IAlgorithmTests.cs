using NRedberry.Tensors.Playground;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors.Playground;

public sealed class IAlgorithmTests
{
    [Fact]
    public void ShouldExposeNameAndAccumulateTiming()
    {
        TestAlgorithm algorithm = new();

        ProductData result = algorithm.Calc(TensorApi.Parse("a"));

        Assert.Equal("Test algorithm", algorithm.Name);
        Assert.Single(result.Data);
        Assert.True(algorithm.Timing >= 0);
        Assert.True(algorithm.TimingMillis() >= 0);
    }

    [Fact]
    public void ShouldValidateInputAndResetTiming()
    {
        TestAlgorithm algorithm = new();

        Assert.Throws<ArgumentNullException>(() => algorithm.Calc(null!));

        _ = algorithm.Calc(TensorApi.Parse("a"));
        Assert.True(algorithm.Timing >= 0);

        algorithm.Restart();

        Assert.Equal(0, algorithm.Timing);
    }

    private sealed class TestAlgorithm() : IAlgorithm("Test algorithm")
    {
        protected override ProductData CalcCore(NRedberry.Tensors.Tensor tensor)
        {
            return new ProductData([tensor], tensor.Indices);
        }
    }
}
