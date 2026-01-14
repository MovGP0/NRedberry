using NRedberry.Contexts.Defaults;

namespace NRedberry.Indices;

public static class IndexTypeMethods
{
    private static readonly Dictionary<string, IndexType> CommonNames = new()
    {
        { "l", IndexType.LatinLower },
        { "L", IndexType.LatinUpper },
        { "l'", IndexType.Matrix1 },
        { "L'", IndexType.Matrix2 },
        { "g", IndexType.GreekLower },
        { "G", IndexType.GreekUpper },
        { "g'", IndexType.Matrix3 },
        { "G'", IndexType.Matrix4 }
    };

    public const byte TypesCount = 8;
    public const byte AlphabetsCount = 4;

    private static readonly Dictionary<IndexType, IIndexSymbolConverter> ConverterMap = new()
    {
        { IndexType.LatinLower, new IndexWithStrokeConverter(IndexConverterExtender.LatinLowerEx, 1) },
        { IndexType.LatinUpper, new IndexWithStrokeConverter(IndexConverterExtender.LatinUpperEx, 1) },
        { IndexType.GreekLower, new IndexWithStrokeConverter(IndexConverterExtender.GreekLowerEx, 1) },
        { IndexType.GreekUpper, new IndexWithStrokeConverter(IndexConverterExtender.GreekUpperEx, 1) },
        { IndexType.Matrix1, new IndexWithStrokeConverter(IndexConverterExtender.LatinLowerEx, 1) },
        { IndexType.Matrix2, new IndexWithStrokeConverter(IndexConverterExtender.LatinUpperEx, 1) },
        { IndexType.Matrix3, new IndexWithStrokeConverter(IndexConverterExtender.GreekLowerEx, 1) },
        { IndexType.Matrix4, new IndexWithStrokeConverter(IndexConverterExtender.GreekUpperEx, 1) },
    };

    private static readonly Dictionary<IndexType, string> ShortNames = new();
    private static readonly IndexType[] ByteToEnum = new IndexType[TypesCount];
    private static readonly byte[] AllBytes;
    private static readonly IIndexSymbolConverter[] AllConverters;

    public static IndexType[] Values { get; } =
    [
        IndexType.LatinLower,
        IndexType.LatinUpper,
        IndexType.GreekLower,
        IndexType.GreekUpper,
        IndexType.Matrix1,
        IndexType.Matrix2,
        IndexType.Matrix3,
        IndexType.Matrix4
    ];

    static IndexTypeMethods()
    {
        foreach ((string shortName, IndexType indexType) in CommonNames)
        {
            ShortNames[indexType] = shortName;
        }

        int length = Values.Length;
        AllBytes = new byte[length];
        AllConverters = new IIndexSymbolConverter[length];
        for (int i = 0; i < length; ++i)
        {
            IndexType indexType = Values[i];
            IIndexSymbolConverter converter = ConverterMap[indexType];
            byte type = converter.Type;
            ByteToEnum[type] = indexType;
            AllBytes[i] = type;
            AllConverters[i] = converter;
        }
    }

    public static string GetShortString(this IndexType indexType)
    {
        return ShortNames.TryGetValue(indexType, out string? shortName)
            ? shortName
            : indexType.ToString();
    }

    public static IndexType? FromShortString(string stringVal)
    {
        ArgumentNullException.ThrowIfNull(stringVal);
        return CommonNames.TryGetValue(stringVal, out IndexType type) ? type : null;
    }

    public static IIndexSymbolConverter GetSymbolConverter(this IndexType indexType)
    {
        return ConverterMap[indexType];
    }

    public static byte GetType_(this IndexType indexType)
    {
        return ConverterMap[indexType].Type;
    }

    public static byte[] GetBytes()
    {
        return (byte[])AllBytes.Clone();
    }

    public static IndexType GetType_(byte type) => GetType(type);

    public static IndexType GetType(byte type)
    {
        if (type >= TypesCount)
        {
            throw new ArgumentException("No such type: " + type);
        }

        return ByteToEnum[type];
    }

    public static IIndexSymbolConverter[] GetAllConverters()
    {
        return (IIndexSymbolConverter[])AllConverters.Clone();
    }
}
