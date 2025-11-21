using NRedberry.Core.Exceptions;
using NRedberry.Indices;

namespace NRedberry.Contexts;

public sealed class IndexConverterManager
{
    public static readonly IndexConverterManager Default = new(IndexTypeMethods.GetAllConverters());
    private readonly IIndexSymbolConverter[] converters;

    public IndexConverterManager(IIndexSymbolConverter[] converters)
    {
        HashSet<byte> types = new HashSet<byte>(converters.Length);
        foreach (IIndexSymbolConverter converter in converters)
        {
            if (types.Contains(converter.Type))
                throw new ArgumentException("Several converters for same type.");
            types.Add(converter.Type);
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
            {
                if (converter.Type == typeId)
                {
                    return converter.GetSymbol(number, outputFormat);
                }
            }

            throw new ArgumentException("No appropriate converter for typeId 0x" + typeId.ToString("X"));
        }
        catch (IndexConverterException e)
        {
            throw new ArgumentException("Index 0x" + code.ToString("X") + " conversion error");
        }
    }

    public int GetCode(string index)
    {
        try
        {
            foreach (IIndexSymbolConverter converter in converters)
            {
                if (converter.ApplicableToSymbol(index))
                    return (converter.GetCode(index) & 0xFFFF) | ((converter.Type & 0x7F) << 24);
            }

            throw new ArgumentException("No available converters for such symbol : " + index);
        }
        catch (IndexConverterException e)
        {
            throw new ArgumentException("No available converters for such symbol : " + index);
        }
    }
}
