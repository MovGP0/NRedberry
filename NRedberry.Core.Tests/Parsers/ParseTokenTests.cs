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

        child.Parent.ShouldBeSameAs(parent);
    }

    [Fact]
    public void ShouldThrowArgumentNullExceptionWhenContentIsNull()
    {
        Should.Throw<ArgumentNullException>(() => new ParseToken(TokenType.Sum, null!));
    }

    [Fact]
    public void ShouldGetProductIndicesByAppendingChildIndices()
    {
        var first = new ParseTokenLeaf("a", "a", IndicesFactory.Create(1, 2));
        var second = new ParseTokenLeaf("b", "b", IndicesFactory.Create(3));
        var token = new ParseToken(TokenType.Product, first, second);

        var indices = token.GetIndices();

        indices.Size().ShouldBe(3);
        indices.AllIndices.ShouldBe(new[] { 1, 2, 3 });
    }

    [Fact]
    public void ShouldGetSumIndicesFromFirstTerm()
    {
        var first = new ParseTokenLeaf("a", "a", IndicesFactory.Create(1, 2));
        var second = new ParseTokenLeaf("b", "b", IndicesFactory.Create(3, 4));
        var token = new ParseToken(TokenType.Sum, first, second);

        var indices = token.GetIndices();

        indices.ShouldBe(first.GetIndices());
    }

    [Fact]
    public void ShouldReturnEmptyIndicesForPower()
    {
        var token = new ParseToken(TokenType.Power, new ParseTokenLeaf("a", "a"), new ParseTokenLeaf("b", "b"));

        var indices = token.GetIndices();

        indices.ShouldBeSameAs(IndicesFactory.EmptyIndices);
    }

    [Fact]
    public void ShouldThrowParserExceptionForUnsupportedGetIndicesTokenType()
    {
        var token = new ParseToken(TokenType.SimpleTensor);

        var exception = Should.Throw<ParserException>(() => token.GetIndices());

        exception.Message.ShouldBe("Unknown tensor type: SimpleTensor");
    }

    [Fact]
    public void ShouldFormatProductUsingOperatorForOutputMode()
    {
        var token = new ParseToken(TokenType.Product, new ParseTokenLeaf("a", "a"), new ParseTokenLeaf("b", "b"));

        token.ToString(OutputFormat.Redberry).ShouldBe("a*b");
        token.ToString(OutputFormat.LaTeX).ShouldBe("a b");
    }

    [Fact]
    public void ShouldFormatSumAndPreserveSignedTerms()
    {
        var token = new ParseToken(TokenType.Sum, new ParseTokenLeaf("a", "a"), new ParseTokenLeaf("-b", "-b"));

        token.ToString(OutputFormat.Redberry).ShouldBe("(a-b)");
    }

    [Fact]
    public void ShouldThrowInvalidOperationExceptionWhenFormattingUnsupportedTokenType()
    {
        var token = new ParseToken(TokenType.Number);

        Should.Throw<InvalidOperationException>(() => token.ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldFormatDebugStringWithTokenTypeAndChildren()
    {
        var token = new ParseToken(TokenType.Sum, new ParseTokenLeaf("a", "a"), new ParseTokenLeaf("b", "b"));

        token.ToString().ShouldBe("Sum[a, b]");
    }

    [Fact]
    public void ShouldThrowParserExceptionWhenPowerArgumentCountIsInvalid()
    {
        var token = new ParseToken(TokenType.Power, new ParseTokenLeaf("a", "a"));

        var exception = Should.Throw<ParserException>(() => token.ToTensor());

        exception.Message.ShouldBe("Power token should have exactly 2 arguments.");
    }

    [Fact]
    public void ShouldThrowParserExceptionWhenConvertingUnsupportedTokenTypeToTensor()
    {
        var token = new ParseToken(TokenType.Number);

        var exception = Should.Throw<ParserException>(() => token.ToTensor());

        exception.Message.ShouldBe("Unknown tensor type: Number");
    }

    [Fact]
    public void ShouldSupportStructuralEqualityAndHashCode()
    {
        var left = new ParseToken(TokenType.Product, new ParseToken(TokenType.Dummy), new ParseToken(TokenType.Dummy));
        var right = new ParseToken(TokenType.Product, new ParseToken(TokenType.Dummy), new ParseToken(TokenType.Dummy));

        left.Equals(right).ShouldBeTrue();
        left == right.ShouldBeTrue();
        left != right.ShouldBeFalse();
        right.GetHashCode().ShouldBe(left.GetHashCode());
    }

    [Fact]
    public void ShouldNotBeEqualWhenRuntimeTypesDiffer()
    {
        var left = new ParseToken(TokenType.Dummy);
        var right = new ParseTokenLeaf("x", "x");

        left.Equals(right).ShouldBeFalse();
    }
}

internal sealed class ParseTokenLeaf(
    string toStringValue,
    string formattedValue,
    NRedberry.Indices.Indices? indices = null)
    : ParseToken(TokenType.Dummy)
{
    private readonly NRedberry.Indices.Indices _indices = indices ?? IndicesFactory.EmptyIndices;

    public override NRedberry.Indices.Indices GetIndices()
    {
        return _indices;
    }

    public override string ToString()
    {
        return toStringValue;
    }

    public override string ToString(OutputFormat mode)
    {
        return formattedValue;
    }
}
