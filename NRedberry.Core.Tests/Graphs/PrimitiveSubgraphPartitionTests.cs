using NRedberry.Graphs;
using NRedberry.Indices;
using NRedberry.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Graphs;

public sealed class PrimitiveSubgraphPartitionTests
{
    [Fact(DisplayName = "Should calculate partition for simple contraction")]
    public void ShouldCalculatePartitionForSimpleContraction()
    {
        // Arrange
        byte type = IndexType.LatinLower.GetType_();
        int upper = IndicesUtils.CreateIndex(0, type, true);
        int lower = IndicesUtils.CreateIndex(0, type, false);

        SimpleTensor left = new SimpleTensor(1, IndicesFactory.CreateSimple(null, upper));
        SimpleTensor right = new SimpleTensor(2, IndicesFactory.CreateSimple(null, lower));
        var product = (Product)NRedberry.Tensors.Tensors.Multiply(left, right);

        // Act
        PrimitiveSubgraph[] partition = PrimitiveSubgraphPartition.CalculatePartition(product, IndexType.LatinLower);

        // Assert
        Assert.Single(partition);
        Assert.Equal(GraphType.Line, partition[0].GraphType);
        Assert.Contains(0, partition[0].Partition);
        Assert.Contains(1, partition[0].Partition);
        Assert.Equal(2, partition[0].Partition.Length);
    }
}
