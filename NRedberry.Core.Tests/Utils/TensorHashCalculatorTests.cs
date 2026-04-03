using NRedberry.Indices;
using NRedberry.Tensors;
using NRedberry.Utils;

namespace NRedberry.Core.Tests.Utils;

public sealed class TensorHashCalculatorTests
{
    [Fact]
    public void ShouldTreatExplicitIndexSetsIndependentOfInputOrder()
    {
        SimpleTensor tensor = CreateSimpleTensor(
            IndicesUtils.CreateIndex(1, IndexType.Matrix1, true),
            IndicesUtils.CreateIndex(2, IndexType.Matrix1, false));
        int firstIndex = tensor.Indices[0];
        int secondIndex = tensor.Indices[1];

        int sortedHash = TensorHashCalculator.HashWithIndices(tensor, [firstIndex, secondIndex]);
        int unsortedHash = TensorHashCalculator.HashWithIndices(tensor, [secondIndex, firstIndex]);

        unsortedHash.ShouldBe(sortedHash);
    }

    [Fact]
    public void ShouldReturnTensorHashWhenNoIndicesAreProvided()
    {
        NRedberry.Tensors.Tensor tensor = NRedberry.Tensors.Tensors.Parse("a+b");

        TensorHashCalculator.HashWithIndices(tensor, []).ShouldBe(tensor.GetHashCode());
    }

    [Fact]
    public void ShouldUseSimpleTensorHashForNonTopologicalHashLeaf()
    {
        SimpleTensor tensor = CreateSimpleTensor(IndicesUtils.CreateIndex(5, IndexType.Matrix1, true));

        TensorHashCalculator.NonTopologicalHash(tensor).ShouldBe(tensor.GetHashCode());
    }

    private static SimpleTensor CreateSimpleTensor(params int[] indices)
    {
        return new SimpleTensor(1, IndicesFactory.CreateSimple(null, indices));
    }
}
