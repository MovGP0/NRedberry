using NRedberry.IndexMapping;

namespace NRedberry.Core.Tests.Indexmapping;

public sealed class IndexMappingTestUtilsTests
{
    [Fact]
    public void ShouldParseSemicolonSeparatedMappings()
    {
        Mapping mapping = IndexMappingTestUtils.Parse("-;_a->^c;^b->_d");
        Mapping expected = Mapping.ValueOf("-{_a->^c, ^b->_d}");

        mapping.ShouldBe(expected);
    }

    [Fact]
    public void ShouldCompareMappingsRegardlessOfInputOrder()
    {
        List<Mapping> first =
        [
            IndexMappingTestUtils.Parse("+;_a->^b"),
            IndexMappingTestUtils.Parse("-;_c->^d"),
        ];
        List<Mapping> second =
        [
            IndexMappingTestUtils.Parse("-;_c->^d"),
            IndexMappingTestUtils.Parse("+;_a->^b"),
        ];

        IndexMappingTestUtils.Compare(first, second).ShouldBeTrue();
    }

    [Fact]
    public void ShouldExposeStableComparator()
    {
        Mapping first = IndexMappingTestUtils.Parse("+;_a->^b");
        Mapping second = IndexMappingTestUtils.Parse("-;_a->^b");
        IComparer<Mapping> comparator = IndexMappingTestUtils.GetComparator();

        int comparison = comparator.Compare(first, second);

        comparison.ShouldBe(first.GetHashCode().CompareTo(second.GetHashCode()));
    }
}
