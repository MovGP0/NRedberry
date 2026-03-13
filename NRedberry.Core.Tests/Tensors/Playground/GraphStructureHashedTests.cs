using NRedberry.Tensors.Playground;
using Xunit;

namespace NRedberry.Core.Tests.Tensors.Playground;

public sealed class GraphStructureHashedTests
{
    [Fact]
    public void ShouldExposeEmptySingleton()
    {
        GraphStructureHashed empty = GraphStructureHashed.EmptyInstance;

        Assert.Empty(empty.StretchIndices);
        Assert.Empty(empty.FreeContraction);
        Assert.Empty(empty.Contractions);
    }

    [Fact]
    public void ShouldStoreConstructorArguments()
    {
        short[] stretchIndices = [1, 2];
        long[] freeContraction = [3L];
        long[][] contractions = [[4L, 5L]];

        GraphStructureHashed graphStructureHashed = new(stretchIndices, freeContraction, contractions);

        Assert.Same(stretchIndices, graphStructureHashed.StretchIndices);
        Assert.Same(freeContraction, graphStructureHashed.FreeContraction);
        Assert.Same(contractions, graphStructureHashed.Contractions);
    }
}
