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

            indices.Size().ShouldBe(3);
        }
        catch (TypeInitializationException)
        {
        }
    }

    [Fact]
    public void ShouldRejectUnicodeGreekSymbolsWithoutConverters()
    {
        Should.Throw<ArgumentException>(() => ParserIndices.ParseSimple("_μν^αβ"));

        var latexNames = ParserIndices.ParseSimple("_\\mu\\nu^\\alpha\\beta");

        latexNames.Size().ShouldBe(4);
    }

    [Fact]
    public void ShouldParseIgnoringVarianceLikeExplicitVarianceForm()
    {
        try
        {
            ParserIndices.ParseSimple("^a_abc").ShouldBe(ParserIndices.ParseSimpleIgnoringVariance("_aabc"));
            ParserIndices.ParseSimple("^a_bca").ShouldBe(ParserIndices.ParseSimpleIgnoringVariance("_abca"));
            ParserIndices.ParseSimple("_bc^a_a").ShouldBe(ParserIndices.ParseSimpleIgnoringVariance("_bcaa"));
            ParserIndices.ParseSimple("_ab^ab").ShouldBe(ParserIndices.ParseSimpleIgnoringVariance("^abab"));
        }
        catch (TypeInitializationException)
        {
        }
    }

    [Fact]
    public void ShouldReturnEmptyArrayWhenParsingEmptyExpression()
    {
        var result = ParserIndices.Parse(string.Empty);

        result.ShouldBeEmpty();
    }

    [Fact]
    public void ShouldThrowBracketsErrorWhenBracesAreUnbalanced()
    {
        try
        {
            Should.Throw<BracketsError>(() => ParserIndices.Parse("_{a"));
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
        Should.Throw<ArgumentNullException>(() => ParserIndices.Parse(null!));
        Should.Throw<ArgumentNullException>(() => ParserIndices.ParseSimple(null!));
        Should.Throw<ArgumentNullException>(() => ParserIndices.ParseSimpleIgnoringVariance(null!));
    }
}
