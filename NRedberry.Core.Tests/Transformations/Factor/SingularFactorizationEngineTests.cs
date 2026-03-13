using System.Reflection;
using NRedberry.Transformations.Factor;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Factor;

public sealed class SingularFactorizationEngineTests
{
    [Fact]
    public void ShouldRejectNullExecutablePath()
    {
        Assert.Throws<ArgumentNullException>(() => new SingularFactorizationEngine(null!));
    }

    [Fact]
    public void ShouldCreateSingularRingDefinition()
    {
        MethodInfo method = typeof(SingularFactorizationEngine).GetMethod(
            "CreateRing",
            BindingFlags.NonPublic | BindingFlags.Static)!;
        object[] parameters = [new NRedberry.Tensors.Tensor[] { TensorApi.Parse("a"), TensorApi.Parse("b") }];

        string actual = Assert.IsType<string>(method.Invoke(null, parameters));

        Assert.Equal("ring r = 0,(a,b),dp;", actual);
    }
}
