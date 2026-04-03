using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Parsers;

namespace NRedberry.Core.Tests.Parsers;

public sealed class ParseTokenNumberTests
{
    [Fact]
    public void ShouldThrowWhenConstructedWithNullValue()
    {
        var exception = Should.Throw<ArgumentNullException>(() => new ParseTokenNumber(null!));

        exception.ParamName.ShouldBe("value");
    }

    [Fact]
    public void ShouldExposeTokenTypeAndValue()
    {
        var value = new Complex(3, 4);

        var token = new ParseTokenNumber(value);

        token.TokenType.ShouldBe(TokenType.Number);
        token.Value.ShouldBeSameAs(value);
    }

    [Fact]
    public void ShouldReturnEmptyIndices()
    {
        var token = new ParseTokenNumber(new Complex(2, 5));

        var indices = token.GetIndices();

        indices.ShouldBeSameAs(IndicesFactory.EmptyIndices);
    }

    [Fact]
    public void ShouldWrapFormattedValueInParentheses()
    {
        var value = new Complex(5, -7);
        var token = new ParseTokenNumber(value);

        var text = token.ToString(OutputFormat.Redberry);

        text.ShouldBe("(" + value.ToString(OutputFormat.Redberry) + ")");
    }

    [Fact]
    public void ShouldReturnValueAsTensor()
    {
        var value = new Complex(8, 9);
        var token = new ParseTokenNumber(value);

        var tensor = token.ToTensor();

        tensor.ShouldBeSameAs(value);
    }

    [Fact]
    public void ShouldBeEqualWhenValuesAreEqual()
    {
        var left = new ParseTokenNumber(new Complex(6, -1));
        var right = new ParseTokenNumber(new Complex(6, -1));

        var areEqual = left.Equals(right);

        areEqual.ShouldBeTrue();
        left.GetHashCode().ShouldBe(right.GetHashCode());
    }

    [Fact]
    public void ShouldNotBeEqualWhenComparedWithDifferentType()
    {
        var token = new ParseTokenNumber(new Complex(4, 4));
        var other = new ParseToken(TokenType.Number);

        var areEqual = token.Equals(other);

        areEqual.ShouldBeFalse();
    }

    [Fact]
    public void ShouldNotBeEqualWhenComparedWithNull()
    {
        var token = new ParseTokenNumber(new Complex(4, 4));

        var areEqual = token.Equals(null);

        areEqual.ShouldBeFalse();
    }

    [Fact]
    public void ShouldReturnValueHashCode()
    {
        var value = new Complex(10, 11);
        var token = new ParseTokenNumber(value);

        var hashCode = token.GetHashCode();

        hashCode.ShouldBe(value.GetHashCode());
    }
}
