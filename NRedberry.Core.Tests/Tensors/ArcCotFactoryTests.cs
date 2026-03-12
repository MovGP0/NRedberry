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

        Assert.Same(argument, ArcCotFactory.Factory.Create(new Cot(argument)));
        Assert.Equal(
            TensorApi.Parse("pi/2").ToString(OutputFormat.Redberry),
            ArcCotFactory.Factory.Create(Complex.Zero).ToString(OutputFormat.Redberry));
        Assert.IsType<ArcCot>(ArcCotFactory.Factory.Create(argument));
    }
}
