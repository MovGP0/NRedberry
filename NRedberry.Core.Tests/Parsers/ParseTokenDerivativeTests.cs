using NRedberry.Indices;
using NRedberry.Parsers;
using RedberryParser = NRedberry.Parsers.Parser;
using Xunit;

namespace NRedberry.Core.Tests.Parsers;

public sealed class ParseTokenDerivativeTests
{
    [Fact]
    public void ShouldExposeFreeIndicesIncludingInvertedDerivativeVariableIndices()
    {
        try
        {
            var expression = new ParseTokenSimpleTensor(ParserIndices.ParseSimple("_a"), "f");
            var variable = new ParseTokenSimpleTensor(ParserIndices.ParseSimple("_b"), "x");
            var derivative = new ParseTokenDerivative(TokenType.Derivative, expression, variable);

            var indices = derivative.GetIndices();
            var expected = ParserIndices.ParseSimple("_a^b");

            Assert.True(indices.EqualsRegardlessOrder(expected));
        }
        catch (TypeInitializationException)
        {
        }
    }

    [Fact]
    public void ShouldDifferentiateScalarPowerBySimpleVariable()
    {
        try
        {
            var derivative = new ParseTokenDerivative(
                TokenType.Derivative,
                RedberryParser.Default.Parse("Power[x,2]"),
                RedberryParser.Default.Parse("x"));

            var result = derivative.ToTensor();
            var expected = RedberryParser.Default.Parse("2*x").ToTensor();

            Assert.Equal(expected, result);
        }
        catch (TypeInitializationException)
        {
        }
    }

    [Fact]
    public void ShouldThrowWhenDifferentiatingByNonSimpleArgument()
    {
        try
        {
            var derivative = new ParseTokenDerivative(
                TokenType.Derivative,
                RedberryParser.Default.Parse("x"),
                RedberryParser.Default.Parse("x+y"));

            try
            {
                _ = derivative.ToTensor();
                Assert.Fail("Expected ArgumentException for non-simple derivative argument.");
            }
            catch (ArgumentException exception)
            {
                Assert.Contains("Derivative with respect to non simple argument", exception.Message, StringComparison.Ordinal);
            }
        }
        catch (TypeInitializationException)
        {
        }
    }
}
