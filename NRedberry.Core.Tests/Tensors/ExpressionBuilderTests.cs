using NRedberry.Indices;
using NRedberry.Tensors;
using IndicesType = NRedberry.Indices.Indices;
using TensorType = NRedberry.Tensors.Tensor;

namespace NRedberry.Core.Tests.Tensors;

public sealed class ExpressionBuilderTests
{
    [Fact]
    public void ShouldThrowWhenBuildCalledBeforeExpressionIsComplete()
    {
        ExpressionBuilder builder = new();

        Should.Throw<InvalidOperationException>(() => builder.Build());
    }

    [Fact]
    public void ShouldThrowWhenPutArgumentIsNull()
    {
        ExpressionBuilder builder = new();

        Should.Throw<ArgumentNullException>(() => builder.Put(null!));
    }

    [Fact]
    public void ShouldBuildExpressionFromTwoCompatibleParts()
    {
        ExpressionBuilder builder = new();
        TensorType left = new TestTensor("L", 1, IndicesFactory.Create(Lower(1)));
        TensorType right = new TestTensor("R", 2, IndicesFactory.Create(Lower(1)));

        builder.Put(left);
        builder.Put(right);

        Expression expression = builder.Build().ShouldBeOfType<Expression>();
        expression[0].ShouldBeSameAs(left);
        expression[1].ShouldBeSameAs(right);
        expression.Indices.EqualsRegardlessOrder(left.Indices.GetFree()).ShouldBeTrue();
    }

    [Fact]
    public void ShouldAllowZeroRightPartWithDifferentIndices()
    {
        ExpressionBuilder builder = new();
        TensorType left = new TestTensor("L", 1, IndicesFactory.Create(Lower(1)));

        builder.Put(left);
        builder.Put(NRedberry.Numbers.Complex.Zero);

        Expression expression = builder.Build().ShouldBeOfType<Expression>();
        expression[1].ShouldBeSameAs(NRedberry.Numbers.Complex.Zero);
    }

    [Fact]
    public void ShouldThrowWhenSecondPartHasInconsistentIndices()
    {
        ExpressionBuilder builder = new();
        TensorType left = new TestTensor("L", 1, IndicesFactory.Create(Lower(1)));
        TensorType right = new TestTensor("R", 2, IndicesFactory.Create(Lower(2)));

        builder.Put(left);

        Exception? exception = Record.Exception(() => builder.Put(right));

        exception.ShouldNotBeNull();
        (exception is TensorException or TypeInitializationException).ShouldBeTrue();
    }

    [Fact]
    public void ShouldThrowWhenMoreThanTwoPartsAreAdded()
    {
        ExpressionBuilder builder = new();
        TensorType operand = new TestTensor("T", 1, IndicesFactory.EmptyIndices);

        builder.Put(operand);
        builder.Put(operand);

        Exception? exception = Record.Exception(() => builder.Put(operand));

        exception.ShouldNotBeNull();
        (exception is TensorException or TypeInitializationException).ShouldBeTrue();
    }

    [Fact]
    public void ShouldCloneCurrentState()
    {
        ExpressionBuilder builder = new();
        TensorType left = new TestTensor("L", 1, IndicesFactory.EmptyIndices);
        TensorType right = new TestTensor("R", 2, IndicesFactory.EmptyIndices);
        builder.Put(left);

        ExpressionBuilder clone = builder.Clone().ShouldBeOfType<ExpressionBuilder>();
        clone.Put(right);

        Expression expression = clone.Build().ShouldBeOfType<Expression>();
        expression[0].ShouldBeSameAs(left);
        expression[1].ShouldBeSameAs(right);
        Should.Throw<InvalidOperationException>(() => builder.Build());
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
