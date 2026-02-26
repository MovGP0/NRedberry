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

        Assert.Same(first, second);
    }

    [Fact]
    public void ShouldExposeExpectedPriority()
    {
        Assert.Equal(9987, ParserFunctions.Instance.Priority);
    }

    [Fact]
    public void ShouldReturnNullWhenExpressionDoesNotContainOpeningBracket()
    {
        var token = ParserFunctions.Instance.ParseToken("Sinx", RedberryParser.Default);

        Assert.Null(token);
    }

    [Fact]
    public void ShouldReturnNullWhenExpressionDoesNotEndWithClosingBracket()
    {
        var token = ParserFunctions.Instance.ParseToken("Sin[x", RedberryParser.Default);

        Assert.Null(token);
    }

    [Fact]
    public void ShouldReturnNullWhenFunctionIsUnknown()
    {
        var token = ParserFunctions.Instance.ParseToken("Foo[x]", RedberryParser.Default);

        Assert.Null(token);
    }

    [Fact]
    public void ShouldReturnNullWhenCharacterAfterFunctionNameIsNotOpeningBracket()
    {
        var token = ParserFunctions.Instance.ParseToken("Sinx]", RedberryParser.Default);

        Assert.Null(token);
    }

    [Fact]
    public void ShouldReturnNullWhenBracketsAreInconsistentInArgument()
    {
        var token = ParserFunctions.Instance.ParseToken("Sin[]a]", RedberryParser.Default);

        Assert.Null(token);
    }

    [Fact]
    public void ShouldThrowWhenFunctionHasMultipleTopLevelArguments()
    {
        var exception = Assert.Throws<ParserException>(() => ParserFunctions.Instance.ParseToken("Sin[x,y]", RedberryParser.Default));

        Assert.Equal("Sin, Cos, Tan and others scalar functions take only one argument.", exception.Message);
    }

    [Fact]
    public void ShouldParseFunctionWhenCommaIsNestedInsideArgument()
    {
        var token = ParserFunctions.Instance.ParseToken("Sin[f[a,b]]", RedberryParser.Default);

        var scalarFunction = Assert.IsType<ParseTokenScalarFunction>(token);
        Assert.Equal("Sin", scalarFunction.Function);
        Assert.Single(scalarFunction.Content);
        Assert.Equal(TokenType.TensorField, scalarFunction.Content[0].TokenType);
    }

    [Theory]
    [MemberData(nameof(SupportedFunctions))]
    public void ShouldParseSupportedScalarFunctions(string functionName)
    {
        var token = ParserFunctions.Instance.ParseToken($"{functionName}[x]", RedberryParser.Default);

        var scalarFunction = Assert.IsType<ParseTokenScalarFunction>(token);
        Assert.Equal(TokenType.ScalarFunction, scalarFunction.TokenType);
        Assert.Equal(functionName, scalarFunction.Function);
        Assert.Single(scalarFunction.Content);
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
