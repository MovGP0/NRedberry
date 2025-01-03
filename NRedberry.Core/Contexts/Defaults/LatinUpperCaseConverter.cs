﻿using NRedberry.Core.Exceptions;

namespace NRedberry.Contexts.Defaults;

/// <summary>
/// <see cref="IIndexSymbolConverter"/> for latin upper case letters.
/// </summary>
/// <remarks>https://github.com/redberry-cas/core/blob/master/src/main/java/cc/redberry/core/context/defaults/LatinUpperCaseConverter.java</remarks>
public sealed class LatinUpperCaseConverter : IIndexSymbolConverter
{
    public static readonly LatinUpperCaseConverter Instance = new();

    private LatinUpperCaseConverter()
    {
    }

    public bool ApplicableToSymbol(string symbol)
    {
        if (symbol.Length == 1)
        {
            char sym = symbol[0];
            if (sym >= 0x41 && sym <= 0x5A)
                return true;
        }
        return false;
    }

    public int GetCode(string symbol) => symbol[0] - 0x41;

    public string GetSymbol(long code, OutputFormat mode) // assuming OutputFormat is a defined type
    {
        long number = code + 0x41;
        if (number > 0x5A)
            throw new IndexConverterException(); // assuming IndexConverterException is a defined type
        return ((char) number).ToString();
    }

    /// <summary>
    /// Always returns 1.
    /// </summary>
    public byte Type => 1;

    public int MaxNumberOfSymbols => 25;
}
