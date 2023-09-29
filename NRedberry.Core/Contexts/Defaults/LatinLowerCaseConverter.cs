namespace NRedberry.Contexts.Defaults;

/// <summary>
/// <see cref="IIndexSymbolConverter"/> for latin lower case letters.
/// </summary>
/// <remarks>https://github.com/redberry-cas/core/blob/master/src/main/java/cc/redberry/core/context/defaults/LatinLowerCaseConverter.java</remarks>
public sealed class LatinLowerCaseConverter : IIndexSymbolConverter
{
    public static readonly byte Type = 0;
    public static readonly LatinLowerCaseConverter Instance = new();

    private LatinLowerCaseConverter()
    {
    }

    public bool ApplicableToSymbol(string symbol)
    {
        if (symbol.Length == 1)
        {
            char sym = symbol[0];
            if (sym >= 0x61 && sym <= 0x7A)
                return true;
        }
        return false;
    }

    public int GetCode(string symbol)
    {
        return symbol[0] - 0x61;
    }

    public string GetSymbol(long code, OutputFormat mode)
    {
        long number = code + 0x61;
        if (number > 0x7A)
            throw new IndexConverterException();
        return ((char) number).ToString();
    }

    public byte GetType_()
    {
        return Type;
    }

    public int MaxNumberOfSymbols()
    {
        return 25;
    }
}
