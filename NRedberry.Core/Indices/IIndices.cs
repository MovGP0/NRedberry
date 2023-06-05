using System;
using NRedberry.Core.Tensors;

namespace NRedberry.Core.Indices;

public interface IIndices : IEquatable<object>
{
    IntArray GetUpper();
    IntArray GetLower();
    IntArray GetAllIndices();
    int Size();
    int Size(IndexType type);
    uint this[uint position] { get; }
    uint this[IndexType type, uint position] { get; }
    IIndices GetFree();
    IIndices GetInverted();
    IIndices GetOfType(IndexType type);
    bool EqualsRegardlessOrder(IIndices indices);
    void TestConsistentWithException();
    IIndices ApplyIndexMapping(IIndexMapping mapping);
    string ToString(OutputFormat outputFormat);
    short[] GetDiffIds();
}