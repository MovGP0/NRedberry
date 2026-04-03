using NRedberry.Tensors.Iterators;
using NRedberry.Transformations;
using NRedberry.Transformations.Expand;

namespace NRedberry.Core.Tests.Transformations.Expand;

public sealed class ExpandOptionsTests
{
    [Fact]
    public void ShouldInitializeDefaults()
    {
        ExpandOptions options = new();

        options.Simplifications.ShouldBeSameAs(Transformation.Identity);
        options.TraverseGuide.ShouldNotBeNull();
        options.TraverseGuide!.GetPermission(
            NRedberry.Tensors.Tensors.Parse("Sin[a]"),
            NRedberry.Tensors.Tensors.Parse("x"),
            0)
            .ShouldBe(TraversePermission.DontShow);
    }
}
