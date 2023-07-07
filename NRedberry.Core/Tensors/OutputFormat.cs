namespace NRedberry.Core.Tensors;

public enum OutputFormat
{
    /// <summary>
    /// This format specifies expressions to be outputted in the LaTeX notation. The produces strings
    /// can be simply putted in some LaTeX math environments and compiled via LaTeX compiler.
    /// </summary>
    LaTeX,
    /// <summary>
    /// This format specifies greek letters to be printed as is (if stdout supports utf-8 characters).
    /// In other aspects it is similar to {@link OutputFormat#Redberry}
    /// </summary>
    UTF8,
    /// <summary>
    /// This format specifies expressions to be outputted in the Redberry input notation. The produces strings
    /// can be parsed in Redberry.
    /// </summary>
    Redberry,
    /// <summary>
    /// This format specifies expressions to be outputted in the Wolfram Mathematica input notation.
    /// </summary>
    WolframMathematica,
    /// <summary>
    /// This format specifies expressions to be outputted in the Redberry console notation.
    /// </summary>
    RedberryConsole
}