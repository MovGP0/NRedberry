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

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ShouldThrowArgumentNullExceptionWhenBracketExpressionIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => ParseUtils.CheckBracketsConsistence(null!));
    }

    [Fact]
    public void ShouldConvertNumberTensorToNumberToken()
    {
        var token = ParseUtils.TensorToAst(Complex.One);

        var numberToken = Assert.IsType<ParseTokenNumber>(token);
        Assert.Same(Complex.One, numberToken.Value);
    }

    [Fact]
    public void ShouldConvertExpressionTensorToExpressionToken()
    {
        var expression = TensorFactory.Expression(Complex.One, Complex.Two);

        var token = ParseUtils.TensorToAst(expression);

        var expressionToken = Assert.IsType<ParseTokenExpression>(token);
        Assert.False(expressionToken.Preprocess);
        var left = Assert.IsType<ParseTokenNumber>(expressionToken.Content[0]);
        var right = Assert.IsType<ParseTokenNumber>(expressionToken.Content[1]);
        Assert.Same(Complex.One, left.Value);
        Assert.Same(Complex.Two, right.Value);
    }

    [Fact]
    public void ShouldConvertSimpleTensorToSimpleTensorToken()
    {
        try
        {
            var simpleTensor = TensorFactory.ParseSimple("A");

            var token = ParseUtils.TensorToAst(simpleTensor);

            var simpleToken = Assert.IsType<ParseTokenSimpleTensor>(token);
            Assert.Equal(simpleTensor.GetStringName(), simpleToken.Name);
            Assert.Equal(simpleTensor.SimpleIndices, simpleToken.Indices);
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
            var tensorField = Assert.IsType<TensorField>(TensorFactory.Parse("F[A]"));

            var token = ParseUtils.TensorToAst(tensorField);

            var fieldToken = Assert.IsType<ParseTokenTensorField>(token);
            Assert.Equal(tensorField.GetStringName(), fieldToken.Name);
            Assert.Single(fieldToken.Content);
            Assert.Single(fieldToken.ArgumentsIndices);
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
            var scalarFunction = Assert.IsAssignableFrom<ScalarFunction>(TensorFactory.Parse("Sin[A]"));

            var token = ParseUtils.TensorToAst(scalarFunction);

            var functionToken = Assert.IsType<ParseTokenScalarFunction>(token);
            Assert.Equal(scalarFunction.GetType().Name, functionToken.Function);
            Assert.Single(functionToken.Content);
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

        Assert.True(new HashSet<int> { 1, 2, 4 }.SetEquals(allIndices));
        Assert.True(new HashSet<int> { 1, 2, 4 }.SetEquals(allIndicesT));
        Assert.DoesNotContain(33, allIndices);
        Assert.DoesNotContain(33, allIndicesT);
    }

    [Fact]
    public void ShouldThrowArgumentNullExceptionWhenGetAllIndicesNodeIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => ParseUtils.GetAllIndices(null!));
        Assert.Throws<ArgumentNullException>(() => ParseUtils.GetAllIndicesT(null!));
    }
}
