using System.Collections.Immutable;
using NRedberry.Indices;
using Xunit;
using TensorIndices = NRedberry.Indices.Indices;

namespace NRedberry.Core.Tests.Indices;

public class AbstractIndicesTests
{
    [Fact]
    public void Constructor_ShouldThrowWhenDataIsNull()
    {
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new TestIndices(null!));
        Assert.Equal("data", exception.ParamName);
    }

    [Fact]
    public void DataAllIndicesSizeAndIndexer_ShouldExposeUnderlyingValues()
    {
        int[] data = [0x01000001, unchecked((int)0x81000002), 0x02000003];
        TestIndices indices = new(data);

        Assert.Same(data, indices.Data);
        Assert.Equal(data.Length, indices.Size());
        Assert.Equal(data, indices.AllIndices);
        Assert.Equal(data[1], indices[1]);
    }

    [Fact]
    public void UpperAndLowerIndices_ShouldBeCalculatedOnceAndThenCached()
    {
        const int upper = unchecked((int)0x81000005);
        const int lower = 0x01000006;
        TestIndices indices = new([upper, lower]);

        ImmutableArray<int> firstUpper = indices.UpperIndices;
        ImmutableArray<int> secondUpper = indices.UpperIndices;
        ImmutableArray<int> firstLower = indices.LowerIndices;
        ImmutableArray<int> secondLower = indices.LowerIndices;
        UpperLowerIndices upperLower = indices.GetCachedUpperLowerForTest();

        Assert.Equal(1, indices.CalculateUpperLowerCallCount);
        Assert.True(firstUpper.SequenceEqual([upper]));
        Assert.True(secondUpper.SequenceEqual([upper]));
        Assert.True(firstLower.SequenceEqual([lower]));
        Assert.True(secondLower.SequenceEqual([lower]));
        Assert.True(upperLower.Upper.SequenceEqual([upper]));
        Assert.True(upperLower.Lower.SequenceEqual([lower]));
    }

    [Fact]
    public void EqualsAndHashCode_ShouldDependOnDataSequence()
    {
        TestIndices left = new([1, 2, 3]);
        TestIndices equal = new([1, 2, 3]);
        TestIndices differentOrder = new([3, 2, 1]);

        Assert.True(left.Equals(equal));
        Assert.Equal(left.GetHashCode(), equal.GetHashCode());
        Assert.False(left.Equals(differentOrder));
    }

    [Fact]
    public void EqualsRegardlessOrder_ShouldHandleEmptyAndSortedComparison()
    {
        TestIndices empty = new([]);
        TestIndices first = new([3, 1, 2]);
        TestIndices second = new([2, 3, 1]);
        TestIndices different = new([2, 4, 1]);

        Assert.True(empty.EqualsRegardlessOrder(EmptyIndices.EmptyIndicesInstance));
        Assert.True(first.EqualsRegardlessOrder(second));
        Assert.False(first.EqualsRegardlessOrder(different));
    }

    [Fact]
    public void GetEnumerator_ShouldYieldDataInOrder()
    {
        TestIndices indices = new([7, 8, 9]);

        Assert.Equal([7, 8, 9], indices.ToArray());
    }

    [Fact]
    public void TypeIndexer_ShouldDelegateToSubclassImplementation()
    {
        int greekLowerA = (IndexType.GreekLower.GetType_() << 24) | 1;
        int latinLowerB = (IndexType.LatinLower.GetType_() << 24) | 2;
        int greekLowerC = (IndexType.GreekLower.GetType_() << 24) | 3;
        TestIndices indices = new([greekLowerA, latinLowerB, greekLowerC]);

        int result = indices[IndexType.GreekLower, 1];

        Assert.Equal(greekLowerC, result);
        Assert.Equal(1, indices.GetByTypePositionCallCount);
    }

    [Fact]
    public void Get_ShouldReturnElementAtPosition()
    {
        TestIndices indices = new([10, 11, 12]);

        Assert.Equal(11, indices.Get(1));
        Assert.Throws<IndexOutOfRangeException>(() => indices.Get(3));
    }

    private sealed class TestIndices : AbstractIndices
    {
        public TestIndices(int[] data)
            : base(data)
        {
        }

        public int CalculateUpperLowerCallCount { get; private set; }
        public int GetByTypePositionCallCount { get; private set; }

        protected override UpperLowerIndices CalculateUpperLower()
        {
            CalculateUpperLowerCallCount++;

            List<int> upper = [];
            List<int> lower = [];
            for (int i = 0; i < Data.Length; i++)
            {
                if ((Data[i] & unchecked((int)0x80000000)) != 0)
                {
                    upper.Add(Data[i]);
                }
                else
                {
                    lower.Add(Data[i]);
                }
            }

            return new UpperLowerIndices([.. upper], [.. lower]);
        }

        public override int[] GetSortedData()
        {
            int[] sorted = (int[])Data.Clone();
            Array.Sort(sorted);
            return sorted;
        }

        public override TensorIndices GetOfType(IndexType type)
        {
            int typeMask = type.GetType_() << 24;
            int[] filtered = Data.Where(i => (i & 0x7F000000) == typeMask).ToArray();
            return new TestIndices(filtered);
        }

        public override void TestConsistentWithException()
        {
        }

        public override TensorIndices ApplyIndexMapping(IIndexMapping mapping)
        {
            return this;
        }

        public override int Size(IndexType type)
        {
            int typeMask = type.GetType_() << 24;
            return Data.Count(i => (i & 0x7F000000) == typeMask);
        }

        public override int this[IndexType type, int position] => GetByTypePosition(type, position);

        public override TensorIndices GetFree()
        {
            return this;
        }

        public override TensorIndices GetInverted()
        {
            return this;
        }

        public override short[] GetDiffIds()
        {
            return new short[Data.Length];
        }

        public UpperLowerIndices GetCachedUpperLowerForTest()
        {
            return UpperLowerIndices;
        }

        private int GetByTypePosition(IndexType type, int position)
        {
            GetByTypePositionCallCount++;

            int typeMask = type.GetType_() << 24;
            int[] ofType = Data.Where(i => (i & 0x7F000000) == typeMask).ToArray();
            return ofType[position];
        }
    }
}
