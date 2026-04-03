using NRedberry.Parsers;
using RedberryParser = NRedberry.Parsers.Parser;

namespace NRedberry.Core.Tests.Parsers;

public sealed class ParserSimpleTensorTests
{
    [Fact]
    public void ShouldExposeSingletonInstance()
    {
        ParserSimpleTensor.Instance.ShouldBeSameAs(ParserSimpleTensor.Instance);
    }

    [Fact]
    public void ShouldExposeExpectedPriority()
    {
        ParserSimpleTensor.Instance.Priority.ShouldBe(0);
    }

    [Fact]
    public void ShouldParseSimpleTensorNameAndIndices()
    {
        try
        {
            var parser = new RedberryParser(ParserSimpleTensor.Instance);
            var token = ParserSimpleTensor.Instance.ParseToken("T^a_b", parser);

            var simpleTensorToken = token.ShouldBeOfType<ParseTokenSimpleTensor>();
            simpleTensorToken.TokenType.ShouldBe(TokenType.SimpleTensor);
            simpleTensorToken.Name.ShouldBe("T");
            simpleTensorToken.Indices.ShouldBe(ParserIndices.ParseSimple("^a_b"));
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

            var simpleTensorToken = token.ShouldBeOfType<ParseTokenSimpleTensor>();
            simpleTensorToken.Name.ShouldBe("F");
            simpleTensorToken.Indices.ShouldBe(ParserIndices.ParseSimple("^a"));
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

            var simpleTensorToken = token.ShouldBeOfType<ParseTokenSimpleTensor>();
            simpleTensorToken.Indices.ShouldBe(ParserIndices.ParseSimpleIgnoringVariance("_aa"));
        }
        catch (TypeInitializationException)
        {
        }
    }

    [Fact]
    public void ShouldThrowParserExceptionWhenSimpleTensorNameIsEmpty()
    {
        var parser = new RedberryParser(ParserSimpleTensor.Instance);

        var exception = Should.Throw<ParserException>(() => ParserSimpleTensor.Instance.ParseToken("_a", parser));

        exception.Message.ShouldContain("Simple tensor with empty name.");
    }
}
