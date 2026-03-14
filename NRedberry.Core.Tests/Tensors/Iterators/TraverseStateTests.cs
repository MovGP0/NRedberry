using NRedberry.Tensors.Iterators;
using Xunit;

namespace NRedberry.Core.Tests.Tensors.Iterators;

public sealed class TraverseStateTests
{
    [Fact]
    public void ShouldExposeExpectedTraversalStates()
    {
        (int)TraverseState.Entering.ShouldBe(0);
        (int)TraverseState.Leaving.ShouldBe(1);
        Enum.GetNames<TraverseState>().ShouldBe(["Entering", "Leaving"]);
    }
}
