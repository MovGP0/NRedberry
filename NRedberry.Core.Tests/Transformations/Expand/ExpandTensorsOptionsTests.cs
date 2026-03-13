using NRedberry.Transformations;
using NRedberry.Transformations.Expand;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Expand;

public sealed class ExpandTensorsOptionsTests
{
    [Fact]
    public void ShouldInheritExpandOptionsDefaults()
    {
        ExpandTensorsOptions options = new();

        Assert.False(options.LeaveScalars);
        Assert.Same(Transformation.Identity, options.Simplifications);
        Assert.NotNull(options.TraverseGuide);
    }
}
