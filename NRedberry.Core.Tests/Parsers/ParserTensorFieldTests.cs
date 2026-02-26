using NRedberry.Parsers;
using NRedberry.Numbers;
using RedberryParser = NRedberry.Parsers.Parser;
using Xunit;

namespace NRedberry.Core.Tests.Parsers;

public sealed class ParserTensorFieldTests
{
    [Fact]
    public void ShouldExposeSingletonInstance()
    {
        var first = ParserTensorField.Instance;
        var second = ParserTensorField.Instance;

        Assert.Same(first, second);
    }

    [Fact]
    public void ShouldExposeExpectedPriority()
    {
        Assert.Equal(7000, ParserTensorField.Instance.Priority);
    }

    [Fact]
    public void ShouldReturnNullWhenExpressionDoesNotContainOpeningBracket()
    {
        var token = ParserTensorField.Instance.ParseToken("F_a", RedberryParser.Default);

        Assert.Null(token);
    }

    [Fact]
    public void ShouldReturnNullWhenExpressionContainsTopLevelOperator()
    {
        var token = ParserTensorField.Instance.ParseToken("F[x]+G[y]", RedberryParser.Default);

        Assert.Null(token);
    }

    [Fact]
    public void ShouldThrowBracketsErrorWhenClosingBracketAppearsBeforeOpeningBracket()
    {
        Assert.Throws<BracketsError>(() => ParserTensorField.Instance.ParseToken("F]x[", RedberryParser.Default));
    }

    [Fact]
    public void ShouldThrowParserExceptionWhenArgumentContainsMoreThanOneColon()
    {
        var exception = Assert.Throws<ParserException>(() => ParserTensorField.Instance.ParseToken("F[x:_a:_b]", RedberryParser.Default));

        Assert.Equal("F[x:_a:_b]", exception.Message);
    }

    [Fact]
    public void ShouldRewriteSqrtAsPowerForScalarArgument()
    {
        var token = ParserTensorField.Instance.ParseToken("sqrt[x]", RedberryParser.Default);

        var power = Assert.IsType<ParseToken>(token);
        Assert.Equal(TokenType.Power, power.TokenType);
        Assert.Equal(2, power.Content.Length);
        Assert.Equal(TokenType.SimpleTensor, power.Content[0].TokenType);

        var exponent = Assert.IsType<ParseTokenNumber>(power.Content[1]);
        Assert.Equal(Complex.OneHalf, exponent.Value);
    }

    [Fact]
    public void ShouldParseSqrtWithSeveralArgumentsAsTensorField()
    {
        var token = ParserTensorField.Instance.ParseToken("sqrt[x,y]", RedberryParser.Default);

        var tensorField = Assert.IsType<ParseTokenTensorField>(token);
        Assert.Equal(TokenType.TensorField, tensorField.TokenType);
        Assert.Equal("sqrt", tensorField.Name);
    }

    [Fact]
    public void ShouldRewriteTrAsTrace()
    {
        var token = ParserTensorField.Instance.ParseToken("tr[x,y]", RedberryParser.Default);

        var trace = Assert.IsType<ParseToken>(token);
        Assert.Equal(TokenType.Trace, trace.TokenType);
        Assert.Equal(2, trace.Content.Length);
    }

    [Fact]
    public void ShouldParseTensorFieldWithArgumentIndicesAndNestedCommas()
    {
        var token = ParserTensorField.Instance.ParseToken("F[x:,Sin[g[m,n]]]", RedberryParser.Default);

        var tensorField = Assert.IsType<ParseTokenTensorField>(token);
        Assert.Equal(TokenType.TensorField, tensorField.TokenType);
        Assert.Equal("F", tensorField.Name);
        Assert.Equal(2, tensorField.Content.Length);
        Assert.Equal(TokenType.SimpleTensor, tensorField.Content[0].TokenType);
        Assert.Equal(TokenType.ScalarFunction, tensorField.Content[1].TokenType);
        Assert.Equal(2, tensorField.ArgumentsIndices.Length);
        Assert.NotNull(tensorField.ArgumentsIndices[0]);
        Assert.Equal(0, tensorField.ArgumentsIndices[0].Size());
        Assert.Null(tensorField.ArgumentsIndices[1]);
    }
}
