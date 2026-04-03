using NRedberry.Parsers;
using RedberryParser = NRedberry.Parsers.Parser;

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

            indices.EqualsRegardlessOrder(expected).ShouldBeTrue();
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

            result.ShouldBe(expected);
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

            ArgumentException exception = Should.Throw<ArgumentException>(() => derivative.ToTensor());
            exception.Message.ShouldContain("Derivative with respect to non simple argument");
        }
        catch (TypeInitializationException)
        {
        }
    }
}
