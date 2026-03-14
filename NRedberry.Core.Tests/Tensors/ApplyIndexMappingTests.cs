using System.Collections.Generic;
using System.Linq;
using NRedberry.Indices;
using NRedberry.Tensors;
using TensorType = NRedberry.Tensors.Tensor;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class ApplyIndexMappingTests
{
    [Fact]
    public void ShouldReturnSameInstanceWhenRenameDummyHasNoForbiddenNames()
    {
        SimpleTensor tensor = CreateSimpleTensor(1, Lower(3), Upper(3));

        TensorType result = ApplyIndexMapping.RenameDummy(tensor);

        Assert.Same(tensor, result);
    }

    [Fact]
    public void ShouldReturnSameInstanceWhenRenameDummyTensorHasNoDummies()
    {
        SimpleTensor tensor = CreateSimpleTensor(1, Lower(3), Lower(4));

        TensorType result = ApplyIndexMapping.RenameDummy(tensor, NameWithType(0));

        Assert.Same(tensor, result);
    }

    [Fact]
    public void ShouldRenameDummyUsingAllowedNames()
    {
        SimpleTensor tensor = CreateSimpleTensor(1, Lower(3), Upper(3));

        TensorType result = ApplyIndexMapping.RenameDummy(tensor, [NameWithType(3)], new int[] { NameWithType(0) });

        Assert.Equal([Lower(0), Upper(0)], result.Indices.AllIndices.ToArray());
    }

    [Fact]
    public void ShouldThrowWhenAllowedNamesDoNotContainRequiredType()
    {
        SimpleTensor tensor = CreateSimpleTensor(1, Lower(3), Upper(3));
        int allowedUpperCaseName = IndicesUtils.GetNameWithType(IndicesUtils.CreateIndex(0, 1, false));

        Assert.Throws<IndexOutOfRangeException>(
            () => ApplyIndexMapping.RenameDummy(tensor, [NameWithType(3)], new int[] { allowedUpperCaseName }));
    }

    [Fact]
    public void ShouldCollectGeneratedNamesWhenRenameDummyUsesAddedSet()
    {
        SimpleTensor tensor = CreateSimpleTensor(1, Lower(3), Upper(3));
        HashSet<int> added = [];

        TensorType result = ApplyIndexMapping.RenameDummy(tensor, [NameWithType(3)], added);

        Assert.Single(added);
        Assert.True(added.SetEquals(TensorUtils.GetAllDummyIndicesT(result)));
        Assert.DoesNotContain(NameWithType(3), added);
    }

    [Fact]
    public void ShouldReturnSameInstanceWhenOptimizingSimpleTensor()
    {
        SimpleTensor tensor = CreateSimpleTensor(1, Lower(3), Lower(4));

        TensorType result = ApplyIndexMapping.OptimizeDummies(tensor);

        Assert.Same(tensor, result);
    }

    [Fact]
    public void ShouldCanonicalizeDummiesAcrossSummandsWhenOptimizing()
    {
        SimpleTensor first = CreateSimpleTensor(1, Lower(3), Upper(3));
        SimpleTensor second = CreateSimpleTensor(2, Lower(7), Upper(7));
        Sum tensor = new([first, second], IndicesFactory.EmptyIndices);

        TensorType result = ApplyIndexMapping.OptimizeDummies(tensor);

        Sum optimized = Assert.IsType<Sum>(result);
        SimpleTensor optimizedFirst = Assert.IsType<SimpleTensor>(optimized[0]);
        SimpleTensor optimizedSecond = Assert.IsType<SimpleTensor>(optimized[1]);
        Assert.Equal([Lower(7), Upper(7)], optimizedFirst.Indices.AllIndices.ToArray());
        Assert.Equal([Lower(7), Upper(7)], optimizedSecond.Indices.AllIndices.ToArray());
    }

    private static SimpleTensor CreateSimpleTensor(int name, params int[] indices)
    {
        return new SimpleTensor(name, IndicesFactory.CreateSimple(null, indices));
    }

    private static int Lower(int name)
    {
        return IndicesUtils.CreateIndex(name, (byte)0, false);
    }

    private static int Upper(int name)
    {
        return IndicesUtils.CreateIndex(name, (byte)0, true);
    }

    private static int NameWithType(int name)
    {
        return IndicesUtils.GetNameWithType(Lower(name));
    }
}
