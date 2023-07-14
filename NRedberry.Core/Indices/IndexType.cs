using System;
using System.Collections.Generic;
using System.Linq;
using NRedberry.Core.Contexts;
using NRedberry.Core.Contexts.Defaults;

namespace NRedberry.Core.Indices;

/// <summary>
/// This {@code enum} is a container of the information on all available index types and appropriate converters. This
/// {@code enum} is scanning at the initialization of {@link ContextSettings} and all the values are putting in the
/// Context as default indices types.
/// </summary>
public enum IndexType
{
    LatinLower,
    LatinUpper,
    GreekLower,
    GreekUpper,
    Matrix1,
    Matrix2,
    Matrix3,
    Matrix4
}

public static class IndexTypeMethods
{
    private static Dictionary<string, IndexType> commonNames = new Dictionary<string, IndexType>
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

    private static Dictionary<IndexType, IIndexSymbolConverter> converterMap = new()
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

    public static IndexType[] Values { get; } = new IndexType[]
    {
        IndexType.LatinLower,
        IndexType.LatinUpper,
        IndexType.GreekLower,
        IndexType.GreekUpper,
        IndexType.Matrix1,
        IndexType.Matrix2,
        IndexType.Matrix3,
        IndexType.Matrix4
    };

    public static string GetShortString(this IndexType indexType)
    {
        return commonNames.Any(e => e.Value == indexType)
            ? commonNames.First(e => e.Value == indexType).Key
            : indexType.ToString();
    }

    public static IndexType FromShortString(string stringVal)
    {
        return commonNames[stringVal];
    }

    public static IIndexSymbolConverter GetSymbolConverter(this IndexType indexType)
    {
        return converterMap[indexType];
    }

    public static byte GetType_(this IndexType indexType)
    {
        return converterMap[indexType].GetType_();
    }

    public static byte[] GetBytes()
    {
        byte[] bytes = new byte[TypesCount];
        for (byte i = 0; i < TypesCount; ++i)
            bytes[i] = i;
        return bytes;
    }

    public static IndexType GetType_(byte type)
    {
        foreach (IndexType indexType in Enum.GetValues(typeof(IndexType)))
            if (indexType.GetType_() == type)
                return indexType;
        throw new ArgumentException("No such type: " + type);
    }

    public static IIndexSymbolConverter[] GetAllConverters()
    {
        List<IIndexSymbolConverter> converters = new List<IIndexSymbolConverter>();
        foreach (IndexType type in Enum.GetValues(typeof(IndexType)))
            converters.Add(type.GetSymbolConverter());
        return converters.ToArray();
    }
}