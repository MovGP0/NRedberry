using NRedberry.Parsers;
using NRedberry.Numbers;
using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;
using RedberryTensor = NRedberry.Tensors.Tensor;

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

        indices.ShouldBe(lhs.GetIndices().GetFree());
    }

    [Fact]
    public void ShouldCreateExpressionWithoutPreprocessingWhenPreprocessIsFalse()
    {
        var lhs = new ParseTokenNumber(Complex.One);
        var rhs = new ParseTokenNumber(Complex.Two);
        var token = new ParseTokenExpression(preprocess: false, lhs, rhs);

        var tensor = token.ToTensor();

        var expression = tensor.ShouldBeOfType<Expression>();
        expression[0].ShouldBeSameAs(Complex.One);
        expression[1].ShouldBeSameAs(Complex.Two);
        expression.ToString(OutputFormat.Redberry).ShouldBe("1 = 2");
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

            var expression = tensor.ShouldBeOfType<Expression>();
            transformation.CallCount.ShouldBe(1);
            preprocessors.Count.ShouldBe(2);
            preprocessors[0].ShouldBeSameAs(transformation);
            preprocessors[1].ShouldBeSameAs(expression);
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
