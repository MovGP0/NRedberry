using System;
using System.Collections.Generic;
using NRedberry.Core.Indices;
using NRedberry.Core.Tensors;

namespace NRedberry.Core.Contexts;

public sealed class IndexConverterManager
{
    public static readonly IndexConverterManager Default = new IndexConverterManager(IndexTypeMethods.GetAllConverters());
    private readonly IIndexSymbolConverter[] converters;

    public IndexConverterManager(IIndexSymbolConverter[] converters)
    {
        HashSet<byte> types = new HashSet<byte>(converters.Length);
        foreach (IIndexSymbolConverter converter in converters)
        {
            if (types.Contains(converter.GetType_()))
                throw new ArgumentException("Several converters for same type.");
            types.Add(converter.GetType_());
        }
        this.converters = converters;
    }

    public string GetSymbol(long code, OutputFormat outputFormat)
    {
        byte typeId = (byte)((code >> 24) & 0x7F);
        long number = code & 0xFFFF;
        try
        {
            foreach (IIndexSymbolConverter converter in converters)
                if (converter.GetType_() == typeId)
                {
                    return converter.GetSymbol(number, outputFormat);
                }
            throw new ArgumentException("No appropriate converter for typeId 0x" + typeId.ToString("X"));
        }
        catch (IndexConverterException e)
        {
            throw new ArgumentException("Index 0x" + code.ToString("X") + " conversion error");
        }
    }

    public long GetCode(string index)
    {
        try
        {
            foreach (IIndexSymbolConverter converter in converters)
                if (converter.ApplicableToSymbol(index))
                    return (converter.GetCode(index) & 0xFFFF) | ((converter.GetType_() & 0x7F) << 24);
            throw new ArgumentException("No available converters for such symbol : " + index);
        }
        catch (IndexConverterException e)
        {
            throw new ArgumentException("No available converters for such symbol : " + index);
        }
    }
}