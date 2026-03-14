using NRedberry.Transformations.Substitutions;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Substitutions;

public sealed class SumBijectionPortTests
{
    [Fact]
    public void ShouldThrowWhileSumBijectionPortIsUnimplemented()
    {
        Should.Throw<NotImplementedException>(() =>
            new SumBijectionPort(TensorApi.Parse("a+b"), TensorApi.Parse("a+b")));
    }
}
