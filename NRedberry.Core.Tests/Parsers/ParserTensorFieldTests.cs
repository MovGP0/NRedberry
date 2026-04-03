using NRedberry.Parsers;
using NRedberry.Numbers;
using RedberryParser = NRedberry.Parsers.Parser;

namespace NRedberry.Core.Tests.Parsers;

public sealed class ParserTensorFieldTests
{
    [Fact]
    public void ShouldExposeSingletonInstance()
    {
        var first = ParserTensorField.Instance;
        var second = ParserTensorField.Instance;

        second.ShouldBeSameAs(first);
    }

    [Fact]
    public void ShouldExposeExpectedPriority()
    {
        ParserTensorField.Instance.Priority.ShouldBe(7000);
    }

    [Fact]
    public void ShouldReturnNullWhenExpressionDoesNotContainOpeningBracket()
    {
        var token = ParserTensorField.Instance.ParseToken("F_a", RedberryParser.Default);

        token.ShouldBeNull();
    }

    [Fact]
    public void ShouldReturnNullWhenExpressionContainsTopLevelOperator()
    {
        var token = ParserTensorField.Instance.ParseToken("F[x]+G[y]", RedberryParser.Default);

        token.ShouldBeNull();
    }

    [Fact]
    public void ShouldThrowBracketsErrorWhenClosingBracketAppearsBeforeOpeningBracket()
    {
        Should.Throw<BracketsError>(() => ParserTensorField.Instance.ParseToken("F]x[", RedberryParser.Default));
    }

    [Fact]
    public void ShouldThrowParserExceptionWhenArgumentContainsMoreThanOneColon()
    {
        var exception = Should.Throw<ParserException>(() => ParserTensorField.Instance.ParseToken("F[x:_a:_b]", RedberryParser.Default));

        exception.Message.ShouldBe("F[x:_a:_b]");
    }

    [Fact]
    public void ShouldRewriteSqrtAsPowerForScalarArgument()
    {
        var token = ParserTensorField.Instance.ParseToken("sqrt[x]", RedberryParser.Default);

        var power = token.ShouldBeOfType<ParseToken>();
        power.TokenType.ShouldBe(TokenType.Power);
        power.Content.Length.ShouldBe(2);
        power.Content[0].TokenType.ShouldBe(TokenType.SimpleTensor);

        var exponent = power.Content[1].ShouldBeOfType<ParseTokenNumber>();
        exponent.Value.ShouldBe(Complex.OneHalf);
    }

    [Fact]
    public void ShouldParseSqrtWithSeveralArgumentsAsTensorField()
    {
        var token = ParserTensorField.Instance.ParseToken("sqrt[x,y]", RedberryParser.Default);

        var tensorField = token.ShouldBeOfType<ParseTokenTensorField>();
        tensorField.TokenType.ShouldBe(TokenType.TensorField);
        tensorField.Name.ShouldBe("sqrt");
    }

    [Fact]
    public void ShouldRewriteTrAsTrace()
    {
        var token = ParserTensorField.Instance.ParseToken("tr[x,y]", RedberryParser.Default);

        var trace = token.ShouldBeOfType<ParseToken>();
        trace.TokenType.ShouldBe(TokenType.Trace);
        trace.Content.Length.ShouldBe(2);
    }

    [Fact]
    public void ShouldParseTensorFieldWithArgumentIndicesAndNestedCommas()
    {
        var token = ParserTensorField.Instance.ParseToken("F[x:,Sin[g[m,n]]]", RedberryParser.Default);

        var tensorField = token.ShouldBeOfType<ParseTokenTensorField>();
        tensorField.TokenType.ShouldBe(TokenType.TensorField);
        tensorField.Name.ShouldBe("F");
        tensorField.Content.Length.ShouldBe(2);
        tensorField.Content[0].TokenType.ShouldBe(TokenType.SimpleTensor);
        tensorField.Content[1].TokenType.ShouldBe(TokenType.ScalarFunction);
        tensorField.ArgumentsIndices.Length.ShouldBe(2);
        tensorField.ArgumentsIndices[0].ShouldNotBeNull();
        tensorField.ArgumentsIndices[0].Size().ShouldBe(0);
        tensorField.ArgumentsIndices[1].ShouldBeNull();
    }
}
