using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Parsers;
using NRedberry.Tensors.Functions;

namespace NRedberry.Core.Tests.Parsers;

public sealed class ParseTokenScalarFunctionTests
{
    [Fact]
    public void ShouldThrowWhenConstructedWithNullFunction()
    {
        var exception = Should.Throw<ArgumentNullException>(() => new ParseTokenScalarFunction(null!, new ParseTokenNumber(Complex.One)));

        exception.ParamName.ShouldBe("function");
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

        var exception = Should.Throw<ArgumentException>(() => new ParseTokenScalarFunction("Sin", content));

        exception.Message.ShouldBe("content");
    }

    [Fact]
    public void ShouldExposeScalarFunctionTypeAndFunctionName()
    {
        var token = new ParseTokenScalarFunction("Sin", new ParseTokenNumber(Complex.One));

        token.TokenType.ShouldBe(TokenType.ScalarFunction);
        token.Function.ShouldBe("Sin");
        token.Content.ShouldHaveSingleItem();
    }

    [Fact]
    public void ShouldReturnEmptyIndices()
    {
        var token = new ParseTokenScalarFunction("Sin", new ParseTokenNumber(Complex.One));

        var indices = token.GetIndices();

        indices.ShouldBeSameAs(IndicesFactory.EmptyIndices);
    }

    [Fact]
    public void ShouldRenderFunctionAndArgumentInToString()
    {
        var token = new ParseTokenScalarFunction("Sin", new ParseTokenLeaf("x", "x"));

        var text = token.ToString();

        text.ShouldBe("Sin[x]");
    }

    [Fact]
    public void ShouldDispatchToKnownFunctionIgnoringCase()
    {
        var token = new ParseTokenScalarFunction("sIN", new ParseTokenNumber(Complex.One));

        var tensor = token.ToTensor();
        var expected = SinFactory.Factory.Create(Complex.One);

        tensor.ShouldBe(expected);
    }

    [Fact]
    public void ShouldThrowForUnknownFunction()
    {
        var token = new ParseTokenScalarFunction("Unknown", new ParseTokenNumber(Complex.One));

        var exception = Should.Throw<InvalidOperationException>(() => token.ToTensor());

        exception.Message.ShouldBe("Unknown scalar function \"Unknown\".");
    }

    [Fact]
    public void ShouldIncludeFunctionNameInEqualityAndHashCode()
    {
        var left = new ParseTokenScalarFunction("Sin", new ParseTokenNumber(Complex.One));
        var equal = new ParseTokenScalarFunction("Sin", new ParseTokenNumber(Complex.One));
        var different = new ParseTokenScalarFunction("Cos", new ParseTokenNumber(Complex.One));

        left.Equals((object?)equal).ShouldBeTrue();
        equal.GetHashCode().ShouldBe(left.GetHashCode());
        left.Equals((object?)different).ShouldBeFalse();
    }
}
