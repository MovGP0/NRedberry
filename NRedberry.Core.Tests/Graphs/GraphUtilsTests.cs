using NRedberry.Graphs;

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
        components.ShouldBe([0, 1, 2, 3]);
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
        components[0].ShouldBe(0);
        components[1].ShouldBe(0);
        components[2].ShouldBe(1);
        components[3].ShouldBe(2);
    }

    [Fact(DisplayName = "Should calculate component sizes")]
    public void ShouldCalculateComponentSizes()
    {
        // Arrange
        int[] components = [0, 0, 1, 2];

        // Act + Assert
        GraphUtils.ComponentSize(0, components).ShouldBe(2);
        GraphUtils.ComponentSize(2, components).ShouldBe(1);
    }

    [Fact(DisplayName = "Should throw for inconsistent arrays and vertices")]
    public void ShouldThrowForInconsistentArraysAndVertices()
    {
        // Act + Assert
        Should.Throw<ArgumentException>(() => GraphUtils.CalculateConnectedComponents([0], [1, 2], 3));
        Should.Throw<IndexOutOfRangeException>(() => GraphUtils.ComponentSize(5, [0, 1, 2]));
    }
}
