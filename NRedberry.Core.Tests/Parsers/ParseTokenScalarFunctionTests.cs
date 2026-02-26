using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Parsers;
using NRedberry.Tensors.Functions;
using Xunit;

namespace NRedberry.Core.Tests.Parsers;

public sealed class ParseTokenScalarFunctionTests
{
    [Fact]
    public void ShouldThrowWhenConstructedWithNullFunction()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new ParseTokenScalarFunction(null!, new ParseTokenNumber(Complex.One)));

        Assert.Equal("function", exception.ParamName);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    public void ShouldThrowWhenConstructedWithWrongArgumentCount(int argumentCount)
    {
        ParseToken[] content = argumentCount switch
        {
            0 => [],
            2 => [new ParseTokenNumber(Complex.One), new ParseTokenNumber(Complex.Two)],
            _ => throw new InvalidOperationException("Unsupported test input.")
        };

        var exception = Assert.Throws<ArgumentException>(() => new ParseTokenScalarFunction("Sin", content));

        Assert.Equal("content", exception.Message);
    }

    [Fact]
    public void ShouldExposeScalarFunctionTypeAndFunctionName()
    {
        var token = new ParseTokenScalarFunction("Sin", new ParseTokenNumber(Complex.One));

        Assert.Equal(TokenType.ScalarFunction, token.TokenType);
        Assert.Equal("Sin", token.Function);
        Assert.Single(token.Content);
    }

    [Fact]
    public void ShouldReturnEmptyIndices()
    {
        var token = new ParseTokenScalarFunction("Sin", new ParseTokenNumber(Complex.One));

        var indices = token.GetIndices();

        Assert.Same(IndicesFactory.EmptyIndices, indices);
    }

    [Fact]
    public void ShouldRenderFunctionAndArgumentInToString()
    {
        var token = new ParseTokenScalarFunction("Sin", new ParseTokenLeaf("x", "x"));

        var text = token.ToString();

        Assert.Equal("Sin[x]", text);
    }

    [Fact]
    public void ShouldDispatchToKnownFunctionIgnoringCase()
    {
        var token = new ParseTokenScalarFunction("sIN", new ParseTokenNumber(Complex.One));

        var tensor = token.ToTensor();
        var expected = SinFactory.Factory.Create(Complex.One);

        Assert.Equal(expected, tensor);
    }

    [Fact]
    public void ShouldThrowForUnknownFunction()
    {
        var token = new ParseTokenScalarFunction("Unknown", new ParseTokenNumber(Complex.One));

        var exception = Assert.Throws<InvalidOperationException>(() => token.ToTensor());

        Assert.Equal("Unknown scalar function \"Unknown\".", exception.Message);
    }

    [Fact]
    public void ShouldIncludeFunctionNameInEqualityAndHashCode()
    {
        var left = new ParseTokenScalarFunction("Sin", new ParseTokenNumber(Complex.One));
        var equal = new ParseTokenScalarFunction("Sin", new ParseTokenNumber(Complex.One));
        var different = new ParseTokenScalarFunction("Cos", new ParseTokenNumber(Complex.One));

        Assert.True(left.Equals((object?)equal));
        Assert.Equal(left.GetHashCode(), equal.GetHashCode());
        Assert.False(left.Equals((object?)different));
    }
}
