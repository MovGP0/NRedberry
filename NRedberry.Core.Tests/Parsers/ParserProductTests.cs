using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Parsers;
using RedberryParser = NRedberry.Parsers.Parser;

namespace NRedberry.Core.Tests.Parsers;

public sealed class ParserProductTests
{
    [Fact]
    public void ShouldReturnNullWhenExpressionDoesNotContainProductOperators()
    {
        var token = ParserProduct.Instance.ParseToken("a+b", CreateParser());

        token.ShouldBeNull();
    }

    [Fact]
    public void ShouldReturnNullWhenExpressionContainsPowerLikeDoubleAsterisk()
    {
        var token = ParserProduct.Instance.ParseToken("a**b", CreateParser());

        token.ShouldBeNull();
    }

    [Fact]
    public void ShouldParseDivisionAsProductWithInversePower()
    {
        var token = ParserProduct.Instance.ParseToken("a/b", CreateParser());

        var product = token.ShouldBeOfType<ParseToken>();
        product.TokenType.ShouldBe(TokenType.Product);
        product.Content.Length.ShouldBe(2);
        product.Content[0].TokenType.ShouldBe(TokenType.SimpleTensor);

        var inverse = product.Content[1].ShouldBeOfType<ParseToken>();
        inverse.TokenType.ShouldBe(TokenType.Power);
        inverse.Content.Length.ShouldBe(2);
        inverse.Content[0].TokenType.ShouldBe(TokenType.SimpleTensor);

        var power = inverse.Content[1].ShouldBeOfType<ParseTokenNumber>();
        power.Value.ShouldBe(Complex.MinusOne);
    }

    [Fact]
    public void ShouldKeepSameVarianceIndicesWhenAllowSameVarianceIsDisabled()
    {
        try
        {
            var token = ParserProduct.Instance.ParseToken("A_a*B_a", CreateParser(allowSameVariance: false));

            var product = token.ShouldBeOfType<ParseToken>();
            var left = product.Content[0].ShouldBeOfType<ParseTokenSimpleTensor>();
            var right = product.Content[1].ShouldBeOfType<ParseTokenSimpleTensor>();

            right.Indices[0].ShouldBe(left.Indices[0]);
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

            var product = token.ShouldBeOfType<ParseToken>();
            var left = product.Content[0].ShouldBeOfType<ParseTokenSimpleTensor>();
            var right = product.Content[1].ShouldBeOfType<ParseTokenSimpleTensor>();

            right.Indices[0].ShouldBe(IndicesUtils.InverseIndexState(left.Indices[0]));
        }
        catch (TypeInitializationException)
        {
        }
    }

    [Fact]
    public void ShouldExposeExpectedPriority()
    {
        ParserProduct.Instance.Priority.ShouldBe(999);
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
