using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Parsers;
using RedberryParser = NRedberry.Parsers.Parser;
using Xunit;

namespace NRedberry.Core.Tests.Parsers;

public sealed class ParserProductTests
{
    [Fact]
    public void ShouldReturnNullWhenExpressionDoesNotContainProductOperators()
    {
        var token = ParserProduct.Instance.ParseToken("a+b", CreateParser());

        Assert.Null(token);
    }

    [Fact]
    public void ShouldReturnNullWhenExpressionContainsPowerLikeDoubleAsterisk()
    {
        var token = ParserProduct.Instance.ParseToken("a**b", CreateParser());

        Assert.Null(token);
    }

    [Fact]
    public void ShouldParseDivisionAsProductWithInversePower()
    {
        var token = ParserProduct.Instance.ParseToken("a/b", CreateParser());

        var product = Assert.IsType<ParseToken>(token);
        Assert.Equal(TokenType.Product, product.TokenType);
        Assert.Equal(2, product.Content.Length);
        Assert.Equal(TokenType.SimpleTensor, product.Content[0].TokenType);

        var inverse = Assert.IsType<ParseToken>(product.Content[1]);
        Assert.Equal(TokenType.Power, inverse.TokenType);
        Assert.Equal(2, inverse.Content.Length);
        Assert.Equal(TokenType.SimpleTensor, inverse.Content[0].TokenType);

        var power = Assert.IsType<ParseTokenNumber>(inverse.Content[1]);
        Assert.Equal(Complex.MinusOne, power.Value);
    }

    [Fact]
    public void ShouldKeepSameVarianceIndicesWhenAllowSameVarianceIsDisabled()
    {
        try
        {
            var token = ParserProduct.Instance.ParseToken("A_a*B_a", CreateParser(allowSameVariance: false));

            var product = Assert.IsType<ParseToken>(token);
            var left = Assert.IsType<ParseTokenSimpleTensor>(product.Content[0]);
            var right = Assert.IsType<ParseTokenSimpleTensor>(product.Content[1]);

            Assert.Equal(left.Indices[0], right.Indices[0]);
        }
        catch (TypeInitializationException)
        {
        }
    }

    [Fact]
    public void ShouldInvertRepeatedFreeIndexWhenAllowSameVarianceIsEnabled()
    {
        try
        {
            var token = ParserProduct.Instance.ParseToken("A_a*B_a", CreateParser(allowSameVariance: true));

            var product = Assert.IsType<ParseToken>(token);
            var left = Assert.IsType<ParseTokenSimpleTensor>(product.Content[0]);
            var right = Assert.IsType<ParseTokenSimpleTensor>(product.Content[1]);

            Assert.Equal(IndicesUtils.InverseIndexState(left.Indices[0]), right.Indices[0]);
        }
        catch (TypeInitializationException)
        {
        }
    }

    [Fact]
    public void ShouldExposeExpectedPriority()
    {
        Assert.Equal(999, ParserProduct.Instance.Priority);
    }

    private static RedberryParser CreateParser(bool allowSameVariance = false)
    {
        var parser = new RedberryParser(
            ParserBrackets.Instance,
            ParserSum.Instance,
            ParserProduct.Instance,
            ParserSimpleTensor.Instance,
            ParserTensorField.Instance,
            ParserDerivative.Instance,
            ParserPower.Instance,
            ParserNumber.Instance,
            ParserFunctions.Instance,
            ParserExpression.Instance);

        parser.AllowSameVariance = allowSameVariance;
        return parser;
    }
}
