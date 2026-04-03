using NRedberry.Graphs;

namespace NRedberry.Core.Tests.Graphs;

public sealed class GraphTypeTests
{
    [Fact(DisplayName = "Should define expected graph types")]
    public void ShouldDefineExpectedGraphTypes()
    {
        // Act
        GraphType[] values = Enum.GetValues<GraphType>();

        // Assert
        values.Length.ShouldBe(3);
        values.ShouldContain(GraphType.Cycle);
        values.ShouldContain(GraphType.Line);
        values.ShouldContain(GraphType.Graph);
    }
}
