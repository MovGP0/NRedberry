using System.Collections.Generic;
using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Parsers;
using NRedberry.Tensors;
using NRedberry.Tensors.Functions;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Parsers;

public sealed class ParseUtilsTests
{
    [Theory]
    [InlineData("A*(B+C)", true)]
    [InlineData("[A_{a} + B^{b}]", true)]
    [InlineData("A*(B+C]", false)]
    [InlineData("A*(B+C))", false)]
    [InlineData("{A[B(C)]}", true)]
    public void ShouldValidateBracketConsistency(string expression, bool expected)
    {
        var actual = ParseUtils.CheckBracketsConsistence(expression);

        actual.ShouldBe(expected);
    }

    [Fact]
    public void ShouldThrowArgumentNullExceptionWhenBracketExpressionIsNull()
    {
        Should.Throw<ArgumentNullException>(() => ParseUtils.CheckBracketsConsistence(null!));
    }

    [Fact]
    public void ShouldConvertNumberTensorToNumberToken()
    {
        var token = ParseUtils.TensorToAst(Complex.One);

        var numberToken = token.ShouldBeOfType<ParseTokenNumber>();
        numberToken.Value.ShouldBeSameAs(Complex.One);
    }

    [Fact]
    public void ShouldConvertExpressionTensorToExpressionToken()
    {
        var expression = TensorFactory.Expression(Complex.One, Complex.Two);

        var token = ParseUtils.TensorToAst(expression);

        var expressionToken = token.ShouldBeOfType<ParseTokenExpression>();
        expressionToken.Preprocess.ShouldBeFalse();
        var left = expressionToken.Content[0].ShouldBeOfType<ParseTokenNumber>();
        var right = expressionToken.Content[1].ShouldBeOfType<ParseTokenNumber>();
        left.Value.ShouldBeSameAs(Complex.One);
        right.Value.ShouldBeSameAs(Complex.Two);
    }

    [Fact]
    public void ShouldConvertSimpleTensorToSimpleTensorToken()
    {
        try
        {
            var simpleTensor = TensorFactory.ParseSimple("A");

            var token = ParseUtils.TensorToAst(simpleTensor);

            var simpleToken = token.ShouldBeOfType<ParseTokenSimpleTensor>();
            simpleToken.Name.ShouldBe(simpleTensor.GetStringName());
            simpleToken.Indices.ShouldBe(simpleTensor.SimpleIndices);
        }
        catch (TypeInitializationException)
        {
        }
    }

    [Fact]
    public void ShouldConvertTensorFieldToTensorFieldToken()
    {
        try
        {
            var tensorField = TensorFactory.Parse("F[A]").ShouldBeOfType<TensorField>();

            var token = ParseUtils.TensorToAst(tensorField);

            var fieldToken = token.ShouldBeOfType<ParseTokenTensorField>();
            fieldToken.Name.ShouldBe(tensorField.GetStringName());
            fieldToken.Content.ShouldHaveSingleItem();
            fieldToken.ArgumentsIndices.ShouldHaveSingleItem();
        }
        catch (TypeInitializationException)
        {
        }
    }

    [Fact]
    public void ShouldConvertScalarFunctionToScalarFunctionToken()
    {
        try
        {
            var scalarFunction = TensorFactory.Parse("Sin[A]").ShouldBeAssignableTo<ScalarFunction>();

            var token = ParseUtils.TensorToAst(scalarFunction);

            var functionToken = token.ShouldBeOfType<ParseTokenScalarFunction>();
            functionToken.Function.ShouldBe(scalarFunction.GetType().Name);
            functionToken.Content.ShouldHaveSingleItem();
        }
        catch (TypeInitializationException)
        {
        }
    }

    [Fact]
    public void ShouldCollectAllIndicesAndSkipScalarFunctionArguments()
    {
        var visibleA = new ParseTokenSimpleTensor(
            IndicesFactory.CreateSimple(null, 1, IndicesUtils.InverseIndexState(1), 2),
            "A");
        var hidden = new ParseTokenSimpleTensor(IndicesFactory.CreateSimple(null, 33), "Hidden");
        var scalar = new ParseTokenScalarFunction("Sin", hidden);
        var visibleB = new ParseTokenSimpleTensor(IndicesFactory.CreateSimple(null, 4), "B");
        var root = new ParseToken(TokenType.Product, visibleA, scalar, visibleB);

        var allIndices = ParseUtils.GetAllIndices(root);
        var allIndicesT = ParseUtils.GetAllIndicesT(root);

        new HashSet<int> { 1, 2, 4 }.SetEquals(allIndices).ShouldBeTrue();
        new HashSet<int> { 1, 2, 4 }.SetEquals(allIndicesT).ShouldBeTrue();
        allIndices.ShouldNotContain(33);
        allIndicesT.ShouldNotContain(33);
    }

    [Fact]
    public void ShouldThrowArgumentNullExceptionWhenGetAllIndicesNodeIsNull()
    {
        Should.Throw<ArgumentNullException>(() => ParseUtils.GetAllIndices(null!));
        Should.Throw<ArgumentNullException>(() => ParseUtils.GetAllIndicesT(null!));
    }
}
