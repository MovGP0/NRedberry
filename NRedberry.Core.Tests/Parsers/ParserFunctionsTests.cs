using System.Collections.Generic;
using NRedberry.Parsers;
using RedberryParser = NRedberry.Parsers.Parser;
using Xunit;

namespace NRedberry.Core.Tests.Parsers;

public sealed class ParserFunctionsTests
{
    [Fact]
    public void ShouldExposeSingletonInstance()
    {
        var first = ParserFunctions.Instance;
        var second = ParserFunctions.Instance;

        second.ShouldBeSameAs(first);
    }

    [Fact]
    public void ShouldExposeExpectedPriority()
    {
        ParserFunctions.Instance.Priority.ShouldBe(9987);
    }

    [Fact]
    public void ShouldReturnNullWhenExpressionDoesNotContainOpeningBracket()
    {
        var token = ParserFunctions.Instance.ParseToken("Sinx", RedberryParser.Default);

        token.ShouldBeNull();
    }

    [Fact]
    public void ShouldReturnNullWhenExpressionDoesNotEndWithClosingBracket()
    {
        var token = ParserFunctions.Instance.ParseToken("Sin[x", RedberryParser.Default);

        token.ShouldBeNull();
    }

    [Fact]
    public void ShouldReturnNullWhenFunctionIsUnknown()
    {
        var token = ParserFunctions.Instance.ParseToken("Foo[x]", RedberryParser.Default);

        token.ShouldBeNull();
    }

    [Fact]
    public void ShouldReturnNullWhenCharacterAfterFunctionNameIsNotOpeningBracket()
    {
        var token = ParserFunctions.Instance.ParseToken("Sinx]", RedberryParser.Default);

        token.ShouldBeNull();
    }

    [Fact]
    public void ShouldReturnNullWhenBracketsAreInconsistentInArgument()
    {
        var token = ParserFunctions.Instance.ParseToken("Sin[]a]", RedberryParser.Default);

        token.ShouldBeNull();
    }

    [Fact]
    public void ShouldThrowWhenFunctionHasMultipleTopLevelArguments()
    {
        var exception = Should.Throw<ParserException>(() => ParserFunctions.Instance.ParseToken("Sin[x,y]", RedberryParser.Default));

        exception.Message.ShouldBe("Sin, Cos, Tan and others scalar functions take only one argument.");
    }

    [Fact]
    public void ShouldParseFunctionWhenCommaIsNestedInsideArgument()
    {
        var token = ParserFunctions.Instance.ParseToken("Sin[f[a,b]]", RedberryParser.Default);

        var scalarFunction = token.ShouldBeOfType<ParseTokenScalarFunction>();
        scalarFunction.Function.ShouldBe("Sin");
        scalarFunction.Content.ShouldHaveSingleItem();
        scalarFunction.Content[0].TokenType.ShouldBe(TokenType.TensorField);
    }

    [Theory]
    [MemberData(nameof(SupportedFunctions))]
    public void ShouldParseSupportedScalarFunctions(string functionName)
    {
        var token = ParserFunctions.Instance.ParseToken($"{functionName}[x]", RedberryParser.Default);

        var scalarFunction = token.ShouldBeOfType<ParseTokenScalarFunction>();
        scalarFunction.TokenType.ShouldBe(TokenType.ScalarFunction);
        scalarFunction.Function.ShouldBe(functionName);
        scalarFunction.Content.ShouldHaveSingleItem();
    }

    public static IEnumerable<object[]> SupportedFunctions()
    {
        yield return ["Sin"];
        yield return ["Cos"];
        yield return ["Tan"];
        yield return ["Log"];
        yield return ["Exp"];
        yield return ["Cot"];
        yield return ["ArcSin"];
        yield return ["ArcCos"];
        yield return ["ArcTan"];
        yield return ["ArcCot"];
    }
}
