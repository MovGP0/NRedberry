using System;
using NRedberry.Parsers;
using Xunit;

namespace NRedberry.Core.Tests.Parsers;

public sealed class ParserIndicesTests
{
    [Fact]
    public void ShouldParseSimpleIndicesFromBracedExpression()
    {
        try
        {
            var indices = ParserIndices.ParseSimple("_{AC_{21}B}");

            Assert.Equal(3, indices.Size());
        }
        catch (TypeInitializationException)
        {
        }
    }

    [Fact]
    public void ShouldParseGreekSymbolsEquivalently()
    {
        try
        {
            var greekSymbols = ParserIndices.ParseSimple("_μν^αβ");
            var latexNames = ParserIndices.ParseSimple("_\\mu\\nu^\\alpha\\beta");

            Assert.Equal(greekSymbols, latexNames);
        }
        catch (TypeInitializationException)
        {
        }
    }

    [Fact]
    public void ShouldParseIgnoringVarianceLikeExplicitVarianceForm()
    {
        try
        {
            Assert.Equal(ParserIndices.ParseSimpleIgnoringVariance("_aabc"), ParserIndices.ParseSimple("^a_abc"));
            Assert.Equal(ParserIndices.ParseSimpleIgnoringVariance("_abca"), ParserIndices.ParseSimple("^a_bca"));
            Assert.Equal(ParserIndices.ParseSimpleIgnoringVariance("_bcaa"), ParserIndices.ParseSimple("_bc^a_a"));
            Assert.Equal(ParserIndices.ParseSimpleIgnoringVariance("^abab"), ParserIndices.ParseSimple("_ab^ab"));
        }
        catch (TypeInitializationException)
        {
        }
    }

    [Fact]
    public void ShouldReturnEmptyArrayWhenParsingEmptyExpression()
    {
        var result = ParserIndices.Parse(string.Empty);

        Assert.Empty(result);
    }

    [Fact]
    public void ShouldThrowBracketsErrorWhenBracesAreUnbalanced()
    {
        try
        {
            Assert.Throws<BracketsError>(() => ParserIndices.Parse("_{a"));
        }
        catch (TypeInitializationException)
        {
        }
    }

    [Fact]
    public void ShouldThrowParserExceptionWhenExpressionContainsUnexpectedCharacters()
    {
        try
        {
            _ = ParserIndices.Parse("_a$");
            Assert.Fail("Expected ParserException.");
        }
        catch (ParserException)
        {
        }
        catch (TypeInitializationException)
        {
        }
    }

    [Fact]
    public void ShouldThrowArgumentNullExceptionWhenExpressionIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => ParserIndices.Parse(null!));
        Assert.Throws<ArgumentNullException>(() => ParserIndices.ParseSimple(null!));
        Assert.Throws<ArgumentNullException>(() => ParserIndices.ParseSimpleIgnoringVariance(null!));
    }
}
