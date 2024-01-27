using System;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace NRedberry.Contexts.Defaults;

/// <summary>
/// <see cref="IIndexSymbolConverter"/> for subscripted letters (e.g. \\alpha_{2} or A_{4}).
/// </summary>
/// <remarks>https://github.com/redberry-cas/core/blob/master/src/main/java/cc/redberry/core/context/defaults/IndexConverterExtender.java</remarks>
public sealed class IndexConverterExtender(IIndexSymbolConverter innerConverter) : IIndexSymbolConverter
{
    public static IndexConverterExtender LatinLowerEx { get; } = new(LatinLowerCaseConverter.Instance);
    public static IndexConverterExtender LatinUpperEx { get; } = new(LatinUpperCaseConverter.Instance);
    public static IndexConverterExtender GreekLowerEx { get; } = new(GreekLaTeXLowerCaseConverter.Instance);
    public static IndexConverterExtender GreekUpperEx { get; } = new(GreekLaTeXUpperCaseConverter.Instance);

    public bool ApplicableToSymbol(string symbol)
    {
        if (!symbol.Contains('_'))
        {
            return innerConverter.ApplicableToSymbol(symbol);
        }

        var split = symbol.Split('_');
        if (split.Length != 2 || split[1].Length == 0)
        {
            return false;
        }

        if (split[1][0] == '{')
        {
            if (split[1].Length < 3)
            {
                return false;
            }

            split[1] = split[1].Substring(1, split[1].Length - 1);
        }
        try
        {
            return int.TryParse(split[1], CultureInfo.InvariantCulture, out _)
                   && innerConverter.ApplicableToSymbol(split[0]);
        }
        catch (FormatException)
        {
            return false;
        }
    }

    public int GetCode(string symbol)
    {
        if (!symbol.Contains('_'))
        {
            return innerConverter.GetCode(symbol);
        }

        var split = symbol.Split('_');
        if (split.Length != 2 || split[1].Length == 0)
        {
            throw new IndexConverterException();
        }

        if (split[1][0] == '{')
        {
            if (split[1].Length < 3)
            {
                throw new IndexConverterException();
            }

            split[1] = split[1].Substring(1, split[1].Length - 1);
        }

        int num;
        try
        {
            num = int.Parse(split[1], CultureInfo.InvariantCulture);
        }
        catch (FormatException)
        {
            throw new IndexConverterException();
        }

        return num * (1 + innerConverter.MaxNumberOfSymbols()) + innerConverter.GetCode(split[0]);
    }

    public string GetSymbol(long code, OutputFormat mode)
    {
        var num = code / (innerConverter.MaxNumberOfSymbols() + 1);
        return num == 0
            ? innerConverter.GetSymbol(code, mode)
            : $"{innerConverter.GetSymbol(code % (innerConverter.MaxNumberOfSymbols() + 1), mode)}_{{{num}}}";
    }

    [Pure]
    public byte GetType_()
        => innerConverter.GetType_();

    public int MaxNumberOfSymbols()
        => 10 * (innerConverter.MaxNumberOfSymbols() + 1) - 1;
}