using NRedberry.Indices;

namespace NRedberry.Core.Tests.Indices;

public sealed class IndexTypeMethodsTests
{
    [Fact]
    public void ShouldExposeAllIndexTypesInValues()
    {
        IndexType[] values = IndexTypeMethods.Values;
        IndexType[] enumValues = Enum.GetValues<IndexType>();

        values.Length.ShouldBe((int)IndexTypeMethods.TypesCount);
        values.Length.ShouldBe(enumValues.Length);
        values.ShouldBe(enumValues);
    }

    [Theory]
    [InlineData(IndexType.LatinLower, "l")]
    [InlineData(IndexType.GreekUpper, "G")]
    [InlineData(IndexType.Matrix1, "l'")]
    [InlineData(IndexType.Matrix4, "G'")]
    public void ShouldRoundtripShortStringForRepresentativeTypes(IndexType indexType, string shortString)
    {
        indexType.GetShortString().ShouldBe(shortString);
        IndexTypeMethods.FromShortString(shortString).ShouldBe(indexType);
    }

    [Fact]
    public void ShouldResolveTypeForValidByteValuesAndThrowForInvalidValues()
    {
        byte[] types = IndexTypeMethods.GetBytes();

        foreach (byte type in types)
        {
            IndexType indexType = IndexTypeMethods.GetType(type);
            indexType.GetType_().ShouldBe(type);
        }

        Should.Throw<ArgumentException>(() => IndexTypeMethods.GetType(IndexTypeMethods.TypesCount));
        Should.Throw<ArgumentException>(() => IndexTypeMethods.GetType(byte.MaxValue));
    }

    [Fact]
    public void ShouldReturnCloneFromGetBytes()
    {
        byte[] original = IndexTypeMethods.GetBytes();
        byte[] mutated = IndexTypeMethods.GetBytes();

        mutated[0] = byte.MaxValue;

        byte[] next = IndexTypeMethods.GetBytes();
        next.ShouldBe(original);
    }

    [Fact]
    public void ShouldReturnCloneFromGetAllConverters()
    {
        IIndexSymbolConverter[] original = IndexTypeMethods.GetAllConverters();
        IIndexSymbolConverter[] mutated = IndexTypeMethods.GetAllConverters();

        mutated[0] = mutated[1];

        IIndexSymbolConverter[] next = IndexTypeMethods.GetAllConverters();
        next[0].ShouldBeSameAs(original[0]);
        next[0].ShouldNotBeSameAs(mutated[0]);
    }

    [Fact]
    public void ShouldKeepGetSymbolConverterAndTypeMappingsConsistent()
    {
        foreach (IndexType indexType in IndexTypeMethods.Values)
        {
            IIndexSymbolConverter converter = indexType.GetSymbolConverter();
            byte type = indexType.GetType_();

            converter.Type.ShouldBe(type);
            IndexTypeMethods.GetType(type).GetType_().ShouldBe(type);
            IndexTypeMethods.GetType_(type).GetType_().ShouldBe(type);
            IndexTypeMethods.GetType(type).GetSymbolConverter().Type.ShouldBe(type);
        }
    }
}
