using NRedberry.Numbers;
using NRedberry.Tensors;
using NRedberry.Tensors.Functions;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class ArcCotFactoryTests
{
    [Fact]
    public void ShouldSimplifyKnownArcCotInputs()
    {
        NRedberry.Tensors.Tensor argument = TensorApi.Parse("a");

        ArcCotFactory.Factory.Create(new Cot(argument)).ShouldBeSameAs(argument);
        ArcCotFactory.Factory.Create(Complex.Zero).ToString(OutputFormat.Redberry).ShouldBe(TensorApi.Parse("pi/2").ToString(OutputFormat.Redberry));
        ArcCotFactory.Factory.Create(argument).ShouldBeOfType<ArcCot>();
    }
}
