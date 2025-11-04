using System.Globalization;
using NRedberry.Core.Exceptions;

namespace NRedberry.Contexts.Defaults;

/// <summary>
/// <see cref="IIndexSymbolConverter"/> for subscripted letters (e.g. \\alpha_{2} or A_{4}).
/// </summary>
/// <param name="innerConverter">The inner converter to extend with subscripted symbols.</param>
/// <remarks>https://github.com/redberry-cas/core/blob/master/src/main/java/cc/redberry/core/context/defaults/IndexConverterExtender.java</remarks>
public sealed class IndexConverterExtender(IIndexSymbolConverter innerConverter) : IIndexSymbolConverter
{
    /// <summary>
    /// Predefined converters for subscripted letters.
    /// </summary>
    public static IndexConverterExtender LatinLowerEx { get; } = new(LatinLowerCaseConverter.Instance);
    public static IndexConverterExtender LatinUpperEx { get; } = new(LatinUpperCaseConverter.Instance);
    public static IndexConverterExtender GreekLowerEx { get; } = new(GreekLaTeXLowerCaseConverter.Instance);
    public static IndexConverterExtender GreekUpperEx { get; } = new(GreekLaTeXUpperCaseConverter.Instance);

    public bool ApplicableToSymbol(string symbol)
    {
        if (string.IsNullOrEmpty(symbol))
            return false;

        if (!symbol.Contains('_'))
            return innerConverter.ApplicableToSymbol(symbol);

        var split = symbol.Split('_');
        if (split.Length != 2 || string.IsNullOrEmpty(split[1]))
            return false;

        if (split[1][0] == '{')
        {
            if (split[1].Length < 3)
                return false;

            split[1] = split[1][1..^1];
        }

        return int.TryParse(split[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out _)
            && innerConverter.ApplicableToSymbol(split[0]);
    }

    public int GetCode(string symbol)
    {
        if (string.IsNullOrEmpty(symbol))
            throw new IndexConverterException();

        if (!symbol.Contains('_'))
            return innerConverter.GetCode(symbol);

        var split = symbol.Split('_');
        if (split.Length != 2 || string.IsNullOrEmpty(split[1]))
            throw new IndexConverterException();

        if (split[1][0] == '{')
        {
            if (split[1].Length < 3)
                throw new IndexConverterException();

            split[1] = split[1][1..^1];
        }

        if (!int.TryParse(split[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out int num))
            throw new IndexConverterException();

        return num * (1 + innerConverter.MaxNumberOfSymbols) + innerConverter.GetCode(split[0]);
    }

    public string GetSymbol(long code, OutputFormat mode)
    {
        var subscript = (int)(code / (innerConverter.MaxNumberOfSymbols + 1));
        var symbol = innerConverter.GetSymbol(code % (innerConverter.MaxNumberOfSymbols + 1), mode);

        return subscript == 0
            ? symbol
            : mode.Is(OutputFormat.WolframMathematica)
                ? $"Subscript[{symbol}, {subscript}]"
                : mode.Is(OutputFormat.Maple)
                    ? $"{symbol}{subscript}"
                    : $"{symbol}_{{{subscript}}}";
    }

    public int MaxNumberOfSymbols => 10 * (innerConverter.MaxNumberOfSymbols + 1) - 1;

    public byte Type => innerConverter.Type;
}
