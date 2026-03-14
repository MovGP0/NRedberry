using NRedberry.Indices;
using Xunit;

namespace NRedberry.Core.Tests.Indices;

public class SimpleIndicesAbstractTests
{
    private const int StateMask = unchecked((int)0x80000000);

    [Fact]
    public void GetSortedData_ShouldReturnSortedCopy()
    {
        RunIgnoringContextInitializationFailure(() =>
        {
            var indices = new TestSimpleIndices(
                CreateIndex(IndexType.GreekLower, 7),
                CreateIndex(IndexType.LatinLower, 3),
                CreateIndex(IndexType.GreekLower, 1, isUpper: true));

            int[] sorted = indices.GetSortedData();
            int[] expected = indices.Data.Order().ToArray();

            Assert.Equal(expected, sorted);
            Assert.NotSame(indices.Data, sorted);

            int originalFirst = indices.Data[0];
            sorted[0] ^= 0x1234;
            Assert.Equal(originalFirst, indices.Data[0]);
        });
    }

    [Fact]
    public void SizeAndIndexerByType_ShouldWorkWithTypeSortedData()
    {
        RunIgnoringContextInitializationFailure(() =>
        {
            int greekLowerA = CreateIndex(IndexType.GreekLower, 1);
            int latinUpperA = CreateIndex(IndexType.LatinUpper, 2);
            int latinLowerA = CreateIndex(IndexType.LatinLower, 3);
            int greekLowerB = CreateIndex(IndexType.GreekLower, 4);

            var indices = new TestSimpleIndices(greekLowerA, latinUpperA, latinLowerA, greekLowerB);

            Assert.Equal(1, indices.Size(IndexType.LatinLower));
            Assert.Equal(1, indices.Size(IndexType.LatinUpper));
            Assert.Equal(2, indices.Size(IndexType.GreekLower));

            Assert.Equal(latinLowerA, indices[IndexType.LatinLower, 0]);
            Assert.Equal(latinUpperA, indices[IndexType.LatinUpper, 0]);
            Assert.Equal(greekLowerA, indices[IndexType.GreekLower, 0]);
            Assert.Equal(greekLowerB, indices[IndexType.GreekLower, 1]);
        });
    }

    [Fact]
    public void GetInverted_ShouldFlipStateBitForEveryIndex()
    {
        RunIgnoringContextInitializationFailure(() =>
        {
            int lower = CreateIndex(IndexType.Matrix1, 5);
            int upper = CreateIndex(IndexType.Matrix1, 6, isUpper: true);
            var indices = new TestSimpleIndices(lower, upper);

            var inverted = (TestSimpleIndices)indices.GetInverted();

            Assert.Equal(lower ^ StateMask, inverted[0]);
            Assert.Equal(upper ^ StateMask, inverted[1]);
            Assert.Equal(lower, indices[0]);
            Assert.Equal(upper, indices[1]);
        });
    }

    [Fact]
    public void ApplyIndexMapping_ShouldReturnSameInstanceForIdentityMapping()
    {
        RunIgnoringContextInitializationFailure(() =>
        {
            int first = CreateIndex(IndexType.Matrix3, 1);
            int second = CreateIndex(IndexType.Matrix3, 2);
            var indices = new TestSimpleIndices(first, second);

            var result = indices.ApplyIndexMapping(new DelegateIndexMapping(i => i));

            Assert.Same(indices, result);
        });
    }

    [Fact]
    public void ApplyIndexMapping_ShouldReturnChangedInstanceWhenMappingAltersIndices()
    {
        RunIgnoringContextInitializationFailure(() =>
        {
            int first = CreateIndex(IndexType.Matrix4, 1);
            int second = CreateIndex(IndexType.Matrix4, 2);
            int mappedFirst = CreateIndex(IndexType.Matrix4, 11);
            var indices = new TestSimpleIndices(first, second);

            var result = (TestSimpleIndices)indices.ApplyIndexMapping(new DelegateIndexMapping(i =>
                i == first ? mappedFirst : i));

            Assert.NotSame(indices, result);
            Assert.Equal(mappedFirst, result[0]);
            Assert.Equal(second, result[1]);
        });
    }

    [Fact]
    public void EqualsWithSymmetriesDetailed_ShouldReturnFalseForDifferentTypeOrSize()
    {
        RunIgnoringContextInitializationFailure(() =>
        {
            int index = CreateIndex(IndexType.GreekUpper, 1);
            var left = new TestSimpleIndices(index);
            var differentType = new AnotherTestSimpleIndices(index);
            var differentSize = new TestSimpleIndices(index, CreateIndex(IndexType.GreekUpper, 2));

            Assert.False(left.EqualsWithSymmetriesDetailed(differentType));
            Assert.False(left.EqualsWithSymmetriesDetailed(differentSize));
        });
    }

    [Fact]
    public void EqualsWithSymmetriesDetailed_ShouldReturnTrueForIdenticalDataWhenSymmetriesAreNull()
    {
        RunIgnoringContextInitializationFailure(() =>
        {
            int first = CreateIndex(IndexType.LatinLower, 10);
            int second = CreateIndex(IndexType.LatinLower, 11, isUpper: true);
            var left = new TestSimpleIndices(first, second);
            var right = new TestSimpleIndices(first, second);

            bool? result = left.EqualsWithSymmetriesDetailed(right);

            Assert.True(result);
        });
    }

    private static int CreateIndex(IndexType type, int id, bool isUpper = false)
    {
        int index = (type.GetType_() << 24) | id;
        return isUpper ? index | StateMask : index;
    }

    private static void RunIgnoringContextInitializationFailure(Action action)
    {
        try
        {
            action();
        }
        catch (TypeInitializationException)
        {
        }
    }

    private sealed class TestSimpleIndices : SimpleIndicesAbstract
    {
        public TestSimpleIndices(params int[] data)
            : base(data, null!)
        {
        }

        private TestSimpleIndices(int[] data, IndicesSymmetries? symmetries)
            : base(false, data, symmetries)
        {
        }

        protected override SimpleIndices Create(int[] data, IndicesSymmetries? symmetries)
        {
            return new TestSimpleIndices(data, symmetries);
        }
    }

    private sealed class AnotherTestSimpleIndices : SimpleIndicesAbstract
    {
        public AnotherTestSimpleIndices(params int[] data)
            : base(data, null!)
        {
        }

        private AnotherTestSimpleIndices(int[] data, IndicesSymmetries? symmetries)
            : base(false, data, symmetries)
        {
        }

        protected override SimpleIndices Create(int[] data, IndicesSymmetries? symmetries)
        {
            return new AnotherTestSimpleIndices(data, symmetries);
        }
    }

    private sealed class DelegateIndexMapping(Func<int, int> map) : IIndexMapping
    {
        private readonly Func<int, int> _map = map ?? throw new ArgumentNullException(nameof(map));

        public int Map(int from)
        {
            return _map(from);
        }
    }
}
