using NRedberry.Parsers;
using Xunit;

namespace NRedberry.Core.Tests.Parser;

public sealed class ParserIndicesTest
{
    [Fact]
    public void ShouldParseSimpleIndices()
    {
        var indices = ParserIndices.ParseSimple("_{AC_{21}B}");

        Assert.Equal(3, indices.Size());
    }

    [Fact]
    public void ShouldRejectUnicodeGreekSymbolsWithoutConverters()
    {
        Assert.Throws<ArgumentException>(() => ParserIndices.ParseSimple("_μν^αβ"));
        Assert.Equal(ParserIndices.ParseSimple("_\\mu\\nu^\\alpha\\beta"), ParserIndices.ParseSimple("_\\mu\\nu^\\alpha\\beta"));
    }

    [Fact]
    public void ShouldParseIgnoringVariance()
    {
        Assert.Equal(ParserIndices.ParseSimpleIgnoringVariance("_aabc"), ParserIndices.ParseSimple("^a_abc"));
        Assert.Equal(ParserIndices.ParseSimpleIgnoringVariance("_abca"), ParserIndices.ParseSimple("^a_bca"));
        Assert.Equal(ParserIndices.ParseSimpleIgnoringVariance("_bcaa"), ParserIndices.ParseSimple("_bc^a_a"));
        Assert.Equal(ParserIndices.ParseSimpleIgnoringVariance("^abab"), ParserIndices.ParseSimple("_ab^ab"));
    }
}
