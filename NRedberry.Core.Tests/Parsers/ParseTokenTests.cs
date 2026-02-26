using NRedberry.Indices;
using NRedberry.Parsers;
using Xunit;

namespace NRedberry.Core.Tests.Parsers;

public sealed class ParseTokenTests
{
    [Fact]
    public void ShouldSetParentForChildren()
    {
        var child = new ParseToken(TokenType.Dummy);
        var parent = new ParseToken(TokenType.Sum, child);

        Assert.Same(parent, child.Parent);
    }

    [Fact]
    public void ShouldThrowArgumentNullExceptionWhenContentIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new ParseToken(TokenType.Sum, null!));
    }

    [Fact]
    public void ShouldGetProductIndicesByAppendingChildIndices()
    {
        var first = new ParseTokenLeaf("a", "a", IndicesFactory.Create(1, 2));
        var second = new ParseTokenLeaf("b", "b", IndicesFactory.Create(3));
        var token = new ParseToken(TokenType.Product, first, second);

        var indices = token.GetIndices();

        Assert.Equal(3, indices.Size());
        Assert.Equal(new[] { 1, 2, 3 }, indices.AllIndices);
    }

    [Fact]
    public void ShouldGetSumIndicesFromFirstTerm()
    {
        var first = new ParseTokenLeaf("a", "a", IndicesFactory.Create(1, 2));
        var second = new ParseTokenLeaf("b", "b", IndicesFactory.Create(3, 4));
        var token = new ParseToken(TokenType.Sum, first, second);

        var indices = token.GetIndices();

        Assert.Equal(first.GetIndices(), indices);
    }

    [Fact]
    public void ShouldReturnEmptyIndicesForPower()
    {
        var token = new ParseToken(TokenType.Power, new ParseTokenLeaf("a", "a"), new ParseTokenLeaf("b", "b"));

        var indices = token.GetIndices();

        Assert.Same(IndicesFactory.EmptyIndices, indices);
    }

    [Fact]
    public void ShouldThrowParserExceptionForUnsupportedGetIndicesTokenType()
    {
        var token = new ParseToken(TokenType.SimpleTensor);

        var exception = Assert.Throws<ParserException>(() => token.GetIndices());

        Assert.Equal("Unknown tensor type: SimpleTensor", exception.Message);
    }

    [Fact]
    public void ShouldFormatProductUsingOperatorForOutputMode()
    {
        var token = new ParseToken(TokenType.Product, new ParseTokenLeaf("a", "a"), new ParseTokenLeaf("b", "b"));

        Assert.Equal("a*b", token.ToString(OutputFormat.Redberry));
        Assert.Equal("a b", token.ToString(OutputFormat.LaTeX));
    }

    [Fact]
    public void ShouldFormatSumAndPreserveSignedTerms()
    {
        var token = new ParseToken(TokenType.Sum, new ParseTokenLeaf("a", "a"), new ParseTokenLeaf("-b", "-b"));

        Assert.Equal("(a-b)", token.ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldThrowInvalidOperationExceptionWhenFormattingUnsupportedTokenType()
    {
        var token = new ParseToken(TokenType.Number);

        Assert.Throws<InvalidOperationException>(() => token.ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldFormatDebugStringWithTokenTypeAndChildren()
    {
        var token = new ParseToken(TokenType.Sum, new ParseTokenLeaf("a", "a"), new ParseTokenLeaf("b", "b"));

        Assert.Equal("Sum[a, b]", token.ToString());
    }

    [Fact]
    public void ShouldThrowParserExceptionWhenPowerArgumentCountIsInvalid()
    {
        var token = new ParseToken(TokenType.Power, new ParseTokenLeaf("a", "a"));

        var exception = Assert.Throws<ParserException>(() => token.ToTensor());

        Assert.Equal("Power token should have exactly 2 arguments.", exception.Message);
    }

    [Fact]
    public void ShouldThrowParserExceptionWhenConvertingUnsupportedTokenTypeToTensor()
    {
        var token = new ParseToken(TokenType.Number);

        var exception = Assert.Throws<ParserException>(() => token.ToTensor());

        Assert.Equal("Unknown tensor type: Number", exception.Message);
    }

    [Fact]
    public void ShouldSupportStructuralEqualityAndHashCode()
    {
        var left = new ParseToken(TokenType.Product, new ParseToken(TokenType.Dummy), new ParseToken(TokenType.Dummy));
        var right = new ParseToken(TokenType.Product, new ParseToken(TokenType.Dummy), new ParseToken(TokenType.Dummy));

        Assert.True(left.Equals(right));
        Assert.True(left == right);
        Assert.False(left != right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }

    [Fact]
    public void ShouldNotBeEqualWhenRuntimeTypesDiffer()
    {
        var left = new ParseToken(TokenType.Dummy);
        var right = new ParseTokenLeaf("x", "x");

        Assert.False(left.Equals(right));
    }
}

internal sealed class ParseTokenLeaf : ParseToken
{
    private readonly string _toStringValue;
    private readonly string _formattedValue;
    private readonly NRedberry.Indices.Indices _indices;

    public ParseTokenLeaf(string toStringValue, string formattedValue, NRedberry.Indices.Indices? indices = null)
        : base(TokenType.Dummy)
    {
        _toStringValue = toStringValue;
        _formattedValue = formattedValue;
        _indices = indices ?? IndicesFactory.EmptyIndices;
    }

    public override NRedberry.Indices.Indices GetIndices()
    {
        return _indices;
    }

    public override string ToString()
    {
        return _toStringValue;
    }

    public override string ToString(OutputFormat mode)
    {
        return _formattedValue;
    }
}
