using NRedberry.Contexts;
using NRedberry.Numbers;
using NRedberry.Parsers;
using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;
using RedberryParser = NRedberry.Parsers.Parser;
using Xunit;

namespace NRedberry.Core.Tests.Parsers;

public sealed class ParseTokenTransformerTests
{
    [Fact]
    public void ShouldApplyTransformerToParsedNode()
    {
        var initialNode = new ParseTokenNumber(Complex.One);
        var transformedNode = new ParseTokenNumber(Complex.Two);
        var parser = new RedberryParser(new StubTokenParser(initialNode));
        var transformer = new RecordingParseTokenTransformer(transformedNode);
        var parseManager = new ParseManager(parser);

        var tensor = parseManager.Parse("x", [], [transformer]);

        var number = Assert.IsType<Complex>(tensor);
        Assert.Same(initialNode, transformer.LastInput);
        Assert.Equal(1, transformer.CallCount);
        Assert.Equal(Complex.Two, number);
    }

    [Fact]
    public void ShouldApplyTransformersInOrder()
    {
        var initialNode = new ParseTokenNumber(Complex.One);
        var firstOutput = new ParseTokenNumber(Complex.Two);
        var secondOutput = new ParseTokenNumber(new Complex(3));
        var parser = new RedberryParser(new StubTokenParser(initialNode));
        var first = new RecordingParseTokenTransformer(firstOutput);
        var second = new RecordingParseTokenTransformer(secondOutput);
        var parseManager = new ParseManager(parser);

        var tensor = parseManager.Parse("x", [first, second]);

        var number = Assert.IsType<Complex>(tensor);
        Assert.Same(initialNode, first.LastInput);
        Assert.Same(firstOutput, second.LastInput);
        Assert.Equal(1, first.CallCount);
        Assert.Equal(1, second.CallCount);
        Assert.Equal(new Complex(3), number);
    }

    [Fact]
    public void ShouldUseDefaultParserPreprocessorsWhenParsingWithoutExplicitPreprocessors()
    {
        var initialNode = new ParseTokenNumber(Complex.One);
        var transformedNode = new ParseTokenNumber(Complex.Two);
        var parser = new RedberryParser(new StubTokenParser(initialNode));
        var transformer = new RecordingParseTokenTransformer(transformedNode);
        var parseManager = new ParseManager(parser);
        parseManager.DefaultParserPreprocessors.Add(transformer);

        var tensor = parseManager.Parse("x");

        var number = Assert.IsType<Complex>(tensor);
        Assert.Equal(1, transformer.CallCount);
        Assert.Same(initialNode, transformer.LastInput);
        Assert.Equal(Complex.Two, number);
    }
}

file sealed class StubTokenParser(ParseToken node) : ITokenParser
{
    public int Priority => 0;

    public ParseToken? ParseToken(string expression, RedberryParser parser)
    {
        return node;
    }
}

file sealed class RecordingParseTokenTransformer(ParseToken output) : IParseTokenTransformer
{
    public int CallCount { get; private set; }

    public ParseToken? LastInput { get; private set; }

    public ParseToken Transform(ParseToken node)
    {
        LastInput = node;
        CallCount++;
        return output;
    }
}
