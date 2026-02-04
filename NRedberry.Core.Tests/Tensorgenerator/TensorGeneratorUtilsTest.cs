using NRedberry.TensorGenerators;
using TensorType = NRedberry.Tensors.Tensor;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensorgenerator;

public sealed class TensorGeneratorUtilsTest
{
    [Fact]
    public void ShouldGenerateAllStateCombinationsForMetric()
    {
        TensorType[] combinations = TensorGeneratorUtils.AllStatesCombinations(TensorFactory.ParseSimple("g_ab"));
        Assert.Equal(3, combinations.Length);
    }

    [Fact]
    public void ShouldGenerateAllStateCombinationsForTensor()
    {
        TensorType[] combinations = TensorGeneratorUtils.AllStatesCombinations(TensorFactory.ParseSimple("F^a_ab"));
        Assert.Equal(2, combinations.Length);
    }
}
