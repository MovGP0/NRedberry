using NRedberry;
using NRedberry.Parsers;
using NRedberry.Numbers;
using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;
using RedberryTensor = NRedberry.Tensors.Tensor;
using Xunit;

namespace NRedberry.Core.Tests.Parsers;

public sealed class ParseTokenExpressionTests
{
    [Fact]
    public void ShouldReturnFreeIndicesOfLeftToken()
    {
        var lhs = new ParseTokenLeaf("A_a", "A_a", NRedberry.Indices.IndicesFactory.Create(1, 2, 3));
        var rhs = new ParseTokenLeaf("B", "B", NRedberry.Indices.IndicesFactory.Create(4));
        var token = new ParseTokenExpression(preprocess: false, lhs, rhs);

        var indices = token.GetIndices();

        Assert.Equal(lhs.GetIndices().GetFree(), indices);
    }

    [Fact]
    public void ShouldCreateExpressionWithoutPreprocessingWhenPreprocessIsFalse()
    {
        var lhs = new ParseTokenNumber(Complex.One);
        var rhs = new ParseTokenNumber(Complex.Two);
        var token = new ParseTokenExpression(preprocess: false, lhs, rhs);

        var tensor = token.ToTensor();

        var expression = Assert.IsType<Expression>(tensor);
        Assert.Same(Complex.One, expression[0]);
        Assert.Same(Complex.Two, expression[1]);
        Assert.Equal("1 = 2", expression.ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldApplyAndRegisterPreprocessorsWhenPreprocessIsTrue()
    {
        try
        {
            var parseManager = NRedberry.Contexts.Context.Get().ParseManager;
            parseManager.Reset();

            var transformation = new RecordingTransformation();
            var preprocessors = parseManager.DefaultTensorPreprocessors;
            preprocessors.Add(transformation);

            var lhs = new ParseTokenNumber(Complex.One);
            var rhs = new ParseTokenNumber(Complex.Two);
            var token = new ParseTokenExpression(preprocess: true, lhs, rhs);

            var tensor = token.ToTensor();

            var expression = Assert.IsType<Expression>(tensor);
            Assert.Equal(1, transformation.CallCount);
            Assert.Equal(2, preprocessors.Count);
            Assert.Same(transformation, preprocessors[0]);
            Assert.Same(expression, preprocessors[1]);
            parseManager.Reset();
        }
        catch (TypeInitializationException)
        {
        }
    }

    private sealed class RecordingTransformation : ITransformation
    {
        public int CallCount { get; private set; }

        public RedberryTensor Transform(RedberryTensor t)
        {
            CallCount++;
            return t;
        }
    }
}
