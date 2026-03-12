using NRedberry.Numbers;
using NRedberry.Tensors;
using NRedberry.Tensors.Functions;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class LogFactoryTests
{
    [Fact]
    public void ShouldSimplifyKnownLogInputs()
    {
        NRedberry.Tensors.Tensor argument = TensorApi.Parse("a");

        Assert.Same(argument, LogFactory.Factory.Create(new Exp(argument)));
        Assert.Equal(Complex.Zero, LogFactory.Factory.Create(Complex.One));
        Assert.IsType<Log>(LogFactory.Factory.Create(argument));
    }
}
