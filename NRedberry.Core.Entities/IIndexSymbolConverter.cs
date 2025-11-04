namespace NRedberry;

/// <summary>
/// Interface defining common functionality for string-integer converters for indices of the same particular type.
/// </summary>
public interface IIndexSymbolConverter
{
    /// <summary>
    /// Returns true if this converter can convert an index string to an integer representation.
    /// </summary>
    /// <param name="symbol">The string representation of the index.</param>
    /// <returns>True if the converter can convert the index string to an integer representation; otherwise, false.</returns>
    bool ApplicableToSymbol(string symbol);

    /// <summary>
    /// Gets the string representation from the specified integer representation of a single index
    /// in the specified <paramref name="outputFormat"/>.
    /// </summary>
    /// <param name="code">The integer representation of the index.</param>
    /// <param name="outputFormat">The output format.</param>
    /// <returns>The string representation of the specified integer index, according to the specified output format.</returns>
    /// <exception cref="IndexConverterException">Thrown if the code does not correspond to this converter.</exception>
    string GetSymbol(long code, OutputFormat outputFormat);

    /// <summary>
    /// Gets the integer representation from the specified string representation of a single index.
    /// </summary>
    /// <param name="symbol">The string representation of the index.</param>
    /// <returns>The integer representation of the index.</returns>
    /// <exception cref="IndexConverterException">Thrown if the converter is not applicable to the specified symbol.</exception>
    int GetCode(string symbol);

    /// <summary>
    /// Gets the number of symbols supported by this converter.
    /// For example, a LatinLowerCaseConverter would support 26 symbols (the size of the Latin alphabet).
    /// </summary>
    int MaxNumberOfSymbols { get; }

    /// <summary>
    /// Gets the type of indices that this converter processes.
    /// The type value is unique for each converter.
    /// </summary>
    byte Type { get; }
}
