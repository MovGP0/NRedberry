using NRedberry.Transformations.Substitutions;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Substitutions;

public sealed class BijectionContainerTests
{
    [Fact]
    public void ShouldThrowWhileBijectionContainerIsUnimplemented()
    {
        Assert.Throws<NotImplementedException>(() => new BijectionContainer(null!, []));
    }
}
