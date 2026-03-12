using NRedberry.Indices;
using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class TensorFieldTests
{
    [Fact]
    public void ShouldExposeArgumentsAndFactories()
    {
        TensorField field = Assert.IsType<TensorField>(TensorFactory.Parse("f[x]"));

        Assert.Equal(1, field.Size);
        Assert.Equal("x", field[0].ToString(OutputFormat.Redberry));
        Assert.False(field.IsDerivative());
        Assert.False(field.IsDiracDelta());
        Assert.Same(field, field.GetParentField());
        Assert.Equal("TensorFieldBuilder", field.GetBuilder().GetType().Name);
        Assert.Equal("TensorFieldFactory", field.GetFactory()!.GetType().Name);
    }

    [Fact]
    public void ShouldReturnClonedArgumentsAndArgumentIndices()
    {
        TensorField field = Assert.IsType<TensorField>(TensorFactory.Parse("f[x]"));

        NRedberry.Tensors.Tensor[] arguments = field.GetArguments();
        SimpleIndices[] argIndices = field.GetArgIndices();

        Assert.Single(arguments);
        Assert.Single(argIndices);
        Assert.NotSame(arguments, field.GetArguments());
        Assert.NotSame(argIndices, field.GetArgIndices());
        Assert.Equal(0, field.GetArgIndices(0).Size());
    }

    [Fact]
    public void ShouldRenderAsFieldApplication()
    {
        TensorField field = Assert.IsType<TensorField>(TensorFactory.Parse("f[x]"));

        Assert.Equal("f[x]", field.ToString(OutputFormat.Redberry));
    }
}
