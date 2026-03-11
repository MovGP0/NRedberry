using NRedberry.Indices;
using NRedberry.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class IndexMapperTests
{
    [Fact]
    public void ShouldThrowWhenFromIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new IndexMapper(null!, []));
    }

    [Fact]
    public void ShouldThrowWhenToIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new IndexMapper([], null!));
    }

    [Fact]
    public void ShouldReturnOriginalIndexWhenNoMappingExists()
    {
        IndexMapper mapper = new([], []);
        int index = Lower(3);

        Assert.Equal(index, mapper.Map(index));
    }

    [Fact]
    public void ShouldMapByNameAndPreserveVariance()
    {
        IndexMapper mapper = new([NameWithType(1)], [NameWithType(7)]);

        Assert.Equal(Lower(7), mapper.Map(Lower(1)));
        Assert.Equal(Upper(7), mapper.Map(Upper(1)));
    }

    [Fact]
    public void ShouldThrowWhenContractInputIsNull()
    {
        IndexMapper mapper = new([], []);

        Assert.Throws<ArgumentNullException>(() => mapper.Contract(null!));
    }

    [Fact]
    public void ShouldReturnFalseWhenContractHasLessThanTwoIndices()
    {
        IndexMapper mapper = new([], []);

        Assert.False(mapper.Contract([]));
        Assert.False(mapper.Contract([Lower(1)]));
    }

    [Fact]
    public void ShouldReturnFalseWhenMappedIndicesDoNotContract()
    {
        IndexMapper mapper = new([NameWithType(1)], [NameWithType(7)]);
        int[] freeIndices = [Lower(1), Lower(2)];

        bool contract = mapper.Contract(freeIndices);

        Assert.False(contract);
        Assert.Equal([NameWithType(2), NameWithType(7)], freeIndices);
    }

    [Fact]
    public void ShouldReturnTrueWhenMappedIndicesContract()
    {
        IndexMapper mapper = new([NameWithType(1), NameWithType(2)], [NameWithType(7), NameWithType(7)]);
        int[] freeIndices = [Lower(1), Upper(2)];

        bool contract = mapper.Contract(freeIndices);

        Assert.True(contract);
        Assert.Equal([NameWithType(7), NameWithType(7)], freeIndices);
    }

    private static int Lower(int name)
    {
        return IndicesUtils.CreateIndex(name, (byte)0, false);
    }

    private static int Upper(int name)
    {
        return IndicesUtils.CreateIndex(name, (byte)0, true);
    }

    private static int NameWithType(int name)
    {
        return IndicesUtils.GetNameWithType(Lower(name));
    }
}
