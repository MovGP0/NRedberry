using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Tensors;
using IndicesType = NRedberry.Indices.Indices;
using TensorType = NRedberry.Tensors.Tensor;

namespace NRedberry.Core.Tests.Tensors;

public sealed class ExpressionFactoryTests
{
    [Fact]
    public void ShouldExposeSingletonInstance()
    {
        ExpressionFactory.Instance.ShouldBeSameAs(ExpressionFactory.Instance);
    }

    [Fact]
    public void ShouldThrowWhenTensorArrayIsNull()
    {
        Should.Throw<ArgumentNullException>(() => ExpressionFactory.Instance.Create(null!));
    }

    [Fact]
    public void ShouldThrowWhenArgumentCountIsNotTwo()
    {
        Should.Throw<ArgumentException>(() => ExpressionFactory.Instance.Create());
        Should.Throw<ArgumentException>(() => ExpressionFactory.Instance.Create(Complex.One));
        Should.Throw<ArgumentException>(() => ExpressionFactory.Instance.Create(Complex.One, Complex.One, Complex.One));
    }

    [Fact]
    public void ShouldThrowWhenAnyArgumentIsNull()
    {
        Should.Throw<ArgumentNullException>(() => ExpressionFactory.Instance.Create(null!, Complex.One));
        Should.Throw<ArgumentNullException>(() => ExpressionFactory.Instance.Create(Complex.One, null!));
    }

    [Fact]
    public void ShouldCreateExpressionFromCompatibleArguments()
    {
        TensorType left = new TestTensor("L", 1, IndicesFactory.Create(Lower(1)));
        TensorType right = new TestTensor("R", 2, IndicesFactory.Create(Lower(1)));

        Expression expression = ExpressionFactory.Instance.Create(left, right).ShouldBeOfType<Expression>();

        expression[0].ShouldBeSameAs(left);
        expression[1].ShouldBeSameAs(right);
        expression.Indices.EqualsRegardlessOrder(left.Indices.GetFree()).ShouldBeTrue();
    }

    [Fact]
    public void ShouldAllowZeroRightArgumentWithDifferentIndices()
    {
        TensorType left = new TestTensor("L", 1, IndicesFactory.Create(Lower(1)));

        Expression expression = ExpressionFactory.Instance.Create(left, Complex.Zero).ShouldBeOfType<Expression>();

        expression[1].ShouldBeSameAs(Complex.Zero);
    }

    [Fact]
    public void ShouldThrowWhenArgumentsHaveInconsistentIndices()
    {
        TensorType left = new TestTensor("L", 1, IndicesFactory.Create(Lower(1)));
        TensorType right = new TestTensor("R", 2, IndicesFactory.Create(Lower(2)));

        Exception? exception = Record.Exception(() => ExpressionFactory.Instance.Create(left, right));

        exception.ShouldNotBeNull();
        (exception is TensorException or TypeInitializationException).ShouldBeTrue();
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
