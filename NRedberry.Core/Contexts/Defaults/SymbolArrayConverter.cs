using NRedberry.Core.Exceptions;

namespace NRedberry.Contexts.Defaults;

/// <summary>
/// Base class for other <see cref="IIndexSymbolConverter"/>.
/// </summary>
/// <remarks>https://github.com/redberry-cas/core/blob/master/src/main/java/cc/redberry/core/context/defaults/SymbolArrayConverter.java</remarks>
public abstract class SymbolArrayConverter : IIndexSymbolConverter
{
    private string[] Symbols { get; }
    private string[] UTF { get; }

    protected SymbolArrayConverter(string[] symbols, string[] utf)
    {
        Symbols = symbols;
        UTF = utf;
        if (symbols.Length != utf.Length)
            throw new ApplicationException();
    }

    public bool ApplicableToSymbol(string symbol)
    {
        return Symbols.Any(s => s.Equals(symbol));
    }

    public int GetCode(string symbol)
    {
        for (var i = 0; i < Symbols.Length; ++i)
        {
            if (Symbols[i].Equals(symbol))
            {
                return i;
            }
        }

        throw new IndexConverterException();
    }

    public string GetSymbol(int code, OutputFormat mode)
    {
        try
        {
            if (mode == OutputFormat.UTF8)
                return UTF[code];
            if (mode == OutputFormat.Redberry)
                return "\\" + Symbols[code];
            return Symbols[code];
        }
        catch (ArgumentOutOfRangeException e)
        {
            throw new IndexConverterException(e.Message, e);
        }
    }

    public int MaxNumberOfSymbols => Symbols.Length - 1;
    public abstract byte Type { get; }
}
