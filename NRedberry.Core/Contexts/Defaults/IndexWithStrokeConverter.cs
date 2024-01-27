using System;
using System.Text;
using NRedberry.Core.Indices;

namespace NRedberry.Contexts.Defaults;

/// <summary>
/// <see cref="IIndexSymbolConverter"/> for letters with strokes (e.g. \\alpha' or A'').
/// </summary>
/// <remarks>https://github.com/redberry-cas/core/blob/master/src/main/java/cc/redberry/core/context/defaults/IndexWithStrokeConverter.java</remarks>
public sealed class IndexWithStrokeConverter : IIndexSymbolConverter
{
    private readonly IIndexSymbolConverter converter;
    private readonly byte numberOfStrokes;
    private readonly string strokesString;

    public IndexWithStrokeConverter(IIndexSymbolConverter converter, byte numberOfStrokes)
    {
        if (numberOfStrokes + converter.GetType_() > byte.MaxValue)
        {
            throw new ArgumentException("Too many strokes.");
        }

        this.converter = converter;
        this.numberOfStrokes = numberOfStrokes;
        var sb = new StringBuilder();
        while (numberOfStrokes-- > 0)
            sb.Append('\'');
        strokesString = sb.ToString();
    }

    private string GetStrokes(string symbol)
    {
        return symbol.Substring(symbol.Length - numberOfStrokes);
    }

    private string GetBase(string symbol)
    {
        return symbol.Substring(0, symbol.Length - numberOfStrokes);
    }

    public bool ApplicableToSymbol(string symbol)
    {
        if (symbol.Length <= strokesString.Length)
            return false;
        if (!strokesString.Equals(GetStrokes(symbol)))
            return false;
        return converter.ApplicableToSymbol(GetBase(symbol));
    }

    public string GetSymbol(long code, OutputFormat mode)
    {
        return converter.GetSymbol(code, mode) + strokesString;
    }

    public int GetCode(string symbol)
    {
        return converter.GetCode(GetBase(symbol));
    }

    public int MaxNumberOfSymbols()
    {
        return converter.MaxNumberOfSymbols();
    }

    public byte GetType_()
    {
        return (byte) (IndexTypeMethods.AlphabetsCount + (numberOfStrokes * converter.GetType_()));
    }
}