using NRedberry.Numbers;
using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Tensors;

public sealed class SumBuilderSplitingScalarsTests
{
    [Fact]
    public void ShouldKeepScalarAndTensorPartsVisible()
    {
        SumBuilderSplitingScalars builder = new();
        builder.Put(TensorFactory.Parse("2*f_m"));

        string text = builder.Build().ToString(OutputFormat.Redberry);

        text.ShouldContain("f_{m}");
        text.ShouldContain("2");
    }

    [Fact]
    public void ShouldReturnZeroForEmptyBuilder()
    {
        SumBuilderSplitingScalars builder = new();

        builder.Build().ShouldBe(Complex.Zero);
    }

    [Fact]
    public void ShouldCloneStateIndependently()
    {
        SumBuilderSplitingScalars builder = new();
        builder.Put(TensorFactory.Parse("f_m"));

        TensorBuilder clone = builder.Clone();
        clone.Put(TensorFactory.Parse("g_m"));

        builder.Build().ToString(OutputFormat.Redberry).ShouldBe("f_{m}");
        string cloneText = clone.Build().ToString(OutputFormat.Redberry);
        cloneText.ShouldContain("f_{m}");
        cloneText.ShouldContain("g_{m}");
    }
}
