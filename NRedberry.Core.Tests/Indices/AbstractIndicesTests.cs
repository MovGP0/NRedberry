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
        ArgumentNullException exception = Should.Throw<ArgumentNullException>(() => new TestIndices(null!));
        exception.ParamName.ShouldBe("data");
    }

    [Fact]
    public void DataAllIndicesSizeAndIndexer_ShouldExposeUnderlyingValues()
    {
        int[] data = [0x01000001, unchecked((int)0x81000002), 0x02000003];
        TestIndices indices = new(data);

        indices.Data.ShouldBeSameAs(data);
        indices.Size().ShouldBe(data.Length);
        indices.AllIndices.ShouldBe(data);
        indices[1].ShouldBe(data[1]);
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

        indices.CalculateUpperLowerCallCount.ShouldBe(1);
        firstUpper.SequenceEqual([upper]).ShouldBeTrue();
        secondUpper.SequenceEqual([upper]).ShouldBeTrue();
        firstLower.SequenceEqual([lower]).ShouldBeTrue();
        secondLower.SequenceEqual([lower]).ShouldBeTrue();
        upperLower.Upper.SequenceEqual([upper]).ShouldBeTrue();
        upperLower.Lower.SequenceEqual([lower]).ShouldBeTrue();
    }

    [Fact]
    public void EqualsAndHashCode_ShouldDependOnDataSequence()
    {
        TestIndices left = new([1, 2, 3]);
        TestIndices equal = new([1, 2, 3]);
        TestIndices differentOrder = new([3, 2, 1]);

        left.Equals(equal).ShouldBeTrue();
        equal.GetHashCode().ShouldBe(left.GetHashCode());
        left.Equals(differentOrder).ShouldBeFalse();
    }

    [Fact]
    public void EqualsRegardlessOrder_ShouldHandleEmptyAndSortedComparison()
    {
        TestIndices empty = new([]);
        TestIndices first = new([3, 1, 2]);
        TestIndices second = new([2, 3, 1]);
        TestIndices different = new([2, 4, 1]);

        empty.EqualsRegardlessOrder(EmptyIndices.EmptyIndicesInstance).ShouldBeTrue();
        first.EqualsRegardlessOrder(second).ShouldBeTrue();
        first.EqualsRegardlessOrder(different).ShouldBeFalse();
    }

    [Fact]
    public void GetEnumerator_ShouldYieldDataInOrder()
    {
        TestIndices indices = new([7, 8, 9]);

        indices.ToArray().ShouldBe([7, 8, 9]);
    }

    [Fact]
    public void TypeIndexer_ShouldDelegateToSubclassImplementation()
    {
        int greekLowerA = (IndexType.GreekLower.GetType_() << 24) | 1;
        int latinLowerB = (IndexType.LatinLower.GetType_() << 24) | 2;
        int greekLowerC = (IndexType.GreekLower.GetType_() << 24) | 3;
        TestIndices indices = new([greekLowerA, latinLowerB, greekLowerC]);

        int result = indices[IndexType.GreekLower, 1];

        result.ShouldBe(greekLowerC);
        indices.GetByTypePositionCallCount.ShouldBe(1);
    }

    [Fact]
    public void Get_ShouldReturnElementAtPosition()
    {
        TestIndices indices = new([10, 11, 12]);

        indices.Get(1).ShouldBe(11);
        Should.Throw<IndexOutOfRangeException>(() => indices.Get(3));
    }

    private sealed class TestIndices(int[] data) : AbstractIndices(data)
    {
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
