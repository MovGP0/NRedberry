using NRedberry.Graphs;

namespace NRedberry.Core.Tests.Graphs;

public sealed class PrimitiveSubgraphTests
{
    [Fact(DisplayName = "Should validate constructor arguments")]
    public void ShouldValidateConstructorArguments()
    {
        // Act + Assert
        Should.Throw<ArgumentNullException>(() => _ = new PrimitiveSubgraph(GraphType.Graph, null!));
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
        subgraph.GraphType.ShouldBe(GraphType.Cycle);
        subgraph.Size.ShouldBe(3);
        subgraph.GetPosition(1).ShouldBe(0);
        subgraph.Partition.ShouldBe([2, 0, 1]);
        subgraph.ToString().ShouldBe("Cycle: [2, 0, 1]");
    }
}
