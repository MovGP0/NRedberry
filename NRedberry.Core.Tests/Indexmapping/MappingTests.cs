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

        mapping.Size().ShouldBe(2);
        mapping.GetFromNames().ToArray().ShouldBe([
                IndicesUtils.GetNameWithType(IndicesUtils.ParseIndex("_a")),
                IndicesUtils.GetNameWithType(IndicesUtils.ParseIndex("^b"))
            ]);
        mapping.GetToData().ToArray().ShouldBe([
                IndicesUtils.ParseIndex("^c"),
                IndicesUtils.ParseIndex("^d")
            ]);
    }

    [Fact]
    public void ConstructorShouldThrowWhenLengthsDiffer()
    {
        int[] from = [IndicesUtils.ParseIndex("_a")];
        int[] to = [IndicesUtils.ParseIndex("_b"), IndicesUtils.ParseIndex("_c")];

        ArgumentException exception = Should.Throw<ArgumentException>(() => new Mapping(from, to));

        exception.Message.ShouldBe("From length != to length.");
    }

    [Fact]
    public void IdentityMappingShouldExposeEmptyPositiveMapping()
    {
        Mapping mapping = Mapping.IdentityMapping;

        mapping.IsIdentity().ShouldBeTrue();
        mapping.IsEmpty().ShouldBeTrue();
        mapping.GetSign().ShouldBeFalse();
        mapping.ToString().ShouldBe("{}");
    }

    [Fact]
    public void AddSignShouldToggleSignWithoutChangingEntries()
    {
        Mapping mapping = new(
            [IndicesUtils.ParseIndex("_a")],
            [IndicesUtils.ParseIndex("^b")]);

        Mapping result = mapping.AddSign(true);

        result.GetSign().ShouldBeTrue();
        result.GetFromNames().ToArray().ShouldBe(mapping.GetFromNames().ToArray());
        result.GetToData().ToArray().ShouldBe(mapping.GetToData().ToArray());
    }

    [Fact]
    public void TransformShouldApplyMappingAutomatically()
    {
        NRedberry.Tensors.Tensor tensor = TensorFactory.Parse("T_a");
        Mapping mapping = new(
            [IndicesUtils.ParseIndex("_a")],
            [IndicesUtils.ParseIndex("^b")]);

        NRedberry.Tensors.Tensor transformed = mapping.Transform(tensor);

        transformed.ToString(OutputFormat.LaTeX).ShouldBe("T{}^{b}");
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

        second.ShouldBe(first);
        second.GetHashCode().ShouldBe(first.GetHashCode());
        third.ShouldNotBe(first);
    }

    [Fact]
    public void ToStringShouldRenderSignedMapping()
    {
        Mapping mapping = new(
            [IndicesUtils.ParseIndex("_a"), IndicesUtils.ParseIndex("^b")],
            [IndicesUtils.ParseIndex("^c"), IndicesUtils.ParseIndex("_d")],
            sign: true);

        mapping.ToString().ShouldBe("-{_a->^c, _b->^d}");
    }

    [Fact]
    public void ValueOfShouldParseSignedMapping()
    {
        Mapping mapping = Mapping.ValueOf("-{_a->^c, ^b->_d}");

        mapping.GetSign().ShouldBeTrue();
        mapping.GetFromNames().ToArray().ShouldBe([
                IndicesUtils.GetNameWithType(IndicesUtils.ParseIndex("_a")),
                IndicesUtils.GetNameWithType(IndicesUtils.ParseIndex("^b"))
            ]);
        mapping.GetToData().ToArray().ShouldBe([
                IndicesUtils.ParseIndex("^c"),
                IndicesUtils.ParseIndex("^d")
            ]);
    }

    [Fact]
    public void ValueOfShouldParseEmptyMappings()
    {
        Mapping mapping = Mapping.ValueOf("+{}");

        mapping.IsIdentity().ShouldBeTrue();
    }

    [Fact]
    public void ValueOfShouldThrowOnInvalidSyntax()
    {
        Should.Throw<ArgumentException>(() => Mapping.ValueOf("[]"));
    }
}
