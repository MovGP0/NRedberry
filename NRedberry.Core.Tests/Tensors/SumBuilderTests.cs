using NRedberry.Numbers;
using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class SumBuilderTests
{
    [Fact]
    public void ShouldSimplifyMergedTerms()
    {
        SumBuilder builder = new();
        builder.Put(TensorFactory.Parse("a"));
        builder.Put(TensorFactory.Parse("2*a"));
        builder.Put(TensorFactory.Parse("-3*a"));
        builder.Put(TensorFactory.Parse("a*b"));
        builder.Put(TensorFactory.Parse("7*a*b"));
        builder.Put(TensorFactory.Parse("Sin[c]"));
        builder.Put(TensorFactory.Parse("d"));
        builder.Put(TensorFactory.Parse("Sin[-c]"));

        Assert.Equal("d+8*a*b", builder.Build().ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldReturnZeroForEmptyBuilder()
    {
        SumBuilder builder = new();

        Assert.Equal(Complex.Zero, builder.Build());
    }

    [Fact]
    public void ShouldCloneStateIndependently()
    {
        SumBuilder builder = new();
        builder.Put(TensorFactory.Parse("a"));

        TensorBuilder clone = builder.Clone();
        clone.Put(TensorFactory.Parse("a"));

        Assert.Equal("a", builder.Build().ToString(OutputFormat.Redberry));
        Assert.Equal("2*a", clone.Build().ToString(OutputFormat.Redberry));
    }
}
