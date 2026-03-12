using NRedberry.Numbers;
using NRedberry.Tensors;
using NRedberry.Tensors.Functions;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class ExpFactoryTests
{
    [Fact]
    public void ShouldSimplifyKnownExpInputs()
    {
        NRedberry.Tensors.Tensor argument = TensorApi.Parse("a");

        Assert.Same(argument, ExpFactory.Factory.Create(new Log(argument)));
        Assert.Equal(Complex.One, ExpFactory.Factory.Create(Complex.Zero));
        Assert.IsType<Exp>(ExpFactory.Factory.Create(argument));
    }
}
