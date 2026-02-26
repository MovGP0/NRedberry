using NRedberry;
using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Parsers;
using Xunit;

namespace NRedberry.Core.Tests.Parsers;

public sealed class ParseTokenNumberTests
{
    [Fact]
    public void ShouldThrowWhenConstructedWithNullValue()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new ParseTokenNumber(null!));

        Assert.Equal("value", exception.ParamName);
    }

    [Fact]
    public void ShouldExposeTokenTypeAndValue()
    {
        var value = new Complex(3, 4);

        var token = new ParseTokenNumber(value);

        Assert.Equal(TokenType.Number, token.TokenType);
        Assert.Same(value, token.Value);
    }

    [Fact]
    public void ShouldReturnEmptyIndices()
    {
        var token = new ParseTokenNumber(new Complex(2, 5));

        var indices = token.GetIndices();

        Assert.Same(IndicesFactory.EmptyIndices, indices);
    }

    [Fact]
    public void ShouldWrapFormattedValueInParentheses()
    {
        var value = new Complex(5, -7);
        var token = new ParseTokenNumber(value);

        var text = token.ToString(OutputFormat.Redberry);

        Assert.Equal("(" + value.ToString(OutputFormat.Redberry) + ")", text);
    }

    [Fact]
    public void ShouldReturnValueAsTensor()
    {
        var value = new Complex(8, 9);
        var token = new ParseTokenNumber(value);

        var tensor = token.ToTensor();

        Assert.Same(value, tensor);
    }

    [Fact]
    public void ShouldBeEqualWhenValuesAreEqual()
    {
        var left = new ParseTokenNumber(new Complex(6, -1));
        var right = new ParseTokenNumber(new Complex(6, -1));

        var areEqual = left.Equals(right);

        Assert.True(areEqual);
        Assert.Equal(right.GetHashCode(), left.GetHashCode());
    }

    [Fact]
    public void ShouldNotBeEqualWhenComparedWithDifferentType()
    {
        var token = new ParseTokenNumber(new Complex(4, 4));
        var other = new ParseToken(TokenType.Number);

        var areEqual = token.Equals(other);

        Assert.False(areEqual);
    }

    [Fact]
    public void ShouldNotBeEqualWhenComparedWithNull()
    {
        var token = new ParseTokenNumber(new Complex(4, 4));

        var areEqual = token.Equals(null);

        Assert.False(areEqual);
    }

    [Fact]
    public void ShouldReturnValueHashCode()
    {
        var value = new Complex(10, 11);
        var token = new ParseTokenNumber(value);

        var hashCode = token.GetHashCode();

        Assert.Equal(value.GetHashCode(), hashCode);
    }
}
