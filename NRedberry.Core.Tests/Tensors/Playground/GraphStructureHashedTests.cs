using NRedberry.Tensors.Playground;
using Xunit;

namespace NRedberry.Core.Tests.Tensors.Playground;

public sealed class GraphStructureHashedTests
{
    [Fact]
    public void ShouldExposeEmptySingleton()
    {
        GraphStructureHashed empty = GraphStructureHashed.EmptyInstance;

        empty.StretchIndices.ShouldBeEmpty();
        empty.FreeContraction.ShouldBeEmpty();
        empty.Contractions.ShouldBeEmpty();
    }

    [Fact]
    public void ShouldStoreConstructorArguments()
    {
        short[] stretchIndices = [1, 2];
        long[] freeContraction = [3L];
        long[][] contractions = [[4L, 5L]];

        GraphStructureHashed graphStructureHashed = new(stretchIndices, freeContraction, contractions);

        graphStructureHashed.StretchIndices.ShouldBeSameAs(stretchIndices);
        graphStructureHashed.FreeContraction.ShouldBeSameAs(freeContraction);
        graphStructureHashed.Contractions.ShouldBeSameAs(contractions);
    }
}
