using System;
using NRedberry.Core.Tensors;

namespace NRedberry.Core.Indices;

public interface IIndices : IEquatable<object>
{
    IntArray GetUpper();
    IntArray GetLower();
    IntArray GetAllIndices();
    long Size();
    long Size(IndexType type);
    long this[long position] { get; }
    long this[IndexType type, long position] { get; }
    IIndices GetFree();
    IIndices GetInverted();
    IIndices GetOfType(IndexType type);
    bool EqualsRegardlessOrder(IIndices indices);
    void TestConsistentWithException();
    IIndices ApplyIndexMapping(IIndexMapping mapping);
    string ToString(OutputFormat outputFormat);
    short[] GetDiffIds();
}