using NRedberry.Indices;
using Xunit;

namespace NRedberry.Core.Tests.Indices;

public sealed class IndicesUtilsTest
{
    [Fact]
    public void ShouldParseIndices()
    {
        int bracketed = IndicesUtils.ParseIndex("_{\\mu}");
        int latex = IndicesUtils.ParseIndex("_\\mu");
        int latin = IndicesUtils.ParseIndex("_a");

        Assert.Equal("_\\mu", IndicesUtils.ToString(bracketed, OutputFormat.LaTeX));
        Assert.Equal("_\\mu", IndicesUtils.ToString(latex, OutputFormat.LaTeX));
        Assert.Equal("_a", IndicesUtils.ToString(latin, OutputFormat.LaTeX));
    }

    [Fact]
    public void ShouldSetState()
    {
        int lower = IndicesUtils.ParseIndex("_a");
        int upper = IndicesUtils.ParseIndex("^a");

        Assert.Equal("^a", IndicesUtils.ToString(IndicesUtils.SetState(true, lower)));
        Assert.Equal("^a", IndicesUtils.ToString(IndicesUtils.SetState(true, upper)));
        Assert.Equal("_a", IndicesUtils.ToString(IndicesUtils.SetRawState(0, upper)));
        Assert.Equal("_a", IndicesUtils.ToString(IndicesUtils.SetState(false, upper)));
    }
}
