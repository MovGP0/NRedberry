using NRedberry.IndexMapping;
using NRedberry.Numbers;
using NRedberry.Tensors;
using Shouldly;
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
        first.ShouldNotBeNull();
        first.GetSign().ShouldBeFalse();
        first.IsEmpty().ShouldBeTrue();
        port.Take().ShouldBeNull();
    }

    [Fact]
    public void SimpleTensorsPortShouldMapMatchingSimpleTensors()
    {
        SimpleTensor from = TensorApi.ParseSimple("A");
        SimpleTensor to = TensorApi.ParseSimple("A");

        Mapping? mapping = IndexMappings.SimpleTensorsPort(from, to).Take();
        mapping.ShouldNotBeNull();
        mapping.GetSign().ShouldBeFalse();
    }

    [Fact]
    public void CreateBijectiveProductPortShouldReturnMappingsForMatchingFactors()
    {
        TensorType[] from = [Complex.One, Complex.One];
        TensorType[] to = [Complex.One, Complex.One];

        Mapping? mapping = IndexMappings.CreateBijectiveProductPort(from, to).Take();
        mapping.ShouldNotBeNull();
    }

    [Fact]
    public void GetFirstShouldReturnNullWhenNoMappingExists()
    {
        IndexMappings.GetFirst(Complex.One, Complex.Zero).ShouldBeNull();
    }

    [Fact]
    public void MappingQueriesShouldReflectEqualAndUnequalCases()
    {
        IndexMappings.MappingExists(Complex.One, Complex.One).ShouldBeTrue();
        IndexMappings.PositiveMappingExists(Complex.One, Complex.One).ShouldBeTrue();
        IndexMappings.AnyMappingExists(Complex.One, Complex.Zero).ShouldBeFalse();
        IndexMappings.MappingExists(Complex.One, Complex.Zero).ShouldBeFalse();
    }

    [Fact]
    public void EqualityQueriesShouldReflectEqualAndUnequalCases()
    {
        IndexMappings.Equals(Complex.One, Complex.One).ShouldBeTrue();
        IndexMappings.Equals(Complex.One, Complex.Zero).ShouldBeFalse();
        IndexMappings.Compare1(Complex.One, Complex.One).ShouldBe(false);
        IndexMappings.Compare1(Complex.One, Complex.Zero).ShouldBeNull();
    }

    [Fact]
    public void IsZeroDueToSymmetryShouldBeFalseForSimpleScalar()
    {
        IndexMappings.IsZeroDueToSymmetry(Complex.One).ShouldBeFalse();
    }

    [Fact]
    public void GetAllMappingsShouldReturnSingleIdentityMappingForEqualScalars()
    {
        ISet<Mapping> mappings = IndexMappings.GetAllMappings(Complex.One, Complex.One);

        mappings.Count.ShouldBe(1);
        Mapping mapping = mappings.Single();
        mapping.GetSign().ShouldBeFalse();
        mapping.IsEmpty().ShouldBeTrue();
    }

    [Fact]
    public void TestMappingShouldValidateReturnedMapping()
    {
        Mapping mapping = IndexMappings.GetFirst(Complex.One, Complex.One).ShouldBeOfType<Mapping>();
        IndexMappings.TestMapping(mapping, Complex.One, Complex.One).ShouldBeTrue();
    }
}
