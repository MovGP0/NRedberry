using NRedberry.Transformations.Symmetrization;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Symmetrization;

public sealed class EliminateDueSymmetriesTransformationTests
{
    [Fact]
    public void ShouldExposeSingletonInstance()
    {
        EliminateDueSymmetriesTransformation.Instance.ShouldNotBeNull();
    }
}
