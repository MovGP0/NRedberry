using NRedberry.Tensors.Iterators;
using NRedberry.Transformations;
using NRedberry.Transformations.Expand;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Expand;

public sealed class ExpandOptionsTests
{
    [Fact]
    public void ShouldInitializeDefaults()
    {
        ExpandOptions options = new();

        Assert.Same(Transformation.Identity, options.Simplifications);
        Assert.NotNull(options.TraverseGuide);
        Assert.Equal(
            TraversePermission.DontShow,
            options.TraverseGuide!.GetPermission(
                NRedberry.Tensors.Tensors.Parse("Sin[a]"),
                NRedberry.Tensors.Tensors.Parse("x"),
                0));
    }
}
