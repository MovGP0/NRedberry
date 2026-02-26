using NRedberry.Graphs;
using Xunit;

namespace NRedberry.Core.Tests.Graphs;

public sealed class GraphUtilsTests
{
    [Fact(DisplayName = "Should compute connected components for empty edges")]
    public void ShouldComputeConnectedComponentsForEmptyEdges()
    {
        // Arrange
        const int vertices = 3;

        // Act
        int[] components = GraphUtils.CalculateConnectedComponents([], [], vertices);

        // Assert
        Assert.Equal(new[] { 0, 1, 2, 3 }, components);
    }

    [Fact(DisplayName = "Should compute connected components for simple graph")]
    public void ShouldComputeConnectedComponentsForSimpleGraph()
    {
        // Arrange
        const int vertices = 3;
        int[] from = [0];
        int[] to = [1];

        // Act
        int[] components = GraphUtils.CalculateConnectedComponents(from, to, vertices);

        // Assert
        Assert.Equal(0, components[0]);
        Assert.Equal(0, components[1]);
        Assert.Equal(1, components[2]);
        Assert.Equal(2, components[3]);
    }

    [Fact(DisplayName = "Should calculate component sizes")]
    public void ShouldCalculateComponentSizes()
    {
        // Arrange
        int[] components = [0, 0, 1, 2];

        // Act + Assert
        Assert.Equal(2, GraphUtils.ComponentSize(0, components));
        Assert.Equal(1, GraphUtils.ComponentSize(2, components));
    }

    [Fact(DisplayName = "Should throw for inconsistent arrays and vertices")]
    public void ShouldThrowForInconsistentArraysAndVertices()
    {
        // Act + Assert
        Assert.Throws<ArgumentException>(() => GraphUtils.CalculateConnectedComponents([0], [1, 2], 3));
        Assert.Throws<IndexOutOfRangeException>(() => GraphUtils.ComponentSize(5, [0, 1, 2]));
    }
}
