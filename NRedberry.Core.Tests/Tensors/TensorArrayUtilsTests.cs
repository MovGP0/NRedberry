using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

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

        Assert.Equal(["a", "b", "c", "d"], result.Select(t => t.ToString(OutputFormat.Redberry)).ToArray());
    }

    [Fact]
    public void ShouldRemoveItemAtIndex()
    {
        NRedberry.Tensors.Tensor[] result = TensorArrayUtils.Remove(
            [TensorFactory.Parse("a"), TensorFactory.Parse("b"), TensorFactory.Parse("c")],
            1);

        Assert.Equal(["a", "c"], result.Select(t => t.ToString(OutputFormat.Redberry)).ToArray());
    }
}
