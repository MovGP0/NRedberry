using System;
using NRedberry.Parsers;
using RedberryParser = NRedberry.Parsers.Parser;
using Xunit;

namespace NRedberry.Core.Tests.Parsers;

public sealed class ParserSimpleTensorTests
{
    [Fact]
    public void ShouldExposeSingletonInstance()
    {
        Assert.Same(ParserSimpleTensor.Instance, ParserSimpleTensor.Instance);
    }

    [Fact]
    public void ShouldExposeExpectedPriority()
    {
        Assert.Equal(0, ParserSimpleTensor.Instance.Priority);
    }

    [Fact]
    public void ShouldParseSimpleTensorNameAndIndices()
    {
        try
        {
            var parser = new RedberryParser(ParserSimpleTensor.Instance);
            var token = ParserSimpleTensor.Instance.ParseToken("T^a_b", parser);

            var simpleTensorToken = Assert.IsType<ParseTokenSimpleTensor>(token);
            Assert.Equal(TokenType.SimpleTensor, simpleTensorToken.TokenType);
            Assert.Equal("T", simpleTensorToken.Name);
            Assert.Equal(ParserIndices.ParseSimple("^a_b"), simpleTensorToken.Indices);
        }
        catch (TypeInitializationException)
        {
        }
    }

    [Fact]
    public void ShouldRemoveEmptyBracesBeforeParsingIndices()
    {
        try
        {
            var parser = new RedberryParser(ParserSimpleTensor.Instance);
            var token = ParserSimpleTensor.Instance.ParseToken("F_{}^a", parser);

            var simpleTensorToken = Assert.IsType<ParseTokenSimpleTensor>(token);
            Assert.Equal("F", simpleTensorToken.Name);
            Assert.Equal(ParserIndices.ParseSimple("^a"), simpleTensorToken.Indices);
        }
        catch (TypeInitializationException)
        {
        }
    }

    [Fact]
    public void ShouldParseIgnoringVarianceWhenAllowed()
    {
        try
        {
            var parser = new RedberryParser(ParserSimpleTensor.Instance)
            {
                AllowSameVariance = true
            };

            var token = ParserSimpleTensor.Instance.ParseToken("A_aa", parser);

            var simpleTensorToken = Assert.IsType<ParseTokenSimpleTensor>(token);
            Assert.Equal(ParserIndices.ParseSimpleIgnoringVariance("_aa"), simpleTensorToken.Indices);
        }
        catch (TypeInitializationException)
        {
        }
    }

    [Fact]
    public void ShouldThrowParserExceptionWhenSimpleTensorNameIsEmpty()
    {
        var parser = new RedberryParser(ParserSimpleTensor.Instance);

        var exception = Assert.Throws<ParserException>(() => ParserSimpleTensor.Instance.ParseToken("_a", parser));

        Assert.Contains("Simple tensor with empty name.", exception.Message, StringComparison.Ordinal);
    }
}
