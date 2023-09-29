using System;

namespace NRedberry.Contexts.Defaults;

/// <summary>
/// <see cref="IIndexSymbolConverter"/> for subscripted letters (e.g. \\alpha_{2} or A_{4}).
/// </summary>
/// <remarks>https://github.com/redberry-cas/core/blob/master/src/main/java/cc/redberry/core/context/defaults/IndexConverterExtender.java</remarks>
public sealed class IndexConverterExtender : IIndexSymbolConverter
{
    public static readonly IndexConverterExtender LatinLowerEx = new(LatinLowerCaseConverter.Instance);
    public static readonly IndexConverterExtender LatinUpperEx = new(LatinUpperCaseConverter.Instance);
    public static readonly IndexConverterExtender GreekLowerEx = new(GreekLaTeXLowerCaseConverter.Instance);
    public static readonly IndexConverterExtender GreekUpperEx = new(GreekLaTeXUpperCaseConverter.Instance);

    private readonly IIndexSymbolConverter innerConverter;

    public IndexConverterExtender(IIndexSymbolConverter innerConverter)
    {
        this.innerConverter = innerConverter;
    }

    public bool ApplicableToSymbol(string symbol)
    {
        if (!symbol.Contains("_"))
            return innerConverter.ApplicableToSymbol(symbol);
        var split = symbol.Split("_");
        if (split.Length != 2 || split[1].Length == 0)
            return false;
        if (split[1][0] == '{')
        {
            if (split[1].Length < 3)
                return false;
            split[1] = split[1].Substring(1, split[1].Length - 1);
        }
        try
        {
            int.Parse(split[1]);
            return innerConverter.ApplicableToSymbol(split[0]);
        }
        catch (FormatException)
        {
            return false;
        }
    }

    public int GetCode(string symbol)
    {
        if (!symbol.Contains("_"))
            return innerConverter.GetCode(symbol);
        var split = symbol.Split("_");
        if (split.Length != 2 || split[1].Length == 0)
            throw new IndexConverterException();
        if (split[1][0] == '{')
        {
            if (split[1].Length < 3)
                throw new IndexConverterException();
            split[1] = split[1].Substring(1, split[1].Length - 1);
        }
        int num;
        try
        {
            num = int.Parse(split[1]);
        }
        catch (FormatException)
        {
            throw new IndexConverterException();
        }
        return num * (1 + innerConverter.MaxNumberOfSymbols()) + innerConverter.GetCode(split[0]);
    }

    public string GetSymbol(long code, OutputFormat mode)
    {
        long num = code / (innerConverter.MaxNumberOfSymbols() + 1);
        if (num == 0)
            return innerConverter.GetSymbol(code, mode);
        else
            return innerConverter.GetSymbol(code % (innerConverter.MaxNumberOfSymbols() + 1), mode) + "_" + "{" + num + "}";
    }

    public byte GetType_()
    {
        return innerConverter.GetType_();
    }

    public int MaxNumberOfSymbols()
    {
        return 10 * (innerConverter.MaxNumberOfSymbols() + 1) - 1;
    }
}