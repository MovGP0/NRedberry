using NRedberry.Core.Exceptions;
using NRedberry.Indices;

namespace NRedberry.Contexts;

public sealed class IndexConverterManager
{
    public static readonly IndexConverterManager Default = new(IndexTypeMethods.GetAllConverters());
    private readonly IIndexSymbolConverter[] _converters;

    public IndexConverterManager(IIndexSymbolConverter[] converters)
    {
        ArgumentNullException.ThrowIfNull(converters);

        var types = new HashSet<byte>(converters.Length);
        foreach (var converter in converters)
        {
            if (types.Contains(converter.Type))
                throw new ArgumentException("Several converters for same type.");
            types.Add(converter.Type);
        }

        _converters = converters;
    }

    public string GetSymbol(long code, OutputFormat outputFormat)
    {
        byte typeId = (byte)((code >> 24) & 0x7F);
        long number = code & 0xFFFF;
        try
        {
            foreach (var converter in _converters)
            {
                if (converter.Type == typeId)
                {
                    return converter.GetSymbol((int)number, outputFormat);
                }
            }

            throw new ArgumentException("No appropriate converter for typeId 0x" + typeId.ToString("X"));
        }
        catch (IndexConverterException)
        {
            throw new ArgumentException("Index 0x" + code.ToString("X") + " conversion error");
        }
    }

    public int GetCode(string index)
    {
        try
        {
            foreach (var converter in _converters)
            {
                if (converter.ApplicableToSymbol(index))
                    return (converter.GetCode(index) & 0xFFFF) | ((converter.Type & 0x7F) << 24);
            }

            throw new ArgumentException("No available converters for such symbol : " + index);
        }
        catch (IndexConverterException)
        {
            throw new ArgumentException("No available converters for such symbol : " + index);
        }
    }
}
