using NRedberry.Contexts.Defaults;

namespace NRedberry.Core.Indices;

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

    public static string GetShortString(this IndexType indexType)
    {
        return CommonNames.Any(e => e.Value == indexType)
            ? CommonNames.First(e => e.Value == indexType).Key
            : indexType.ToString();
    }

    public static IndexType FromShortString(string stringVal)
    {
        return CommonNames[stringVal];
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
        byte[] bytes = new byte[TypesCount];
        for (byte i = 0; i < TypesCount; ++i)
            bytes[i] = i;
        return bytes;
    }

    public static IndexType GetType_(byte type) => GetType(type);
    public static IndexType GetType(byte type)
    {
        foreach (IndexType indexType in Enum.GetValues(typeof(IndexType)))
            if (indexType.GetType_() == type)
                return indexType;
        throw new ArgumentException("No such type: " + type);
    }

    public static IIndexSymbolConverter[] GetAllConverters()
    {
        List<IIndexSymbolConverter> converters = [];
        foreach (IndexType type in Enum.GetValues(typeof(IndexType)))
            converters.Add(type.GetSymbolConverter());
        return converters.ToArray();
    }
}