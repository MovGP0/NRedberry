using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Core.Tests.Transformations.Symmetrization;

public sealed class EliminateFromSymmetriesITransformationTests
{
    [Fact]
    public void ShouldCreateCompatibilityWrapper()
    {
        new EliminateFromSymmetriesITransformation().ShouldBeAssignableTo<ITransformation>();
    }
}
