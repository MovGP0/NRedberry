namespace NRedberry;

public struct OutputFormat : IEquatable<OutputFormat>
{
    /// <summary>
    /// This format specifies expressions to be outputted in the LaTeX notation. The produces strings
    /// can be simply putted in some LaTeX math environments and compiled via LaTeX compiler.
    /// </summary>
    public static OutputFormat LaTeX { get; } = new(0, "^", "_");

    /// <summary>
    /// This format specifies greek letters to be printed as is (if stdout supports utf-8 characters).
    /// In other aspects it is similar to {@link OutputFormat#Redberry}
    /// </summary>
    public static OutputFormat UTF8 { get; } = new(1, "^", "_");

    /// <summary>
    /// This format specifies expressions to be outputted in the Redberry input notation. Produced strings
    /// can be parsed in Redberry.
    /// </summary>
    public static OutputFormat Redberry { get; } = new(2, "^", "_");

    /// <summary>
    /// This format specifies expressions to be outputted in the Redberry input notation. Produced strings
    /// can be parsed in Redberry.
    /// </summary>
    public static OutputFormat Cadabra { get; } = new(3, "^", "_");

    /// <summary>
    /// This format specifies expressions to be outputted in the Wolfram Mathematica input notation.
    /// </summary>
    public static OutputFormat WolframMathematica { get; } = new(4, "", "-");

    /// <summary>
    /// This format specifies expressions to be outputted in the Maplesoft Maple input notation.
    /// </summary>
    public static OutputFormat Maple { get; } = new(5, "~", "");

    /// <summary>
    /// This format will not print explicit indices of matrices. E.g. if A and B are matrices, that it will
    /// produce A*B instead of A^i'_j'*B^j'_k'.
    /// </summary>
    public static OutputFormat SimpleRedberry { get; } = new(6, "^", "_", false);

    /// <summary>
    /// Format used to export expressions for C/C++
    /// </summary>
    public static OutputFormat C { get; } = new(7, "^", "_");

    /// <summary>
    /// Unique identifier.
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Prefix, which specifies upper index (e.g. '^' in LaTeX)
    /// </summary>
    public string UpperIndexPrefix { get; }

    /// <summary>
    /// Prefix, which specifies lower index (e.g. '_' in LaTeX)
    /// </summary>
    public string LowerIndexPrefix { get; }

    /// <summary>
    /// Specifies whether print matrix indices or not.
    /// </summary>
    public bool PrintMatrixIndices { get; }

    private OutputFormat(OutputFormat format, bool printMatrixIndices)
        :this(format.Id, format.UpperIndexPrefix, format.LowerIndexPrefix, printMatrixIndices)
    {
    }

    private OutputFormat(int id, string upperIndexPrefix, string lowerIndexPrefix, bool printMatrixIndices = true)
    {
        Id = id;
        UpperIndexPrefix = upperIndexPrefix;
        LowerIndexPrefix = lowerIndexPrefix;
        PrintMatrixIndices = printMatrixIndices;
    }

    /// <summary>
    /// Returns whether this and oth defines same format.
    /// </summary>
    /// <param name="other">other format</param>
    /// <returns>whether this and oth defines same format</returns>
    public bool Is(OutputFormat other)
        => Id == other.Id;

    public string GetPrefixFromIntState(int intState)
    {
        return intState switch
        {
            0 => LowerIndexPrefix,
            1 => UpperIndexPrefix,
            _ => throw new ArgumentException("Not a state int")
        };
    }

    public string GetPrefixFromRawIntState(int rawIntState)
    {
        return rawIntState switch
        {
            0 => LowerIndexPrefix,
            unchecked((int)0x80000000) => UpperIndexPrefix,
            _ => throw new ArgumentException("Not a state int")
        };
    }

    public bool Equals(OutputFormat other)
    {
        return Id == other.Id
            && UpperIndexPrefix == other.UpperIndexPrefix
            && LowerIndexPrefix == other.LowerIndexPrefix
            && PrintMatrixIndices == other.PrintMatrixIndices;
    }

    public override bool Equals(object? obj)
        => obj is OutputFormat other && Equals(other);

    public override int GetHashCode()
        => HashCode.Combine(Id, UpperIndexPrefix, LowerIndexPrefix, PrintMatrixIndices);

    public static bool operator ==(OutputFormat left, OutputFormat right)
        => left.Equals(right);

    public static bool operator !=(OutputFormat left, OutputFormat right)
        => !left.Equals(right);
}