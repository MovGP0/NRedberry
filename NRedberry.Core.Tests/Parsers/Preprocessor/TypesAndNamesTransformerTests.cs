using NRedberry;
using NRedberry.Contexts;
using NRedberry.Indices;
using NRedberry.Parsers.Preprocessor;
using Xunit;

namespace NRedberry.Core.Tests.Parsers.Preprocessor;

public sealed class TypesAndNamesTransformerTests
{
    [Fact]
    public void ShouldSetIndicesUsingSortedMappingAndKeepOtherValues()
    {
        var transformer = TypesAndNamesTransformer.Utils.SetIndices([30, 10, 20], [300, 100, 200]);
        var descriptor = CreateDescriptor("F");

        Assert.Equal(100, transformer.NewIndex(10, descriptor));
        Assert.Equal(200, transformer.NewIndex(20, descriptor));
        Assert.Equal(300, transformer.NewIndex(30, descriptor));
        Assert.Equal(99, transformer.NewIndex(99, descriptor));

        Assert.Equal(IndexType.GreekUpper, transformer.NewType(IndexType.GreekUpper, descriptor));
        Assert.Equal("Name", transformer.NewName("Name", descriptor));
    }

    [Fact]
    public void ShouldChangeOnlyRequestedType()
    {
        var transformer = TypesAndNamesTransformer.Utils.ChangeType(IndexType.LatinLower, IndexType.LatinUpper);
        var descriptor = CreateDescriptor("F");

        Assert.Equal(IndexType.LatinUpper, transformer.NewType(IndexType.LatinLower, descriptor));
        Assert.Equal(IndexType.GreekLower, transformer.NewType(IndexType.GreekLower, descriptor));

        Assert.Equal(17, transformer.NewIndex(17, descriptor));
        Assert.Equal("f", transformer.NewName("f", descriptor));
    }

    [Fact]
    public void ShouldChangeNameByDescriptorName()
    {
        var transformer = TypesAndNamesTransformer.Utils.ChangeName(["k", "f"], ["K", "F"]);

        var mappedDescriptor = CreateDescriptor("f");
        var unmappedDescriptor = CreateDescriptor("x");

        Assert.Equal("F", transformer.NewName("ignored", mappedDescriptor));
        Assert.Equal("x", transformer.NewName("ignored", unmappedDescriptor));

        Assert.Equal(IndexType.Matrix1, transformer.NewType(IndexType.Matrix1, mappedDescriptor));
        Assert.Equal(7, transformer.NewIndex(7, mappedDescriptor));
    }

    [Fact]
    public void ShouldApplyTransformersInOrderWhenComposed()
    {
        var descriptor = CreateDescriptor("f");

        var first = new DelegateTransformer(
            (_, _) => IndexType.LatinLower,
            (index, _) => index + 1,
            (name, _) => name + "1");
        var second = new DelegateTransformer(
            (_, _) => IndexType.GreekUpper,
            (index, _) => index * 2,
            (name, _) => name + "2");

        var combined = TypesAndNamesTransformer.Utils.And(first, second);

        Assert.Equal(IndexType.GreekUpper, combined.NewType(IndexType.Matrix4, descriptor));
        Assert.Equal(8, combined.NewIndex(3, descriptor));
        Assert.Equal("x12", combined.NewName("x", descriptor));
    }

    [Fact]
    public void ShouldKeepInputWhenComposedWithNoTransformers()
    {
        var descriptor = CreateDescriptor("f");
        var combined = TypesAndNamesTransformer.Utils.And();

        Assert.Equal(IndexType.Matrix2, combined.NewType(IndexType.Matrix2, descriptor));
        Assert.Equal(42, combined.NewIndex(42, descriptor));
        Assert.Equal("abc", combined.NewName("abc", descriptor));
    }

    [Fact]
    public void ShouldThrowWhenSetIndicesArgumentsAreNull()
    {
        var fromException = Assert.Throws<ArgumentNullException>(() => TypesAndNamesTransformer.Utils.SetIndices(null!, [1]));
        var toException = Assert.Throws<ArgumentNullException>(() => TypesAndNamesTransformer.Utils.SetIndices([1], null!));

        Assert.Equal("from", fromException.ParamName);
        Assert.Equal("to", toException.ParamName);
    }

    [Fact]
    public void ShouldThrowWhenChangeNameArgumentsAreNull()
    {
        var fromException = Assert.Throws<ArgumentNullException>(() => TypesAndNamesTransformer.Utils.ChangeName(null!, ["a"]));
        var toException = Assert.Throws<ArgumentNullException>(() => TypesAndNamesTransformer.Utils.ChangeName(["a"], null!));

        Assert.Equal("from", fromException.ParamName);
        Assert.Equal("to", toException.ParamName);
    }

    [Fact]
    public void ShouldThrowWhenComposedTransformersArrayIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => TypesAndNamesTransformer.Utils.And(null!));

        Assert.Equal("transformers", exception.ParamName);
    }

    private static NameAndStructureOfIndices CreateDescriptor(string name)
    {
        return new NameAndStructureOfIndices(name, Array.Empty<StructureOfIndices>());
    }

    private sealed class DelegateTransformer(
        Func<IndexType, NameAndStructureOfIndices, IndexType> typeTransformer,
        Func<int, NameAndStructureOfIndices, int> indexTransformer,
        Func<string, NameAndStructureOfIndices, string> nameTransformer) : TypesAndNamesTransformer
    {
        public IndexType NewType(IndexType oldType, NameAndStructureOfIndices descriptor)
        {
            return typeTransformer(oldType, descriptor);
        }

        public int NewIndex(int oldIndex, NameAndStructureOfIndices descriptor)
        {
            return indexTransformer(oldIndex, descriptor);
        }

        public string NewName(string oldName, NameAndStructureOfIndices descriptor)
        {
            return nameTransformer(oldName, descriptor);
        }
    }
}
