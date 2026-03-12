using NRedberry.Numbers;
using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class SumBuilderTests
{
    [Fact]
    public void ShouldFlattenNestedSumsAndKeepDistinctTerms()
    {
        SumBuilder builder = new();
        builder.Put(TensorFactory.Parse("a+b"));
        builder.Put(TensorFactory.Parse("Sin[c]"));
        builder.Put(TensorFactory.Parse("d"));

        string text = builder.Build().ToString(OutputFormat.Redberry);

        Assert.Contains("a", text);
        Assert.Contains("b", text);
        Assert.Contains("Sin[c]", text);
        Assert.Contains("d", text);
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
        clone.Put(TensorFactory.Parse("b"));

        Assert.Equal("a", builder.Build().ToString(OutputFormat.Redberry));
        string cloneText = clone.Build().ToString(OutputFormat.Redberry);
        Assert.Contains("a", cloneText);
        Assert.Contains("b", cloneText);
    }
}
