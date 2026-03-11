using NRedberry.Indices;
using NRedberry.Tensors;
using IndicesType = NRedberry.Indices.Indices;
using TensorType = NRedberry.Tensors.Tensor;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class ExpressionTests
{
    [Fact]
    public void ShouldThrowWhenIndicesAreNull()
    {
        Assert.Throws<ArgumentNullException>(() => new Expression(null!, new TestTensor("a", 1), new TestTensor("b", 2)));
    }

    [Fact]
    public void ShouldThrowWhenLeftIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new Expression(IndicesFactory.EmptyIndices, null!, new TestTensor("b", 2)));
    }

    [Fact]
    public void ShouldThrowWhenRightIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new Expression(IndicesFactory.EmptyIndices, new TestTensor("a", 1), null!));
    }

    [Fact]
    public void ShouldExposeIndicesChildrenAndSize()
    {
        TensorType left = new TestTensor("left", 5);
        TensorType right = new TestTensor("right", 7);
        Expression expression = new(IndicesFactory.EmptyIndices, left, right);

        Assert.Same(IndicesFactory.EmptyIndices, expression.Indices);
        Assert.Equal(2, expression.Size);
        Assert.Same(left, expression[0]);
        Assert.Same(right, expression[1]);
    }

    [Fact]
    public void ShouldThrowWhenIndexerIsOutOfRange()
    {
        Expression expression = new(IndicesFactory.EmptyIndices, new TestTensor("left", 5), new TestTensor("right", 7));

        Assert.Throws<ArgumentOutOfRangeException>(() => _ = expression[2]);
    }

    [Fact]
    public void ShouldFormatUsingOutputSpecificAssignmentOperator()
    {
        Expression expression = new(IndicesFactory.EmptyIndices, new TestTensor("L", 1), new TestTensor("R", 2));

        Assert.Equal("L = R", expression.ToString(OutputFormat.Redberry));
        Assert.Equal("L := R", expression.ToString(OutputFormat.Maple));
    }

    [Fact]
    public void ShouldUseDocumentedHashFormula()
    {
        Expression expression = new(IndicesFactory.EmptyIndices, new TestTensor("L", 5), new TestTensor("R", 2));

        Assert.Equal((3 * 5) - (7 * 2), expression.GetHashCode());
    }

    [Fact]
    public void ShouldReturnExpressionBuilderAndFactory()
    {
        Expression expression = new(IndicesFactory.EmptyIndices, new TestTensor("L", 5), new TestTensor("R", 2));

        Assert.IsType<ExpressionBuilder>(expression.GetBuilder());
        Assert.Same(ExpressionFactory.Instance, expression.GetFactory());
    }

    [Fact]
    public void ShouldTransposeBySwappingSides()
    {
        TensorType left = new TestTensor("L", 5);
        TensorType right = new TestTensor("R", 2);
        Expression expression = new(IndicesFactory.EmptyIndices, left, right);

        Expression transposed = expression.Transpose();

        Assert.Same(expression.Indices, transposed.Indices);
        Assert.Same(right, transposed[0]);
        Assert.Same(left, transposed[1]);
    }

    private sealed class TestTensor(string text, int hashCode) : TensorType
    {
        public override IndicesType Indices => IndicesFactory.EmptyIndices;

        public override TensorType this[int i] => throw new IndexOutOfRangeException();

        public override int Size => 0;

        public override string ToString(OutputFormat outputFormat)
        {
            return text;
        }

        public override int GetHashCode()
        {
            return hashCode;
        }

        public override TensorBuilder GetBuilder()
        {
            throw new NotSupportedException();
        }

        public override TensorFactory? GetFactory()
        {
            return null;
        }
    }
}
