using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class OutputFormatTests
{
    [Fact]
    public void ShouldExposeExpectedPredefinedFormats()
    {
        Assert.Equal(0, OutputFormat.LaTeX.Id);
        Assert.Equal(5, OutputFormat.Maple.Id);
        Assert.Equal(string.Empty, OutputFormat.Maple.LowerIndexPrefix);
        Assert.Equal("~", OutputFormat.Maple.UpperIndexPrefix);
        Assert.False(OutputFormat.SimpleRedberry.PrintMatrixIndices);
        Assert.True(OutputFormat.Redberry.PrintMatrixIndices);
    }

    [Fact]
    public void ShouldDisableMatrixIndicesOnlyOnce()
    {
        OutputFormat disabled = OutputFormat.Redberry.DoNotPrintMatrixIndices();

        Assert.False(disabled.PrintMatrixIndices);
        Assert.Same(disabled, disabled.DoNotPrintMatrixIndices());
        Assert.NotSame(OutputFormat.Redberry, disabled);
        Assert.Equal(OutputFormat.Redberry.Id, disabled.Id);
    }

    [Fact]
    public void ShouldEnableMatrixIndicesOnlyOnce()
    {
        OutputFormat enabled = OutputFormat.SimpleRedberry.PrintMatrixIndicesAlways();

        Assert.True(enabled.PrintMatrixIndices);
        Assert.Same(enabled, enabled.PrintMatrixIndicesAlways());
        Assert.NotSame(OutputFormat.SimpleRedberry, enabled);
        Assert.Equal(OutputFormat.SimpleRedberry.Id, enabled.Id);
    }

    [Fact]
    public void ShouldCompareFormatsById()
    {
        Assert.True(OutputFormat.Redberry.Is(OutputFormat.Redberry));
        Assert.True(OutputFormat.Redberry.DoNotPrintMatrixIndices().Is(OutputFormat.Redberry));
        Assert.False(OutputFormat.Redberry.Is(OutputFormat.LaTeX));
    }

    [Fact]
    public void ShouldReturnPrefixFromIntState()
    {
        Assert.Equal("_", OutputFormat.Redberry.GetPrefixFromIntState(0));
        Assert.Equal("^", OutputFormat.Redberry.GetPrefixFromIntState(1));
        Assert.Throws<ArgumentException>(() => OutputFormat.Redberry.GetPrefixFromIntState(2));
    }

    [Fact]
    public void ShouldReturnPrefixFromRawIntState()
    {
        Assert.Equal("_", OutputFormat.Redberry.GetPrefixFromRawIntState(0));
        Assert.Equal("^", OutputFormat.Redberry.GetPrefixFromRawIntState(unchecked((int)0x80000000)));
        Assert.Throws<ArgumentException>(() => OutputFormat.Redberry.GetPrefixFromRawIntState(1));
    }
}
