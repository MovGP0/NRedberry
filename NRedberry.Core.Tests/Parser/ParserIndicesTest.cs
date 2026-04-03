using NRedberry.Parsers;

namespace NRedberry.Core.Tests.Parser;

public sealed class ParserIndicesTest
{
    [Fact]
    public void ShouldParseSimpleIndices()
    {
        var indices = ParserIndices.ParseSimple("_{AC_{21}B}");

        indices.Size().ShouldBe(3);
    }

    [Fact]
    public void ShouldRejectUnicodeGreekSymbolsWithoutConverters()
    {
        Should.Throw<ArgumentException>(() => ParserIndices.ParseSimple("_μν^αβ"));
        ParserIndices.ParseSimple("_\\mu\\nu^\\alpha\\beta").ShouldBe(ParserIndices.ParseSimple("_\\mu\\nu^\\alpha\\beta"));
    }

    [Fact]
    public void ShouldParseIgnoringVariance()
    {
        ParserIndices.ParseSimple("^a_abc").ShouldBe(ParserIndices.ParseSimpleIgnoringVariance("_aabc"));
        ParserIndices.ParseSimple("^a_bca").ShouldBe(ParserIndices.ParseSimpleIgnoringVariance("_abca"));
        ParserIndices.ParseSimple("_bc^a_a").ShouldBe(ParserIndices.ParseSimpleIgnoringVariance("_bcaa"));
        ParserIndices.ParseSimple("_ab^ab").ShouldBe(ParserIndices.ParseSimpleIgnoringVariance("^abab"));
    }
}
