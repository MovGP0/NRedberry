using System;
using System.Collections.Generic;
using NRedberry.Core.Utils;

namespace NRedberry.Core.Indices;

public interface Indices : IEquatable<object>, IEnumerable<int>
{
    IntArray GetUpper();
    IntArray GetLower();
    IntArray GetAllIndices();
    IntArray AllIndices => GetAllIndices();
    int Size();
    int Size(IndexType type);
    int this[int position] { get; }
    int this[IndexType type, int position] { get; }
    Indices GetFree();
    Indices GetInverted();
    Indices GetOfType(IndexType type);
    bool EqualsRegardlessOrder(Indices indices);
    void TestConsistentWithException();
    Indices ApplyIndexMapping(IIndexMapping mapping);
    string ToString(OutputFormat outputFormat);
    short[] GetDiffIds();
}