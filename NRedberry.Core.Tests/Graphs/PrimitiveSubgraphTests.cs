using NRedberry.Graphs;
using Xunit;

namespace NRedberry.Core.Tests.Graphs;

public sealed class PrimitiveSubgraphTests
{
    [Fact(DisplayName = "Should validate constructor arguments")]
    public void ShouldValidateConstructorArguments()
    {
        // Act + Assert
        Assert.Throws<ArgumentNullException>(() => _ = new PrimitiveSubgraph(GraphType.Graph, null!));
    }

    [Fact(DisplayName = "Should expose partition and metadata")]
    public void ShouldExposePartitionAndMetadata()
    {
        // Arrange
        int[] partition = [2, 0, 1];
        var subgraph = new PrimitiveSubgraph(GraphType.Cycle, partition);

        // Act
        int[] clone = subgraph.Partition;
        clone[0] = 99;

        // Assert
        Assert.Equal(GraphType.Cycle, subgraph.GraphType);
        Assert.Equal(3, subgraph.Size);
        Assert.Equal(0, subgraph.GetPosition(1));
        Assert.Equal(new[] { 2, 0, 1 }, subgraph.Partition);
        Assert.Equal("Cycle: [2, 0, 1]", subgraph.ToString());
    }
}
