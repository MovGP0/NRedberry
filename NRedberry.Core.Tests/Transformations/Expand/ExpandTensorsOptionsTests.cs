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

        options.LeaveScalars.ShouldBeFalse();
        options.Simplifications.ShouldBeSameAs(Transformation.Identity);
        options.TraverseGuide.ShouldNotBeNull();
    }
}
