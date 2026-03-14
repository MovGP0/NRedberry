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

        transformer.NewIndex(10, descriptor).ShouldBe(100);
        transformer.NewIndex(20, descriptor).ShouldBe(200);
        transformer.NewIndex(30, descriptor).ShouldBe(300);
        transformer.NewIndex(99, descriptor).ShouldBe(99);

        transformer.NewType(IndexType.GreekUpper, descriptor).ShouldBe(IndexType.GreekUpper);
        transformer.NewName("Name", descriptor).ShouldBe("Name");
    }

    [Fact]
    public void ShouldChangeOnlyRequestedType()
    {
        var transformer = TypesAndNamesTransformer.Utils.ChangeType(IndexType.LatinLower, IndexType.LatinUpper);
        var descriptor = CreateDescriptor("F");

        transformer.NewType(IndexType.LatinLower, descriptor).ShouldBe(IndexType.LatinUpper);
        transformer.NewType(IndexType.GreekLower, descriptor).ShouldBe(IndexType.GreekLower);

        transformer.NewIndex(17, descriptor).ShouldBe(17);
        transformer.NewName("f", descriptor).ShouldBe("f");
    }

    [Fact]
    public void ShouldChangeNameByDescriptorName()
    {
        var transformer = TypesAndNamesTransformer.Utils.ChangeName(["k", "f"], ["K", "F"]);

        var mappedDescriptor = CreateDescriptor("f");
        var unmappedDescriptor = CreateDescriptor("x");

        transformer.NewName("ignored", mappedDescriptor).ShouldBe("F");
        transformer.NewName("ignored", unmappedDescriptor).ShouldBe("x");

        transformer.NewType(IndexType.Matrix1, mappedDescriptor).ShouldBe(IndexType.Matrix1);
        transformer.NewIndex(7, mappedDescriptor).ShouldBe(7);
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

        combined.NewType(IndexType.Matrix4, descriptor).ShouldBe(IndexType.GreekUpper);
        combined.NewIndex(3, descriptor).ShouldBe(8);
        combined.NewName("x", descriptor).ShouldBe("x12");
    }

    [Fact]
    public void ShouldKeepInputWhenComposedWithNoTransformers()
    {
        var descriptor = CreateDescriptor("f");
        var combined = TypesAndNamesTransformer.Utils.And();

        combined.NewType(IndexType.Matrix2, descriptor).ShouldBe(IndexType.Matrix2);
        combined.NewIndex(42, descriptor).ShouldBe(42);
        combined.NewName("abc", descriptor).ShouldBe("abc");
    }

    [Fact]
    public void ShouldThrowWhenSetIndicesArgumentsAreNull()
    {
        var fromException = Should.Throw<ArgumentNullException>(() => TypesAndNamesTransformer.Utils.SetIndices(null!, [1]));
        var toException = Should.Throw<ArgumentNullException>(() => TypesAndNamesTransformer.Utils.SetIndices([1], null!));

        fromException.ParamName.ShouldBe("from");
        toException.ParamName.ShouldBe("to");
    }

    [Fact]
    public void ShouldThrowWhenChangeNameArgumentsAreNull()
    {
        var fromException = Should.Throw<ArgumentNullException>(() => TypesAndNamesTransformer.Utils.ChangeName(null!, ["a"]));
        var toException = Should.Throw<ArgumentNullException>(() => TypesAndNamesTransformer.Utils.ChangeName(["a"], null!));

        fromException.ParamName.ShouldBe("from");
        toException.ParamName.ShouldBe("to");
    }

    [Fact]
    public void ShouldThrowWhenComposedTransformersArrayIsNull()
    {
        var exception = Should.Throw<ArgumentNullException>(() => TypesAndNamesTransformer.Utils.And(null!));

        exception.ParamName.ShouldBe("transformers");
    }

    private static NameAndStructureOfIndices CreateDescriptor(string name)
    {
        return new NameAndStructureOfIndices(name, []);
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
