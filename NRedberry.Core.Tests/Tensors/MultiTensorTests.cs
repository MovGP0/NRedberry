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

        result.ShouldBeOfType<TestMultiTensor>();
        ((TestMultiTensor)result).Size.ShouldBe(2);
        result[0].ShouldBeSameAs(Complex.Two);
        result[1].ShouldBeSameAs(child);
    }

    [Fact]
    public void ShouldReturnProvidedTensorWhenTensorIsNotPresent()
    {
        TestMultiTensor tensor = new([Complex.One, Complex.Two]);
        TensorType missing = Complex.Four;

        TensorType result = tensor.Remove(missing);

        result.ShouldBeSameAs(missing);
    }

    [Fact]
    public void ShouldReturnSameInstanceWhenRemovePositionsIsEmpty()
    {
        TestMultiTensor tensor = new([Complex.One, Complex.Two]);

        TensorType result = tensor.Remove([]);

        result.ShouldBeSameAs(tensor);
    }

    [Fact]
    public void ShouldThrowWhenRemovePositionIsOutOfRange()
    {
        TestMultiTensor tensor = new([Complex.One, Complex.Two]);

        Should.Throw<IndexOutOfRangeException>(() => tensor.Remove([2]));
        Should.Throw<IndexOutOfRangeException>(() => tensor.Remove([-1]));
    }

    [Fact]
    public void ShouldReturnNeutralWhenRemovingAllPositionsAfterNormalization()
    {
        TestMultiTensor tensor = new([Complex.One, Complex.Two, Complex.Four]);

        TensorType result = tensor.Remove([2, 0, 1, 1]);

        result.ShouldBeSameAs(Complex.Zero);
    }

    [Fact]
    public void ShouldPassSortedDistinctPositionsToRemoveCore()
    {
        TestMultiTensor tensor = new([Complex.One, Complex.Two, Complex.Four, Complex.ImaginaryOne]);

        TensorType result = tensor.Remove([3, 1, 3]);

        result.ShouldBeSameAs(tensor.RemoveMarker);
        tensor.LastRemovedPositions.ShouldNotBeNull();
        tensor.LastRemovedPositions.ShouldBe([1, 3]);
    }

    [Fact]
    public void ShouldReturnNeutralWhenSelectPositionsIsEmpty()
    {
        TestMultiTensor tensor = new([Complex.One, Complex.Two]);

        TensorType result = tensor.Select([]);

        result.ShouldBeSameAs(Complex.Zero);
    }

    [Fact]
    public void ShouldReturnSelectedChildWhenSinglePositionIsProvided()
    {
        TestMultiTensor tensor = new([Complex.One, Complex.Two, Complex.Four]);

        TensorType result = tensor.Select([1]);

        result.ShouldBeSameAs(Complex.Two);
    }

    [Fact]
    public void ShouldReturnSameInstanceWhenSelectingAllPositionsAfterNormalization()
    {
        TestMultiTensor tensor = new([Complex.One, Complex.Two, Complex.Four]);

        TensorType result = tensor.Select([2, 0, 1, 1]);

        result.ShouldBeSameAs(tensor);
    }

    [Fact]
    public void ShouldPassSortedDistinctPositionsToSelectCore()
    {
        TestMultiTensor tensor = new([Complex.One, Complex.Two, Complex.Four, Complex.ImaginaryOne]);

        TensorType result = tensor.Select([3, 1, 3]);

        result.ShouldBeSameAs(tensor.SelectMarker);
        tensor.LastSelectedPositions.ShouldNotBeNull();
        tensor.LastSelectedPositions.ShouldBe([1, 3]);
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
