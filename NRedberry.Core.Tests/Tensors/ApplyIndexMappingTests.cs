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

        result.ShouldBeSameAs(tensor);
    }

    [Fact]
    public void ShouldReturnSameInstanceWhenRenameDummyTensorHasNoDummies()
    {
        SimpleTensor tensor = CreateSimpleTensor(1, Lower(3), Lower(4));

        TensorType result = ApplyIndexMapping.RenameDummy(tensor, NameWithType(0));

        result.ShouldBeSameAs(tensor);
    }

    [Fact]
    public void ShouldRenameDummyUsingAllowedNames()
    {
        SimpleTensor tensor = CreateSimpleTensor(1, Lower(3), Upper(3));

        TensorType result = ApplyIndexMapping.RenameDummy(tensor, [NameWithType(3)], new int[] { NameWithType(0) });

        result.Indices.AllIndices.ToArray().ShouldBe([Lower(0), Upper(0)]);
    }

    [Fact]
    public void ShouldThrowWhenAllowedNamesDoNotContainRequiredType()
    {
        SimpleTensor tensor = CreateSimpleTensor(1, Lower(3), Upper(3));
        int allowedUpperCaseName = IndicesUtils.GetNameWithType(IndicesUtils.CreateIndex(0, 1, false));

        Should.Throw<IndexOutOfRangeException>(() => ApplyIndexMapping.RenameDummy(tensor, [NameWithType(3)], new int[] { allowedUpperCaseName }));
    }

    [Fact]
    public void ShouldCollectGeneratedNamesWhenRenameDummyUsesAddedSet()
    {
        SimpleTensor tensor = CreateSimpleTensor(1, Lower(3), Upper(3));
        HashSet<int> added = [];

        TensorType result = ApplyIndexMapping.RenameDummy(tensor, [NameWithType(3)], added);

        added.ShouldHaveSingleItem();
        added.SetEquals(TensorUtils.GetAllDummyIndicesT(result)).ShouldBeTrue();
        added.ShouldNotContain(NameWithType(3));
    }

    [Fact]
    public void ShouldReturnSameInstanceWhenOptimizingSimpleTensor()
    {
        SimpleTensor tensor = CreateSimpleTensor(1, Lower(3), Lower(4));

        TensorType result = ApplyIndexMapping.OptimizeDummies(tensor);

        result.ShouldBeSameAs(tensor);
    }

    [Fact]
    public void ShouldCanonicalizeDummiesAcrossSummandsWhenOptimizing()
    {
        SimpleTensor first = CreateSimpleTensor(1, Lower(3), Upper(3));
        SimpleTensor second = CreateSimpleTensor(2, Lower(7), Upper(7));
        Sum tensor = new([first, second], IndicesFactory.EmptyIndices);

        TensorType result = ApplyIndexMapping.OptimizeDummies(tensor);

        Sum optimized = result.ShouldBeOfType<Sum>();
        SimpleTensor optimizedFirst = optimized[0].ShouldBeOfType<SimpleTensor>();
        SimpleTensor optimizedSecond = optimized[1].ShouldBeOfType<SimpleTensor>();
        optimizedFirst.Indices.AllIndices.ToArray().ShouldBe([Lower(7), Upper(7)]);
        optimizedSecond.Indices.AllIndices.ToArray().ShouldBe([Lower(7), Upper(7)]);
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
