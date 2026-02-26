using NRedberry.Graphs;
using Xunit;

namespace NRedberry.Core.Tests.Graphs;

public sealed class GraphTypeTests
{
    [Fact(DisplayName = "Should define expected graph types")]
    public void ShouldDefineExpectedGraphTypes()
    {
        // Act
        GraphType[] values = Enum.GetValues<GraphType>();

        // Assert
        Assert.Equal(3, values.Length);
        Assert.Contains(GraphType.Cycle, values);
        Assert.Contains(GraphType.Line, values);
        Assert.Contains(GraphType.Graph, values);
    }
}
