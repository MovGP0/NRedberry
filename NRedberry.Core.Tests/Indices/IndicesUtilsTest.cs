using NRedberry.Indices;

namespace NRedberry.Core.Tests.Indices;

public sealed class IndicesUtilsTest
{
    [Fact]
    public void ShouldParseIndices()
    {
        int bracketed = IndicesUtils.ParseIndex("_{\\mu}");
        int latex = IndicesUtils.ParseIndex("_\\mu");
        int latin = IndicesUtils.ParseIndex("_a");

        IndicesUtils.ToString(bracketed, OutputFormat.LaTeX).ShouldBe("_\\mu");
        IndicesUtils.ToString(latex, OutputFormat.LaTeX).ShouldBe("_\\mu");
        IndicesUtils.ToString(latin, OutputFormat.LaTeX).ShouldBe("_a");
    }

    [Fact]
    public void ShouldSetState()
    {
        int lower = IndicesUtils.ParseIndex("_a");
        int upper = IndicesUtils.ParseIndex("^a");

        IndicesUtils.ToString(IndicesUtils.SetState(true, lower)).ShouldBe("^a");
        IndicesUtils.ToString(IndicesUtils.SetState(true, upper)).ShouldBe("^a");
        IndicesUtils.ToString(IndicesUtils.SetRawState(0, upper)).ShouldBe("_a");
        IndicesUtils.ToString(IndicesUtils.SetState(false, upper)).ShouldBe("_a");
    }
}
