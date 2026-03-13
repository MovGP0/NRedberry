using NRedberry.Tensors.Iterators;
using Xunit;

namespace NRedberry.Core.Tests.Tensors.Iterators;

public sealed class TraverseStateTests
{
    [Fact]
    public void ShouldExposeExpectedTraversalStates()
    {
        Assert.Equal(0, (int)TraverseState.Entering);
        Assert.Equal(1, (int)TraverseState.Leaving);
        Assert.Equal(
            ["Entering", "Leaving"],
            Enum.GetNames<TraverseState>());
    }
}
