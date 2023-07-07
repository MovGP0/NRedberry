using System;
using System.Text;
using NRedberry.Core.Indices;
using NRedberry.Core.Tensors;

namespace NRedberry.Core.Contexts.Defaults;

/// <summary>
/// <see cref="IIndexSymbolConverter"/> for letters with strokes (e.g. \\alpha' or A'').
/// </summary>
/// <remarks>https://github.com/redberry-cas/core/blob/master/src/main/java/cc/redberry/core/context/defaults/IndexWithStrokeConverter.java</remarks>
public sealed class IndexWithStrokeConverter : IIndexSymbolConverter
{
    private readonly IIndexSymbolConverter _converter;
    private readonly byte _numberOfStrokes;
    private readonly string _strokesString;

    public IndexWithStrokeConverter(IIndexSymbolConverter converter, byte numberOfStrokes)
    {
        if (numberOfStrokes + converter.GetType_() > byte.MaxValue)
            throw new ArgumentException("Too many strokes.");
        _converter = converter;
        _numberOfStrokes = numberOfStrokes;
        StringBuilder sb = new StringBuilder();
        while (numberOfStrokes-- > 0)
            sb.Append('\'');
        _strokesString = sb.ToString();
    }

    private string GetStrokes(string symbol)
    {
        return symbol.Substring(symbol.Length - _numberOfStrokes);
    }

    private string GetBase(string symbol)
    {
        return symbol.Substring(0, symbol.Length - _numberOfStrokes);
    }

    public bool ApplicableToSymbol(string symbol)
    {
        if (symbol.Length <= _strokesString.Length)
            return false;
        if (!_strokesString.Equals(GetStrokes(symbol)))
            return false;
        return _converter.ApplicableToSymbol(GetBase(symbol));
    }

    public string GetSymbol(long code, OutputFormat mode)
    {
        return _converter.GetSymbol(code, mode) + _strokesString;
    }

    public long GetCode(string symbol)
    {
        return _converter.GetCode(GetBase(symbol));
    }

    public long MaxNumberOfSymbols()
    {
        return _converter.MaxNumberOfSymbols();
    }

    public byte GetType_()
    {
        return (byte) (IndexTypeMethods.AlphabetsCount + (_numberOfStrokes * _converter.GetType_()));
    }
}