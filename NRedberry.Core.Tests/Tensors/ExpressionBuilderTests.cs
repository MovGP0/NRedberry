using NRedberry.Indices;
using NRedberry.Tensors;
using IndicesType = NRedberry.Indices.Indices;
using TensorType = NRedberry.Tensors.Tensor;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class ExpressionBuilderTests
{
    [Fact]
    public void ShouldThrowWhenBuildCalledBeforeExpressionIsComplete()
    {
        ExpressionBuilder builder = new();

        Assert.Throws<InvalidOperationException>(() => builder.Build());
    }

    [Fact]
    public void ShouldThrowWhenPutArgumentIsNull()
    {
        ExpressionBuilder builder = new();

        Assert.Throws<ArgumentNullException>(() => builder.Put(null!));
    }

    [Fact]
    public void ShouldBuildExpressionFromTwoCompatibleParts()
    {
        ExpressionBuilder builder = new();
        TensorType left = new TestTensor("L", 1, IndicesFactory.Create(Lower(1)));
        TensorType right = new TestTensor("R", 2, IndicesFactory.Create(Lower(1)));

        builder.Put(left);
        builder.Put(right);

        Expression expression = Assert.IsType<Expression>(builder.Build());
        Assert.Same(left, expression[0]);
        Assert.Same(right, expression[1]);
        Assert.True(expression.Indices.EqualsRegardlessOrder(left.Indices.GetFree()));
    }

    [Fact]
    public void ShouldAllowZeroRightPartWithDifferentIndices()
    {
        ExpressionBuilder builder = new();
        TensorType left = new TestTensor("L", 1, IndicesFactory.Create(Lower(1)));

        builder.Put(left);
        builder.Put(NRedberry.Numbers.Complex.Zero);

        Expression expression = Assert.IsType<Expression>(builder.Build());
        Assert.Same(NRedberry.Numbers.Complex.Zero, expression[1]);
    }

    [Fact]
    public void ShouldThrowWhenSecondPartHasInconsistentIndices()
    {
        ExpressionBuilder builder = new();
        TensorType left = new TestTensor("L", 1, IndicesFactory.Create(Lower(1)));
        TensorType right = new TestTensor("R", 2, IndicesFactory.Create(Lower(2)));

        builder.Put(left);

        Exception? exception = Record.Exception(() => builder.Put(right));

        Assert.NotNull(exception);
        Assert.True(exception is TensorException or TypeInitializationException);
    }

    [Fact]
    public void ShouldThrowWhenMoreThanTwoPartsAreAdded()
    {
        ExpressionBuilder builder = new();
        TensorType operand = new TestTensor("T", 1, IndicesFactory.EmptyIndices);

        builder.Put(operand);
        builder.Put(operand);

        Exception? exception = Record.Exception(() => builder.Put(operand));

        Assert.NotNull(exception);
        Assert.True(exception is TensorException or TypeInitializationException);
    }

    [Fact]
    public void ShouldCloneCurrentState()
    {
        ExpressionBuilder builder = new();
        TensorType left = new TestTensor("L", 1, IndicesFactory.EmptyIndices);
        TensorType right = new TestTensor("R", 2, IndicesFactory.EmptyIndices);
        builder.Put(left);

        ExpressionBuilder clone = Assert.IsType<ExpressionBuilder>(builder.Clone());
        clone.Put(right);

        Expression expression = Assert.IsType<Expression>(clone.Build());
        Assert.Same(left, expression[0]);
        Assert.Same(right, expression[1]);
        Assert.Throws<InvalidOperationException>(() => builder.Build());
    }

    private static int Lower(int name)
    {
        return IndicesUtils.CreateIndex(name, (byte)0, false);
    }

    private sealed class TestTensor(string text, int hashCode, IndicesType indices) : TensorType
    {
        public override IndicesType Indices => indices;

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
