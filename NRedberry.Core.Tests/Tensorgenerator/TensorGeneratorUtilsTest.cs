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
        combinations.Length.ShouldBe(3);
    }

    [Fact]
    public void ShouldGenerateAllStateCombinationsForTensor()
    {
        TensorType[] combinations = TensorGeneratorUtils.AllStatesCombinations(TensorFactory.ParseSimple("F^a_ab"));
        combinations.Length.ShouldBe(2);
    }
}
