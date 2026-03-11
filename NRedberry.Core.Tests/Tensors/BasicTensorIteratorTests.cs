using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Tensors;
using IndicesType = NRedberry.Indices.Indices;
using TensorType = NRedberry.Tensors.Tensor;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class BasicTensorIteratorTests
{
    [Fact]
    public void ShouldThrowWhenTensorIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new BasicTensorIterator(null!));
    }

    [Fact]
    public void ShouldThrowWhenCurrentIsReadBeforeMoveNext()
    {
        BasicTensorIterator iterator = new(new TestTensor(Complex.One));

        Assert.Throws<InvalidOperationException>(() => _ = iterator.Current);
    }

    [Fact]
    public void ShouldEnumerateChildrenInOrder()
    {
        TensorType[] children = [Complex.One, Complex.Two, Complex.Four];
        BasicTensorIterator iterator = new(new TestTensor(children));

        List<TensorType> visited = [];
        while (iterator.MoveNext())
        {
            visited.Add(iterator.Current);
        }

        Assert.Equal(children, visited);
    }

    [Fact]
    public void ShouldSupportReset()
    {
        BasicTensorIterator iterator = new(new TestTensor(Complex.One, Complex.Two));

        Assert.True(iterator.MoveNext());
        Assert.Same(Complex.One, iterator.Current);
        Assert.True(iterator.MoveNext());
        Assert.Same(Complex.Two, iterator.Current);

        iterator.Reset();

        Assert.Throws<InvalidOperationException>(() => _ = iterator.Current);
        Assert.True(iterator.MoveNext());
        Assert.Same(Complex.One, iterator.Current);
    }

    [Fact]
    public void ShouldHandleEmptyTensor()
    {
        BasicTensorIterator iterator = new(new TestTensor());

        Assert.False(iterator.MoveNext());
        Assert.Throws<InvalidOperationException>(() => _ = iterator.Current);
    }

    private sealed class TestTensor(params TensorType[] children) : TensorType
    {
        private readonly TensorType[] _children = children;

        public override IndicesType Indices => IndicesFactory.EmptyIndices;

        public override TensorType this[int i] => _children[i];

        public override int Size => _children.Length;

        public override string ToString(OutputFormat outputFormat)
        {
            return "test";
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
