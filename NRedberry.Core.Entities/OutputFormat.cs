﻿using System.Runtime.CompilerServices;

namespace NRedberry;

/// <summary>
/// Defines formats of string representation of expressions in Redberry.
/// </summary>
public struct OutputFormat : IEquatable<OutputFormat>
{
    /// <summary>
    /// This format specifies expressions to be outputted in the LaTeX notation. 
    /// The produced strings can be simply put in some LaTeX math environments and compiled via LaTeX compiler.
    /// </summary>
    public static readonly OutputFormat LaTeX = new(0, "^", "_");

    /// <summary>
    /// This format specifies Greek letters to be printed as is (if stdout supports UTF-8 characters).
    /// In other aspects, it is similar to <see cref="Redberry"/>.
    /// </summary>
    public static readonly OutputFormat UTF8 = new(1, "^", "_");

    /// <summary>
    /// This format specifies expressions to be outputted in the Redberry input notation.
    /// Produced strings can be parsed in Redberry.
    /// </summary>
    public static readonly OutputFormat Redberry = new(2, "^", "_");

    /// <summary>
    /// This format specifies expressions to be outputted in the Cadabra input notation.
    /// </summary>
    public static readonly OutputFormat Cadabra = new(3, "^", "_");

    /// <summary>
    /// This format specifies expressions to be outputted in the Wolfram Mathematica input notation.
    /// </summary>
    public static readonly OutputFormat WolframMathematica = new(4, "", "-");

    /// <summary>
    /// This format specifies expressions to be outputted in the Maplesoft Maple input notation.
    /// </summary>
    public static readonly OutputFormat Maple = new(5, "~", "");

    /// <summary>
    /// This format will not print explicit indices of matrices.
    /// E.g., it produces A*B instead of A^i'_j'*B^j'_k'.
    /// </summary>
    public static readonly OutputFormat SimpleRedberry = new(6, "^", "_", false);

    /// <summary>
    /// Format used to export expressions for C/C++.
    /// </summary>
    public static readonly OutputFormat C = new(7, "^", "_");

    /// <summary>
    /// Unique identifier.
    /// </summary>
    public int Id { get; }
    /// <summary>
    /// Prefix, which specifies the upper index (e.g., '^' in LaTeX).
    /// </summary>
    public string UpperIndexPrefix { get; }

    /// <summary>
    /// Prefix, which specifies the lower index (e.g., '_' in LaTeX).
    /// </summary>
    public string LowerIndexPrefix { get; }

    /// <summary>
    /// Specifies whether to print matrix indices or not.
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
    /// Returns an output format that will not print matrix indices.
    /// </summary>
    /// <returns>Output format that will not print matrix indices.</returns>
    public OutputFormat DoNotPrintMatrixIndices()
    {
        return PrintMatrixIndices ? new OutputFormat(this, false) : this;
    }

    /// <summary>
    /// Returns an output format that will always print matrix indices.
    /// </summary>
    /// <returns>Output format that will always print matrix indices.</returns>
    public OutputFormat PrintMatrixIndicesAlways()
    {
        return PrintMatrixIndices ? this : new OutputFormat(this, true);
    }

    /// <summary>
    /// Returns whether this and oth defines same format.
    /// </summary>
    /// <param name="other">other format</param>
    /// <returns>whether this and oth defines same format</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Is(OutputFormat other)
        => Id == other.Id;

    /// <summary>
    /// Returns the appropriate prefix based on the integer state.
    /// </summary>
    /// <param name="intState">Integer state (0 - lower, 1 - upper).</param>
    /// <returns>Prefix.</returns>
    public string GetPrefixFromIntState(int intState)
    {
        return intState switch
        {
            0 => LowerIndexPrefix,
            1 => UpperIndexPrefix,
            _ => throw new ArgumentException("Not a state int")
        };
    }

    /// <summary>
    /// Returns the appropriate prefix based on the raw integer state.
    /// </summary>
    /// <param name="rawIntState">Raw integer state (0 - lower, <see cref="int.MaxValue"/> - upper).</param>
    /// <returns>Prefix.</returns>
    public string GetPrefixFromRawIntState(int rawIntState)
    {
        return rawIntState switch
        {
            0 => LowerIndexPrefix,
            int.MaxValue => UpperIndexPrefix,
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