using NRedberry.IndexMapping;
using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Indexmapping;

public sealed class MappingTests
{
    [Fact]
    public void ConstructorShouldNormalizeIndicesAndSortEntries()
    {
        int[] from = [IndicesUtils.ParseIndex("^b"), IndicesUtils.ParseIndex("_a")];
        int[] to = [IndicesUtils.ParseIndex("_d"), IndicesUtils.ParseIndex("^c")];

        Mapping mapping = new(from, to);

        Assert.Equal(2, mapping.Size());
        Assert.Equal(
            [
                IndicesUtils.GetNameWithType(IndicesUtils.ParseIndex("_a")),
                IndicesUtils.GetNameWithType(IndicesUtils.ParseIndex("^b"))
            ],
            mapping.GetFromNames().ToArray());
        Assert.Equal(
            [
                IndicesUtils.ParseIndex("^c"),
                IndicesUtils.ParseIndex("^d")
            ],
            mapping.GetToData().ToArray());
    }

    [Fact]
    public void ConstructorShouldThrowWhenLengthsDiffer()
    {
        int[] from = [IndicesUtils.ParseIndex("_a")];
        int[] to = [IndicesUtils.ParseIndex("_b"), IndicesUtils.ParseIndex("_c")];

        ArgumentException exception = Assert.Throws<ArgumentException>(() => new Mapping(from, to));

        Assert.Equal("From length != to length.", exception.Message);
    }

    [Fact]
    public void IdentityMappingShouldExposeEmptyPositiveMapping()
    {
        Mapping mapping = Mapping.IdentityMapping;

        Assert.True(mapping.IsIdentity());
        Assert.True(mapping.IsEmpty());
        Assert.False(mapping.GetSign());
        Assert.Equal("{}", mapping.ToString());
    }

    [Fact]
    public void AddSignShouldToggleSignWithoutChangingEntries()
    {
        Mapping mapping = new(
            [IndicesUtils.ParseIndex("_a")],
            [IndicesUtils.ParseIndex("^b")]);

        Mapping result = mapping.AddSign(true);

        Assert.True(result.GetSign());
        Assert.Equal(mapping.GetFromNames().ToArray(), result.GetFromNames().ToArray());
        Assert.Equal(mapping.GetToData().ToArray(), result.GetToData().ToArray());
    }

    [Fact]
    public void TransformShouldApplyMappingAutomatically()
    {
        NRedberry.Tensors.Tensor tensor = TensorFactory.Parse("T_a");
        Mapping mapping = new(
            [IndicesUtils.ParseIndex("_a")],
            [IndicesUtils.ParseIndex("^b")]);

        NRedberry.Tensors.Tensor transformed = mapping.Transform(tensor);

        Assert.Equal("T{}^{b}", transformed.ToString(OutputFormat.LaTeX));
    }

    [Fact]
    public void EqualsAndHashCodeShouldDependOnEntriesAndSign()
    {
        Mapping first = new(
            [IndicesUtils.ParseIndex("_a")],
            [IndicesUtils.ParseIndex("^b")],
            sign: true);
        Mapping second = new(
            [IndicesUtils.ParseIndex("_a")],
            [IndicesUtils.ParseIndex("^b")],
            sign: true);
        Mapping third = new(
            [IndicesUtils.ParseIndex("_a")],
            [IndicesUtils.ParseIndex("^b")],
            sign: false);

        Assert.Equal(first, second);
        Assert.Equal(first.GetHashCode(), second.GetHashCode());
        Assert.NotEqual(first, third);
    }

    [Fact]
    public void ToStringShouldRenderSignedMapping()
    {
        Mapping mapping = new(
            [IndicesUtils.ParseIndex("_a"), IndicesUtils.ParseIndex("^b")],
            [IndicesUtils.ParseIndex("^c"), IndicesUtils.ParseIndex("_d")],
            sign: true);

        Assert.Equal("-{_a->^c, _b->^d}", mapping.ToString());
    }

    [Fact]
    public void ValueOfShouldParseSignedMapping()
    {
        Mapping mapping = Mapping.ValueOf("-{_a->^c, ^b->_d}");

        Assert.True(mapping.GetSign());
        Assert.Equal(
            [
                IndicesUtils.GetNameWithType(IndicesUtils.ParseIndex("_a")),
                IndicesUtils.GetNameWithType(IndicesUtils.ParseIndex("^b"))
            ],
            mapping.GetFromNames().ToArray());
        Assert.Equal(
            [
                IndicesUtils.ParseIndex("^c"),
                IndicesUtils.ParseIndex("^d")
            ],
            mapping.GetToData().ToArray());
    }

    [Fact]
    public void ValueOfShouldParseEmptyMappings()
    {
        Mapping mapping = Mapping.ValueOf("+{}");

        Assert.True(mapping.IsIdentity());
    }

    [Fact]
    public void ValueOfShouldThrowOnInvalidSyntax()
    {
        Assert.Throws<ArgumentException>(() => Mapping.ValueOf("[]"));
    }
}
