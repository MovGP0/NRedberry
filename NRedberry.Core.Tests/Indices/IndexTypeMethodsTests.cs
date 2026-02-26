using NRedberry.Indices;
using Xunit;

namespace NRedberry.Core.Tests.Indices;

public sealed class IndexTypeMethodsTests
{
    [Fact]
    public void ShouldExposeAllIndexTypesInValues()
    {
        IndexType[] values = IndexTypeMethods.Values;
        IndexType[] enumValues = Enum.GetValues<IndexType>();

        Assert.Equal((int)IndexTypeMethods.TypesCount, values.Length);
        Assert.Equal(enumValues.Length, values.Length);
        Assert.Equal(enumValues, values);
    }

    [Theory]
    [InlineData(IndexType.LatinLower, "l")]
    [InlineData(IndexType.GreekUpper, "G")]
    [InlineData(IndexType.Matrix1, "l'")]
    [InlineData(IndexType.Matrix4, "G'")]
    public void ShouldRoundtripShortStringForRepresentativeTypes(IndexType indexType, string shortString)
    {
        Assert.Equal(shortString, indexType.GetShortString());
        Assert.Equal(indexType, IndexTypeMethods.FromShortString(shortString));
    }

    [Fact]
    public void ShouldResolveTypeForValidByteValuesAndThrowForInvalidValues()
    {
        byte[] types = IndexTypeMethods.GetBytes();

        foreach (byte type in types)
        {
            IndexType indexType = IndexTypeMethods.GetType(type);
            Assert.Equal(type, indexType.GetType_());
        }

        Assert.Throws<ArgumentException>(() => IndexTypeMethods.GetType(IndexTypeMethods.TypesCount));
        Assert.Throws<ArgumentException>(() => IndexTypeMethods.GetType(byte.MaxValue));
    }

    [Fact]
    public void ShouldReturnCloneFromGetBytes()
    {
        byte[] original = IndexTypeMethods.GetBytes();
        byte[] mutated = IndexTypeMethods.GetBytes();

        mutated[0] = byte.MaxValue;

        byte[] next = IndexTypeMethods.GetBytes();
        Assert.Equal(original, next);
    }

    [Fact]
    public void ShouldReturnCloneFromGetAllConverters()
    {
        IIndexSymbolConverter[] original = IndexTypeMethods.GetAllConverters();
        IIndexSymbolConverter[] mutated = IndexTypeMethods.GetAllConverters();

        mutated[0] = mutated[1];

        IIndexSymbolConverter[] next = IndexTypeMethods.GetAllConverters();
        Assert.Same(original[0], next[0]);
        Assert.NotSame(mutated[0], next[0]);
    }

    [Fact]
    public void ShouldKeepGetSymbolConverterAndTypeMappingsConsistent()
    {
        foreach (IndexType indexType in IndexTypeMethods.Values)
        {
            IIndexSymbolConverter converter = indexType.GetSymbolConverter();
            byte type = indexType.GetType_();

            Assert.Equal(type, converter.Type);
            Assert.Equal(type, IndexTypeMethods.GetType(type).GetType_());
            Assert.Equal(type, IndexTypeMethods.GetType_(type).GetType_());
            Assert.Equal(type, IndexTypeMethods.GetType(type).GetSymbolConverter().Type);
        }
    }
}
