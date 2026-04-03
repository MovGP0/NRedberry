using NRedberry.Numbers;
using NRedberry.Tensors.Functions;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Tensors;

public sealed class ExpFactoryTests
{
    [Fact]
    public void ShouldSimplifyKnownExpInputs()
    {
        NRedberry.Tensors.Tensor argument = TensorApi.Parse("a");

        ExpFactory.Factory.Create(new Log(argument)).ShouldBeSameAs(argument);
        ExpFactory.Factory.Create(Complex.Zero).ShouldBe(Complex.One);
        ExpFactory.Factory.Create(argument).ShouldBeOfType<Exp>();
    }
}
