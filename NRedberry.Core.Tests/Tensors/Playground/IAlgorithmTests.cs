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

        algorithm.Name.ShouldBe("Test algorithm");
        result.Data.ShouldHaveSingleItem();
        (algorithm.Timing >= 0).ShouldBeTrue();
        (algorithm.TimingMillis() >= 0).ShouldBeTrue();
    }

    [Fact]
    public void ShouldValidateInputAndResetTiming()
    {
        TestAlgorithm algorithm = new();

        Should.Throw<ArgumentNullException>(() => algorithm.Calc(null!));

        _ = algorithm.Calc(TensorApi.Parse("a"));
        (algorithm.Timing >= 0).ShouldBeTrue();

        algorithm.Restart();

        algorithm.Timing.ShouldBe(0);
    }

    private sealed class TestAlgorithm() : IAlgorithm("Test algorithm")
    {
        protected override ProductData CalcCore(NRedberry.Tensors.Tensor tensor)
        {
            return new ProductData([tensor], tensor.Indices);
        }
    }
}
