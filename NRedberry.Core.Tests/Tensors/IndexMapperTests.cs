using NRedberry.Indices;
using NRedberry.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class IndexMapperTests
{
    [Fact]
    public void ShouldThrowWhenFromIsNull()
    {
        Should.Throw<ArgumentNullException>(() => new IndexMapper(null!, []));
    }

    [Fact]
    public void ShouldThrowWhenToIsNull()
    {
        Should.Throw<ArgumentNullException>(() => new IndexMapper([], null!));
    }

    [Fact]
    public void ShouldReturnOriginalIndexWhenNoMappingExists()
    {
        IndexMapper mapper = new([], []);
        int index = Lower(3);

        mapper.Map(index).ShouldBe(index);
    }

    [Fact]
    public void ShouldMapByNameAndPreserveVariance()
    {
        IndexMapper mapper = new([NameWithType(1)], [NameWithType(7)]);

        mapper.Map(Lower(1)).ShouldBe(Lower(7));
        mapper.Map(Upper(1)).ShouldBe(Upper(7));
    }

    [Fact]
    public void ShouldThrowWhenContractInputIsNull()
    {
        IndexMapper mapper = new([], []);

        Should.Throw<ArgumentNullException>(() => mapper.Contract(null!));
    }

    [Fact]
    public void ShouldReturnFalseWhenContractHasLessThanTwoIndices()
    {
        IndexMapper mapper = new([], []);

        mapper.Contract([]).ShouldBeFalse();
        mapper.Contract([Lower(1)]).ShouldBeFalse();
    }

    [Fact]
    public void ShouldReturnFalseWhenMappedIndicesDoNotContract()
    {
        IndexMapper mapper = new([NameWithType(1)], [NameWithType(7)]);
        int[] freeIndices = [Lower(1), Lower(2)];

        bool contract = mapper.Contract(freeIndices);

        contract.ShouldBeFalse();
        freeIndices.ShouldBe([NameWithType(2), NameWithType(7)]);
    }

    [Fact]
    public void ShouldReturnTrueWhenMappedIndicesContract()
    {
        IndexMapper mapper = new([NameWithType(1), NameWithType(2)], [NameWithType(7), NameWithType(7)]);
        int[] freeIndices = [Lower(1), Upper(2)];

        bool contract = mapper.Contract(freeIndices);

        contract.ShouldBeTrue();
        freeIndices.ShouldBe([NameWithType(7), NameWithType(7)]);
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
