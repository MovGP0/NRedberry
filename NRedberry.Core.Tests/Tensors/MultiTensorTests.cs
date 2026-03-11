using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Tensors;
using TensorType = NRedberry.Tensors.Tensor;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class MultiTensorTests
{
    [Fact]
    public void ShouldRemoveFirstMatchingTensor()
    {
        TensorType child = Complex.One;
        TestMultiTensor tensor = new([child, Complex.Two, child]);

        TensorType result = tensor.Remove(child);

        Assert.IsType<TestMultiTensor>(result);
        Assert.Equal(2, ((TestMultiTensor)result).Size);
        Assert.Same(Complex.Two, result[0]);
        Assert.Same(child, result[1]);
    }

    [Fact]
    public void ShouldReturnProvidedTensorWhenTensorIsNotPresent()
    {
        TestMultiTensor tensor = new([Complex.One, Complex.Two]);
        TensorType missing = Complex.Four;

        TensorType result = tensor.Remove(missing);

        Assert.Same(missing, result);
    }

    [Fact]
    public void ShouldReturnSameInstanceWhenRemovePositionsIsEmpty()
    {
        TestMultiTensor tensor = new([Complex.One, Complex.Two]);

        TensorType result = tensor.Remove([]);

        Assert.Same(tensor, result);
    }

    [Fact]
    public void ShouldThrowWhenRemovePositionIsOutOfRange()
    {
        TestMultiTensor tensor = new([Complex.One, Complex.Two]);

        Assert.Throws<IndexOutOfRangeException>(() => tensor.Remove([2]));
        Assert.Throws<IndexOutOfRangeException>(() => tensor.Remove([-1]));
    }

    [Fact]
    public void ShouldReturnNeutralWhenRemovingAllPositionsAfterNormalization()
    {
        TestMultiTensor tensor = new([Complex.One, Complex.Two, Complex.Four]);

        TensorType result = tensor.Remove([2, 0, 1, 1]);

        Assert.Same(Complex.Zero, result);
    }

    [Fact]
    public void ShouldPassSortedDistinctPositionsToRemoveCore()
    {
        TestMultiTensor tensor = new([Complex.One, Complex.Two, Complex.Four, Complex.ImaginaryOne]);

        TensorType result = tensor.Remove([3, 1, 3]);

        Assert.Same(tensor.RemoveMarker, result);
        Assert.NotNull(tensor.LastRemovedPositions);
        Assert.Equal([1, 3], tensor.LastRemovedPositions);
    }

    [Fact]
    public void ShouldReturnNeutralWhenSelectPositionsIsEmpty()
    {
        TestMultiTensor tensor = new([Complex.One, Complex.Two]);

        TensorType result = tensor.Select([]);

        Assert.Same(Complex.Zero, result);
    }

    [Fact]
    public void ShouldReturnSelectedChildWhenSinglePositionIsProvided()
    {
        TestMultiTensor tensor = new([Complex.One, Complex.Two, Complex.Four]);

        TensorType result = tensor.Select([1]);

        Assert.Same(Complex.Two, result);
    }

    [Fact]
    public void ShouldReturnSameInstanceWhenSelectingAllPositionsAfterNormalization()
    {
        TestMultiTensor tensor = new([Complex.One, Complex.Two, Complex.Four]);

        TensorType result = tensor.Select([2, 0, 1, 1]);

        Assert.Same(tensor, result);
    }

    [Fact]
    public void ShouldPassSortedDistinctPositionsToSelectCore()
    {
        TestMultiTensor tensor = new([Complex.One, Complex.Two, Complex.Four, Complex.ImaginaryOne]);

        TensorType result = tensor.Select([3, 1, 3]);

        Assert.Same(tensor.SelectMarker, result);
        Assert.NotNull(tensor.LastSelectedPositions);
        Assert.Equal([1, 3], tensor.LastSelectedPositions);
    }

    private sealed class TestMultiTensor(TensorType[] children) : MultiTensor(IndicesFactory.EmptyIndices)
    {
        private readonly TensorType[] _children = children;

        public int[]? LastRemovedPositions { get; private set; }

        public int[]? LastSelectedPositions { get; private set; }

        public TensorType RemoveMarker { get; } = Complex.MinusOne;

        public TensorType SelectMarker { get; } = Complex.ImaginaryOne;

        public override TensorType this[int i] => _children[i];

        public override int Size => _children.Length;

        public override TensorType Remove(int position)
        {
            TensorType[] result = new TensorType[_children.Length - 1];
            int targetIndex = 0;
            for (int i = 0; i < _children.Length; i++)
            {
                if (i == position)
                {
                    continue;
                }

                result[targetIndex++] = _children[i];
            }

            return new TestMultiTensor(result);
        }

        protected override TensorType Remove1(int[] positions)
        {
            LastRemovedPositions = positions;
            return RemoveMarker;
        }

        protected override Complex GetNeutral()
        {
            return Complex.Zero;
        }

        protected override TensorType Select1(int[] positions)
        {
            LastSelectedPositions = positions;
            return SelectMarker;
        }

        public override string ToString(OutputFormat outputFormat)
        {
            return "multi";
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
