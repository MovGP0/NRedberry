namespace NRedberry.Core.Tests.Tensors;

public sealed class OutputFormatTests
{
    [Fact]
    public void ShouldExposeExpectedPredefinedFormats()
    {
        OutputFormat.LaTeX.Id.ShouldBe(0);
        OutputFormat.Maple.Id.ShouldBe(5);
        OutputFormat.Maple.LowerIndexPrefix.ShouldBe(string.Empty);
        OutputFormat.Maple.UpperIndexPrefix.ShouldBe("~");
        OutputFormat.SimpleRedberry.PrintMatrixIndices.ShouldBeFalse();
        OutputFormat.Redberry.PrintMatrixIndices.ShouldBeTrue();
    }

    [Fact]
    public void ShouldDisableMatrixIndicesOnlyOnce()
    {
        OutputFormat disabled = OutputFormat.Redberry.DoNotPrintMatrixIndices();

        disabled.PrintMatrixIndices.ShouldBeFalse();
        disabled.DoNotPrintMatrixIndices().ShouldBeSameAs(disabled);
        disabled.ShouldNotBeSameAs(OutputFormat.Redberry);
        disabled.Id.ShouldBe(OutputFormat.Redberry.Id);
    }

    [Fact]
    public void ShouldEnableMatrixIndicesOnlyOnce()
    {
        OutputFormat enabled = OutputFormat.SimpleRedberry.PrintMatrixIndicesAlways();

        enabled.PrintMatrixIndices.ShouldBeTrue();
        enabled.PrintMatrixIndicesAlways().ShouldBeSameAs(enabled);
        enabled.ShouldNotBeSameAs(OutputFormat.SimpleRedberry);
        enabled.Id.ShouldBe(OutputFormat.SimpleRedberry.Id);
    }

    [Fact]
    public void ShouldCompareFormatsById()
    {
        OutputFormat.Redberry.Is(OutputFormat.Redberry).ShouldBeTrue();
        OutputFormat.Redberry.DoNotPrintMatrixIndices().Is(OutputFormat.Redberry).ShouldBeTrue();
        OutputFormat.Redberry.Is(OutputFormat.LaTeX).ShouldBeFalse();
    }

    [Fact]
    public void ShouldReturnPrefixFromIntState()
    {
        OutputFormat.Redberry.GetPrefixFromIntState(0).ShouldBe("_");
        OutputFormat.Redberry.GetPrefixFromIntState(1).ShouldBe("^");
        Should.Throw<ArgumentException>(() => OutputFormat.Redberry.GetPrefixFromIntState(2));
    }

    [Fact]
    public void ShouldReturnPrefixFromRawIntState()
    {
        OutputFormat.Redberry.GetPrefixFromRawIntState(0).ShouldBe("_");
        OutputFormat.Redberry.GetPrefixFromRawIntState(unchecked((int)0x80000000)).ShouldBe("^");
        Should.Throw<ArgumentException>(() => OutputFormat.Redberry.GetPrefixFromRawIntState(1));
    }
}
