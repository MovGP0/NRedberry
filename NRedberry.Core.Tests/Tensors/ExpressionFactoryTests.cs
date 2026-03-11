using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Tensors;
using IndicesType = NRedberry.Indices.Indices;
using TensorType = NRedberry.Tensors.Tensor;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class ExpressionFactoryTests
{
    [Fact]
    public void ShouldExposeSingletonInstance()
    {
        Assert.Same(ExpressionFactory.Instance, ExpressionFactory.Instance);
    }

    [Fact]
    public void ShouldThrowWhenTensorArrayIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => ExpressionFactory.Instance.Create(null!));
    }

    [Fact]
    public void ShouldThrowWhenArgumentCountIsNotTwo()
    {
        Assert.Throws<ArgumentException>(() => ExpressionFactory.Instance.Create());
        Assert.Throws<ArgumentException>(() => ExpressionFactory.Instance.Create(Complex.One));
        Assert.Throws<ArgumentException>(() => ExpressionFactory.Instance.Create(Complex.One, Complex.One, Complex.One));
    }

    [Fact]
    public void ShouldThrowWhenAnyArgumentIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => ExpressionFactory.Instance.Create(null!, Complex.One));
        Assert.Throws<ArgumentNullException>(() => ExpressionFactory.Instance.Create(Complex.One, null!));
    }

    [Fact]
    public void ShouldCreateExpressionFromCompatibleArguments()
    {
        TensorType left = new TestTensor("L", 1, IndicesFactory.Create(Lower(1)));
        TensorType right = new TestTensor("R", 2, IndicesFactory.Create(Lower(1)));

        Expression expression = Assert.IsType<Expression>(ExpressionFactory.Instance.Create(left, right));

        Assert.Same(left, expression[0]);
        Assert.Same(right, expression[1]);
        Assert.True(expression.Indices.EqualsRegardlessOrder(left.Indices.GetFree()));
    }

    [Fact]
    public void ShouldAllowZeroRightArgumentWithDifferentIndices()
    {
        TensorType left = new TestTensor("L", 1, IndicesFactory.Create(Lower(1)));

        Expression expression = Assert.IsType<Expression>(ExpressionFactory.Instance.Create(left, Complex.Zero));

        Assert.Same(Complex.Zero, expression[1]);
    }

    [Fact]
    public void ShouldThrowWhenArgumentsHaveInconsistentIndices()
    {
        TensorType left = new TestTensor("L", 1, IndicesFactory.Create(Lower(1)));
        TensorType right = new TestTensor("R", 2, IndicesFactory.Create(Lower(2)));

        Exception? exception = Record.Exception(() => ExpressionFactory.Instance.Create(left, right));

        Assert.NotNull(exception);
        Assert.True(exception is TensorException or TypeInitializationException);
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
