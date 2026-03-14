using NRedberry.IndexMapping;
using NRedberry.Numbers;
using NRedberry.Tensors;
using Xunit;
using TensorApi = NRedberry.Tensors.Tensors;
using TensorType = NRedberry.Tensors.Tensor;

namespace NRedberry.Core.Tests.Indexmapping;

public sealed class IndexMappingsTests
{
    [Fact]
    public void CreatePortShouldReturnIdentityMappingForEqualScalars()
    {
        MappingsPort port = IndexMappings.CreatePort(Complex.One, Complex.One);

        Mapping? first = port.Take();
        Assert.NotNull(first);
        Assert.False(first.GetSign());
        Assert.True(first.IsEmpty());
        Assert.Null(port.Take());
    }

    [Fact]
    public void SimpleTensorsPortShouldMapMatchingSimpleTensors()
    {
        SimpleTensor from = TensorApi.ParseSimple("A");
        SimpleTensor to = TensorApi.ParseSimple("A");

        Mapping? mapping = IndexMappings.SimpleTensorsPort(from, to).Take();
        Assert.NotNull(mapping);
        Assert.False(mapping.GetSign());
    }

    [Fact]
    public void CreateBijectiveProductPortShouldReturnMappingsForMatchingFactors()
    {
        TensorType[] from = [Complex.One, Complex.One];
        TensorType[] to = [Complex.One, Complex.One];

        Mapping? mapping = IndexMappings.CreateBijectiveProductPort(from, to).Take();
        Assert.NotNull(mapping);
    }

    [Fact]
    public void GetFirstShouldReturnNullWhenNoMappingExists()
    {
        Assert.Null(IndexMappings.GetFirst(Complex.One, Complex.Zero));
    }

    [Fact]
    public void MappingQueriesShouldReflectEqualAndUnequalCases()
    {
        Assert.True(IndexMappings.MappingExists(Complex.One, Complex.One));
        Assert.True(IndexMappings.PositiveMappingExists(Complex.One, Complex.One));
        Assert.False(IndexMappings.AnyMappingExists(Complex.One, Complex.Zero));
        Assert.False(IndexMappings.MappingExists(Complex.One, Complex.Zero));
    }

    [Fact]
    public void EqualityQueriesShouldReflectEqualAndUnequalCases()
    {
        Assert.True(IndexMappings.Equals(Complex.One, Complex.One));
        Assert.False(IndexMappings.Equals(Complex.One, Complex.Zero));
        Assert.False(IndexMappings.Compare1(Complex.One, Complex.One));
        Assert.Null(IndexMappings.Compare1(Complex.One, Complex.Zero));
    }

    [Fact]
    public void IsZeroDueToSymmetryShouldBeFalseForSimpleScalar()
    {
        Assert.False(IndexMappings.IsZeroDueToSymmetry(Complex.One));
    }

    [Fact]
    public void GetAllMappingsShouldReturnSingleIdentityMappingForEqualScalars()
    {
        ISet<Mapping> mappings = IndexMappings.GetAllMappings(Complex.One, Complex.One);

        Assert.Single(mappings);
        Mapping mapping = Assert.Single(mappings);
        Assert.False(mapping.GetSign());
        Assert.True(mapping.IsEmpty());
    }

    [Fact]
    public void TestMappingShouldValidateReturnedMapping()
    {
        Mapping mapping = Assert.IsType<Mapping>(IndexMappings.GetFirst(Complex.One, Complex.One));
        Assert.True(IndexMappings.TestMapping(mapping, Complex.One, Complex.One));
    }
}
