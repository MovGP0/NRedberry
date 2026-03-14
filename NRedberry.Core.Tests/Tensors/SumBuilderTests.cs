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

        text.ShouldContain("a");
        text.ShouldContain("b");
        text.ShouldContain("Sin[c]");
        text.ShouldContain("d");
    }

    [Fact]
    public void ShouldReturnZeroForEmptyBuilder()
    {
        SumBuilder builder = new();

        builder.Build().ShouldBe(Complex.Zero);
    }

    [Fact]
    public void ShouldCloneStateIndependently()
    {
        SumBuilder builder = new();
        builder.Put(TensorFactory.Parse("a"));

        TensorBuilder clone = builder.Clone();
        clone.Put(TensorFactory.Parse("b"));

        builder.Build().ToString(OutputFormat.Redberry).ShouldBe("a");
        string cloneText = clone.Build().ToString(OutputFormat.Redberry);
        cloneText.ShouldContain("a");
        cloneText.ShouldContain("b");
    }
}
