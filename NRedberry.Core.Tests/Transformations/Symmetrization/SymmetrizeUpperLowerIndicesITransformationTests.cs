using NRedberry.Transformations.Symmetrization;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Symmetrization;

public sealed class SymmetrizeUpperLowerIndicesITransformationTests
{
    [Fact]
    public void ShouldExposeSingleton()
    {
        SymmetrizeUpperLowerIndicesITransformation.Instance.ShouldNotBeNull();
    }
}
