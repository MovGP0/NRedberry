using NRedberry.Numbers;
using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class SumBuilderSplitingScalarsTests
{
    [Fact]
    public void ShouldKeepScalarAndTensorPartsVisible()
    {
        SumBuilderSplitingScalars builder = new();
        builder.Put(TensorFactory.Parse("2*f_m"));

        string text = builder.Build().ToString(OutputFormat.Redberry);

        Assert.Contains("f_{m}", text);
        Assert.Contains("2", text);
    }

    [Fact]
    public void ShouldReturnZeroForEmptyBuilder()
    {
        SumBuilderSplitingScalars builder = new();

        Assert.Equal(Complex.Zero, builder.Build());
    }

    [Fact]
    public void ShouldCloneStateIndependently()
    {
        SumBuilderSplitingScalars builder = new();
        builder.Put(TensorFactory.Parse("f_m"));

        TensorBuilder clone = builder.Clone();
        clone.Put(TensorFactory.Parse("g_m"));

        Assert.Equal("f_{m}", builder.Build().ToString(OutputFormat.Redberry));
        string cloneText = clone.Build().ToString(OutputFormat.Redberry);
        Assert.Contains("f_{m}", cloneText);
        Assert.Contains("g_{m}", cloneText);
    }
}
