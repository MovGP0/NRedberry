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
        Should.Throw<ArgumentNullException>(() => new Expression(null!, new TestTensor("a", 1), new TestTensor("b", 2)));
    }

    [Fact]
    public void ShouldThrowWhenLeftIsNull()
    {
        Should.Throw<ArgumentNullException>(() => new Expression(IndicesFactory.EmptyIndices, null!, new TestTensor("b", 2)));
    }

    [Fact]
    public void ShouldThrowWhenRightIsNull()
    {
        Should.Throw<ArgumentNullException>(() => new Expression(IndicesFactory.EmptyIndices, new TestTensor("a", 1), null!));
    }

    [Fact]
    public void ShouldExposeIndicesChildrenAndSize()
    {
        TensorType left = new TestTensor("left", 5);
        TensorType right = new TestTensor("right", 7);
        Expression expression = new(IndicesFactory.EmptyIndices, left, right);

        expression.Indices.ShouldBeSameAs(IndicesFactory.EmptyIndices);
        expression.Size.ShouldBe(2);
        expression[0].ShouldBeSameAs(left);
        expression[1].ShouldBeSameAs(right);
    }

    [Fact]
    public void ShouldThrowWhenIndexerIsOutOfRange()
    {
        Expression expression = new(IndicesFactory.EmptyIndices, new TestTensor("left", 5), new TestTensor("right", 7));

        Should.Throw<ArgumentOutOfRangeException>(() => _ = expression[2]);
    }

    [Fact]
    public void ShouldFormatUsingOutputSpecificAssignmentOperator()
    {
        Expression expression = new(IndicesFactory.EmptyIndices, new TestTensor("L", 1), new TestTensor("R", 2));

        expression.ToString(OutputFormat.Redberry).ShouldBe("L = R");
        expression.ToString(OutputFormat.Maple).ShouldBe("L := R");
    }

    [Fact]
    public void ShouldUseDocumentedHashFormula()
    {
        Expression expression = new(IndicesFactory.EmptyIndices, new TestTensor("L", 5), new TestTensor("R", 2));

        expression.GetHashCode().ShouldBe((3 * 5) - (7 * 2));
    }

    [Fact]
    public void ShouldReturnExpressionBuilderAndFactory()
    {
        Expression expression = new(IndicesFactory.EmptyIndices, new TestTensor("L", 5), new TestTensor("R", 2));

        expression.GetBuilder().ShouldBeOfType<ExpressionBuilder>();
        expression.GetFactory().ShouldBeSameAs(ExpressionFactory.Instance);
    }

    [Fact]
    public void ShouldTransposeBySwappingSides()
    {
        TensorType left = new TestTensor("L", 5);
        TensorType right = new TestTensor("R", 2);
        Expression expression = new(IndicesFactory.EmptyIndices, left, right);

        Expression transposed = expression.Transpose();

        transposed.Indices.ShouldBeSameAs(expression.Indices);
        transposed[0].ShouldBeSameAs(right);
        transposed[1].ShouldBeSameAs(left);
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
