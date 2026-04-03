using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Tensors;

public sealed class TensorArrayUtilsTests
{
    [Fact]
    public void ShouldAppendArrays()
    {
        NRedberry.Tensors.Tensor[] result = TensorArrayUtils.AddAll(
            [TensorFactory.Parse("a"), TensorFactory.Parse("b")],
            TensorFactory.Parse("c"),
            TensorFactory.Parse("d"));

        result.Select(t => t.ToString(OutputFormat.Redberry)).ToArray().ShouldBe(["a", "b", "c", "d"]);
    }

    [Fact]
    public void ShouldRemoveItemAtIndex()
    {
        NRedberry.Tensors.Tensor[] result = TensorArrayUtils.Remove(
            [TensorFactory.Parse("a"), TensorFactory.Parse("b"), TensorFactory.Parse("c")],
            1);

        result.Select(t => t.ToString(OutputFormat.Redberry)).ToArray().ShouldBe(["a", "c"]);
    }
}
