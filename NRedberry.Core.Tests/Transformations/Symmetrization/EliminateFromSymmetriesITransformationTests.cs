using NRedberry.Transformations.Symmetrization;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Symmetrization;

public sealed class EliminateFromSymmetriesITransformationTests
{
    [Fact]
    public void ShouldCreateCompatibilityWrapper()
    {
        new EliminateFromSymmetriesITransformation().ShouldBeAssignableTo<ITransformation>();
    }
}
